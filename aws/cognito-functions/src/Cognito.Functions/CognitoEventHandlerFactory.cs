using System.Text.Json;
using Amazon.Lambda.Core;
using Cognito.Functions.Core;
using Cognito.Functions.Handlers;

namespace Cognito.Functions;

internal sealed class CognitoEventHandlerFactory
{
    private readonly ILambdaLogger logger;

    public CognitoEventHandlerFactory(ILambdaLogger logger)
    {
        this.logger = logger;
    }

    public ICognitoTriggerHandler GetTriggerHandler(JsonElement @event)
    {
        var trigger = @event.Deserialize<CognitoTriggerEventBase>()!;
        return trigger.TriggerSource switch
        {
            PreSignUpSignUpHandler.TriggerSourceName => new PreSignUpSignUpHandler(@event, logger),
            _ => new NoActionHandler(@event, logger)
        };
    }
}