using System.Text.Json;
using Amazon.Lambda.Core;

namespace Cognito.Functions.Core;

internal abstract class CognitoTriggerHandler<T> : ICognitoTriggerHandler where T : ICognitoTriggerEvent
{
    protected CognitoTriggerHandler(JsonElement @event, ILambdaLogger logger)
    {
        Trigger = @event.Deserialize<T>()!;
        Logger = logger;
    }

    protected T Trigger { get; }

    public abstract string TriggerSource { get; }

    protected ILambdaLogger Logger { get; }

    public virtual JsonElement HandleTriggerEvent() => JsonSerializer.SerializeToElement(Trigger);

    public virtual async Task<JsonElement> HandleTriggerEventAsync() => await Task.FromResult(HandleTriggerEvent());
}