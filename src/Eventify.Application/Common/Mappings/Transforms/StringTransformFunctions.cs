using System.Linq.Expressions;

namespace Eventify.Application.Common.Mappings.Transforms;

internal static class StringTransformFunctions
{
    public static readonly Expression<Func<string, string>> Trim = str =>
        string.IsNullOrEmpty(str) ? str : str.Trim();
}