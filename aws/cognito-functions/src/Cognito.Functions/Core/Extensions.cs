namespace Cognito.Functions.Core;

internal static class Extensions
{
    public static bool AnyBaseType(this Type type, Func<Type, bool> predicate) =>
        type.BaseTypes().Any(predicate);

    public static bool IsParticularGeneric(this Type type, Type generic) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == generic;
    
    private static IEnumerable<Type> BaseTypes(this Type type)
    {
        var t = type;
        while (true)
        {
            t = t.BaseType;
            if (t is null)
            {
                break;
            }

            yield return t;
        }
    }
}