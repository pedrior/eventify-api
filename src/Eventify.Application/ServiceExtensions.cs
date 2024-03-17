using System.Reflection;
using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Behaviors;
using Eventify.Application.Common.Mappings.Transforms;
using Eventify.Application.Common.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Eventify.Application;

public static class ServiceExtensions
{
    private static readonly Assembly ApplicationAssembly = typeof(ServiceExtensions).Assembly;

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMapper();
        services.AddMediator();
        services.AddValidator();

        RegisterGenericTypes(services, typeof(IAuthorizer<>), ServiceLifetime.Scoped);
        RegisterGenericTypes(services, typeof(IRequirementHandler<>), ServiceLifetime.Scoped);

        return services;
    }

    private static void AddMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(ApplicationAssembly);

        config.Default.AddDestinationTransform(StringTransformFunctions.Trim);

        TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;

        services.AddSingleton(config);
    }

    private static void AddMediator(this IServiceCollection services) => services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(ApplicationAssembly);

        config.AddOpenBehavior(typeof(ExceptionBehavior<,>))
            .AddOpenBehavior(typeof(LoggingBehavior<,>))
            .AddOpenBehavior(typeof(AuthorizationBehavior<,>))
            .AddOpenBehavior(typeof(ValidationBehavior<,>))
            .AddOpenBehavior(typeof(TransactionBehavior<,>));
    });

    private static void AddValidator(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(ApplicationAssembly, includeInternalTypes: true);

        ValidatorOptions.Global.PropertyNameResolver = PropertyNameResolvers.SnakeCaseResolver;
    }

    private static void RegisterGenericTypes(this IServiceCollection services, Type genericType,
        ServiceLifetime lifetime)
    {
        var implementationTypes = GetTypesImplementingGenericType(ApplicationAssembly, genericType);
        foreach (var implementationType in implementationTypes)
        {
            foreach (var interfaceType in implementationType.ImplementedInterfaces)
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    services.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));
                }
            }
        }
    }

    private static List<TypeInfo> GetTypesImplementingGenericType(Assembly assembly, Type genericType)
    {
        if (!genericType.IsGenericType)
        {
            throw new ArgumentException("Must be a generic type", nameof(genericType));
        }

        return assembly.DefinedTypes
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType))
            .ToList();
    }
}