using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Helper
{
    public static class EnumerableExtensions
    {
        public static string AsStringRepr<TValue>(this IEnumerable<TValue> enumerable, string delimiter = "")
        {
            if (enumerable == null)
                throw new ArgumentNullException();
            return string.Join(delimiter, enumerable);
        }

        public static List<List<TValue>> Transpose<TValue>(this IEnumerable<IEnumerable<TValue>> enumerable)
        {
            var list = enumerable.Select(inner => inner.ToList()).ToList();

            var transposedList = list
                .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => g.ToList())
                .ToList();

            return transposedList;
        }
    }
}
