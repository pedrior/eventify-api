namespace Cognito.Functions.Core;

internal interface ICognitoTriggerEvent
{
    string Version { get; }

    string TriggerSource { get; }

    string Region { get; }

    string UserPoolId { get; }

    string UserName { get; }

    CallerContext CallerContext { get; }
}