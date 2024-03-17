using System.Text.Json.Serialization;

namespace Cognito.Functions.Core;

internal record CognitoTriggerEvent<TRequest, TResponse> : CognitoTriggerEventBase where TRequest : Request
{
    [JsonPropertyName("request")]
    public TRequest Request { get; set; } = default!;

    [JsonPropertyName("response")]
    public TResponse Response { get; set; } = default!;
}