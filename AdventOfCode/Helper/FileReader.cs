using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Helper
{
    public static class FileReader
    {
        public static List<T> ParseDataFromFile<T>(string fileLocation, char delimiter = '\n')
        {
            var lines = File.ReadAllText(fileLocation)
                .Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => (T)Convert.ChangeType(line.Trim('\r', '\n'), typeof(T)))
                .ToList();

            return lines;
        }

        public static List<T> ParseDataFromFile<T>(string fileLocation, string delimiter)
        {
            var lines = File.ReadAllText(fileLocation)
                .Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => (T)Convert.ChangeType(line.Trim('\r', '\n'), typeof(T)))
                .ToList();

            return lines;
        }

        public static List<T> ParseDataFromFile<T>(string fileLocation, params char[] delimiter)
        {
            var lines = File.ReadAllText(fileLocation)
                .Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => (T)Convert.ChangeType(line.Trim('\r', '\n'), typeof(T)))
                .ToList();

            return lines;
        }
    }
}
