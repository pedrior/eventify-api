namespace Eventify.Presentation;

internal static class StartupExtensions
{
    public static void UsePresentation(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseRateLimiter();

        app.MapControllers();
    }
}