using System.Text.Json.Serialization;
using Cognito.Functions.Core;

namespace Cognito.Functions.Events;

internal sealed record PreSignUpRequest : Request
{
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; } = null!;

    [JsonPropertyName("validationData")]
    public Dictionary<string, string> ValidationData { get; } = null!;
}