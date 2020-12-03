using System;
using System.Collections.Generic;

namespace AdventOfCode.Helper
{
    public static class EnumerableExtensions
    {
        public static string ToStringRepr<TValue>(this IEnumerable<TValue> enumerable, string delimiter = "")
        {
            if (enumerable == null)
                throw new ArgumentNullException();
            return string.Join(delimiter, enumerable);
        }
    }
}
