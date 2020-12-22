using System;

namespace AdventOfCode.Helper
{
    public static class StringExtensions
    {
        public static string ReplaceAt(this string currentString, int index, char replaceValue)
        {
            if (string.IsNullOrEmpty(currentString))
                throw new ArgumentException("Provided string need to have length of at least 1");
            if (index >= currentString.Length)
                throw new ArgumentException("Index offset starts at 0 and must smaller or equal to current string length: " +
                                            $"{index}, {currentString.Length}");

            var chars = currentString.ToCharArray();
            chars[index] = replaceValue;

            return new string(chars);
        }
    }
}
