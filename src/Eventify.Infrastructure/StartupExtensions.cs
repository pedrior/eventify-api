using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Eventify.Infrastructure;

public static class StartupExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseAuthentication();

        app.UseAuthorization();
        
        app.UseSerilogRequestLogging();
    }
}