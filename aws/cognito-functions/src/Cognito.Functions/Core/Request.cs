using System.Text.Json.Serialization;

namespace Cognito.Functions.Core;

internal record Request
{
    [JsonPropertyName("userAttributes")]
    public Dictionary<string, string> UserAttributes { get; set; } = default!;
}