using Cognito.Functions.Core;

namespace Cognito.Functions.Events;

internal sealed record PreSignUpEvent : CognitoTriggerEvent<PreSignUpRequest, PreSignUpResponse>;