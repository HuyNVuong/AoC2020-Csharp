﻿using System;
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
                .Split(delimiter)
                .Select(line => (T)Convert.ChangeType(line.Trim('\r', '\n'), typeof(T)))
                .ToList();

            return lines;
        }
    }
}
