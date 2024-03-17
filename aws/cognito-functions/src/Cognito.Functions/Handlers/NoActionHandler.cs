using System.Text.Json;
using Amazon.Lambda.Core;
using Cognito.Functions.Core;

namespace Cognito.Functions.Handlers;

internal sealed class NoActionHandler : CognitoTriggerHandler<CognitoTriggerEventBase>
{
    public override string TriggerSource => string.Empty;

    public NoActionHandler(JsonElement @event, ILambdaLogger logger)
        : base(@event, logger)
    {
    }

    public override JsonElement HandleTriggerEvent()
    {
        Logger.LogInformation("No action taken.");

        return base.HandleTriggerEvent();
    }
}