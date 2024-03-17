using System.Text.Json.Serialization;

namespace Cognito.Functions.Core;

internal sealed record CallerContext
{
    [JsonPropertyName("awsSdkVersion")]
    public string AwsSdkVersion { get; set; } = default!;

    [JsonPropertyName("clientId")]
    public string ClientId { get; set; } = default!;
}