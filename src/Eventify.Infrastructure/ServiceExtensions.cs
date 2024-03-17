using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using Amazon.S3;
using Eventify.Application.Common.Abstractions.Auth;
using Eventify.Application.Common.Abstractions.Data;
using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Bookings.Services;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.Services;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Users;
using Eventify.Infrastructure.Attendees.Data;
using Eventify.Infrastructure.Bookings.Data;
using Eventify.Infrastructure.Common.Auth;
using Eventify.Infrastructure.Common.Data;
using Eventify.Infrastructure.Common.Data.Interceptors;
using Eventify.Infrastructure.Common.Identity;
using Eventify.Infrastructure.Common.Storage;
using Eventify.Infrastructure.Events;
using Eventify.Infrastructure.Events.Data;
using Eventify.Infrastructure.Producers.Data;
using Eventify.Infrastructure.Tickets.Data;
using Eventify.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Eventify.Infrastructure;

public static class ServiceExtensions
{
    private static readonly RegionEndpoint AwsDefaultRegion = RegionEndpoint.GetBySystemName(
        Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION")!);

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddStorage(configuration);
        services.AddAuthentication(configuration);
        services.AddAuthorization();
        services.AddIdentity();

        services.AddTransient<IBookingService, BookingService>();
        services.AddTransient<IEventSlugUniquenessChecker, EventSlugUniquenessChecker>();
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        // https://www.npgsql.org/doc/types/json.html#poco-mapping
        NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
#pragma warning restore CS0618 // Type or member is obsolete

        // https://stackoverflow.com/a/73586129/21190599
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<DataContext>((provider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), pgsqlOptions =>
                pgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

            options.AddInterceptors(
                provider.GetRequiredService<AuditableInterceptor>(),
                provider.GetRequiredService<SoftDeleteInterceptor>(),
                provider.GetRequiredService<EventDispatchInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<DataContext>());

        services.AddScoped<AuditableInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<EventDispatchInterceptor>();

        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IProducerRepository, ProducerRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
    }

    private static void AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<StorageOptions>()
            .Bind(configuration.GetRequiredSection(StorageOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(
                accessKey: Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!,
                secretKey: Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!),
            region: AwsDefaultRegion));

        services.AddTransient<IStorageService, StorageService>();
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://cognito-idp.{AwsDefaultRegion.SystemName}" +
                                    $".amazonaws.com/{configuration["AWS:UserPoolId"]}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddScoped<IAuthService, AuthService>();
    }

    private static void AddIdentity(this IServiceCollection services)
    {
        services.AddCognitoIdentity();

        services.AddScoped<IUser, UserContext>();
        services.AddScoped<IIdentityService, IdentityService>();

        var serviceDescriptor = services.FirstOrDefault(descriptor =>
            descriptor.ServiceType == typeof(IAmazonCognitoIdentityProvider));

        if (serviceDescriptor is not null)
        {
            services.Remove(serviceDescriptor);
        }

        services.AddSingleton<IAmazonCognitoIdentityProvider>(
            _ => new AmazonCognitoIdentityProviderClient(new BasicAWSCredentials(
                    accessKey: Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!,
                    secretKey: Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!),
                region: AwsDefaultRegion));
    }
}