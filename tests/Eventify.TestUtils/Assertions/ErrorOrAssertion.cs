using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Eventify.TestUtils.Assertions;

public sealed class ErrorOrAssertion<T>(ErrorOr<T> subject)
    : ReferenceTypeAssertions<ErrorOr<T>, ErrorOrAssertion<T>>(subject)
{
    protected override string Identifier => "ErrorOr";

    public AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>> BeValue(
        string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.IsError)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:IsError} to be a {0}, but found {1}.", false, Subject.IsError);

        return new AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>>(this, Subject);
    }

    public AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>> BeValue(
        T value, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.IsError)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:IsError} to be a {0}, but found {1} with error: {2}.",
                false, Subject.IsError, Subject.FirstError)
            .Then
            .ForCondition(Subject.Value!.Equals(value))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Value} to be {0}, but found {1}.", value, Subject.Value);

        return new AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>>(this, Subject);
    }

    public AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>> BeError(
        string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.IsError)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:IsError} to be an {0}, but found {1}.", true, Subject.IsError);

        return new AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>>(this, Subject);
    }

    public AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>> BeError(
        Error error, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.IsError)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:IsError} to be a {0}, but found {1} with value: {2}.",
                true, Subject.IsError, Subject.Value)
            .Then
            .ForCondition(ErrorOrEqualityComparer.Instance.Equals(Subject.FirstError, error))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:FirstError} to be {0}, but found {1}.", error, Subject.FirstError);

        return new AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>>(this, Subject);
    }

    public AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>> BeError(
        Error[] errors, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.IsError)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:IsError} to be a {0}, but found {1} with value: {2}.",
                true, Subject.IsError, Subject.Value)
            .Then
            .ForCondition(Subject.Errors.TrueForAll(e => errors.Contains(e, ErrorOrEqualityComparer.Instance)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Errors} to be {0}, but found {1}.", errors, Subject.Errors);

        return new AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>>(this, Subject);
    }

    public AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>> BeErrorSequentially(
        Error[] errors, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.IsError)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:IsError} to be a {0}, but found {1} with value: {2}.",
                true, Subject.IsError, Subject.Value)
            .Then
            .ForCondition(Subject.Errors.SequenceEqual(errors, ErrorOrEqualityComparer.Instance))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Errors} to be {0}, but found {1}.", errors, Subject.Errors);

        return new AndWhichConstraint<ErrorOrAssertion<T>, ErrorOr<T>>(this, Subject);
    }
}

file sealed class ErrorOrEqualityComparer : IEqualityComparer<Error>
{
    public static readonly ErrorOrEqualityComparer Instance = new();

    public bool Equals(Error x, Error y)
    {
        return x.NumericType == y.NumericType && x.Code == y.Code && x.Description == y.Description &&
               (x.Metadata is null && y.Metadata is null || x.Metadata is not null && y.Metadata is not null &&
                   x.Metadata.SequenceEqual(y.Metadata));
    }

    public int GetHashCode(Error obj) => obj.GetHashCode();
}