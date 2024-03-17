namespace Eventify.TestUtils.Assertions;

public static class ErrorOrExtensions
{
    public static ErrorOrAssertion<T> Should<T>(this ErrorOr<T> subject) => new(subject);
}