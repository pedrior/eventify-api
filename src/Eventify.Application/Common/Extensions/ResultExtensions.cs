namespace Eventify.Application.Common.Extensions;

internal static class ResultExtensions
{
    public static string Describe(this Error error)
    {
        return $"{error.Code}: {error.Description}" + (error.Metadata is null
            ? string.Empty
            : $"\nAdditional information:\n{string.Join("\n",
                error.Metadata.Select(kv => $"- {kv.Key}: {kv.Value}"))}");
    }
}