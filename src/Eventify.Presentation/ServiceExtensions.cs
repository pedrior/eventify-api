using System.Text.Json;
using Eventify.Presentation.Common.Json.Serialization;
using Microsoft.AspNetCore.RateLimiting;

namespace Eventify.Presentation;

internal static class ServiceExtensions
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetConverter());
            });
        
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddSlidingWindowLimiter("default", configure =>
            {
                configure.PermitLimit = 15;
                configure.SegmentsPerWindow = 5;
                configure.Window = TimeSpan.FromSeconds(15);
            });
        });
        
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddMvc();
    }
}