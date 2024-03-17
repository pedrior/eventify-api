using System.Text.Json;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cognito.Functions;

public sealed class Function
{
    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.CamelCaseLambdaJsonSerializer))]
    public async Task<JsonElement> FunctionHandler(JsonElement @event, ILambdaContext context)
    {
        return await new CognitoEventHandlerFactory(context.Logger)
            .GetTriggerHandler(@event)
            .HandleTriggerEventAsync();
    }
}