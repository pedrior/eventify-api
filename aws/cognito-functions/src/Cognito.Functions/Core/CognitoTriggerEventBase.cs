using System.Text.Json.Serialization;

namespace Cognito.Functions.Core;

internal record CognitoTriggerEventBase : ICognitoTriggerEvent
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = default!;

    [JsonPropertyName("triggerSource")]
    public string TriggerSource { get; set; } = default!;

    [JsonPropertyName("region")]
    public string Region { get; set; } = default!;

    [JsonPropertyName("userPoolId")]
    public string UserPoolId { get; set; } = default!;

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = default!;

    [JsonPropertyName("callerContext")]
    public CallerContext CallerContext { get; set; } = default!;
}