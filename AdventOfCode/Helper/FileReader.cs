using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Helper
{
    public static class FileReader
    {
        public static List<T> ParseDataFromFile<TDelimiter, T>(string fileLocation, params TDelimiter[] delimiters)
        {
            var delimiterCombine = string.Join("", delimiters);
            var lines = File.ReadAllText(fileLocation)
                .Split(delimiterCombine, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => (T)Convert.ChangeType(line.Trim('\r', '\n'), typeof(T)))
                .ToList();

            return lines;
        }
    }
}
