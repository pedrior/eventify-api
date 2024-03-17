using System.Text.Json.Serialization;

namespace Cognito.Functions.Events;

internal sealed record PreSignUpResponse
{
    [JsonPropertyName("autoConfirmUser")]
    public bool AutoConfirmUser { get; set; }

    [JsonPropertyName("autoVerifyPhone")]
    public bool AutoVerifyPhone { get; set; }

    [JsonPropertyName("autoVerifyEmail")]
    public bool AutoVerifyEmail { get; set; }
}