using System.Text.Json;
using Amazon.Lambda.Core;
using Cognito.Functions.Core;
using Cognito.Functions.Events;

namespace Cognito.Functions.Handlers;

internal sealed class PreSignUpSignUpHandler : CognitoTriggerHandler<PreSignUpEvent>
{
    public const string TriggerSourceName = "PreSignUp_SignUp";
    
    public PreSignUpSignUpHandler(JsonElement @event, ILambdaLogger logger) : base(@event, logger)
    {
    }

    public override string TriggerSource => TriggerSourceName;

    public override JsonElement HandleTriggerEvent()
    {
        Logger.LogInformation($"Handling Cognito event trigger source '{TriggerSource}'.");
        
        Trigger.Response.AutoConfirmUser = true;

        return base.HandleTriggerEvent();
    }
}