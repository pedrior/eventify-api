using System.Text.Json;

namespace Cognito.Functions.Core;

internal interface ICognitoTriggerHandler
{
    string TriggerSource { get; }

    JsonElement HandleTriggerEvent();
    
    Task<JsonElement> HandleTriggerEventAsync();
}