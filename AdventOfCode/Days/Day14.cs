using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day14
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day14.txt");

            var memoryStorage = new Dictionary<long, long>();
            memoryStorage.PopulateMemoryStorage(lines);
            var memoryStorageSum = memoryStorage.Values.Sum();
            Console.WriteLine(memoryStorageSum);

            var floatingMemoryStorage = new Dictionary<long, long>();
            floatingMemoryStorage.PopulateFloatingMemoryStorage(lines);
            var floatingMemoryStorageSum = floatingMemoryStorage.Values.Sum();
            Console.WriteLine(floatingMemoryStorageSum);
        }

        private static void PopulateMemoryStorage(
            this Dictionary<long, long> memoryStorage,
            List<string> inputs)
        {
            var mask = Enumerable.Repeat('0', 64).AsStringRepr();
            foreach (var input in inputs)
            {
                var split = input.Split("=");
                var lhs = split[0].Trim();
                var rhs = split[1].Trim();
                if (lhs.Contains("mask"))
                {
                    mask = rhs;
                    continue;
                }

                var rhsValue = Convert.ToString(long.Parse(rhs), 2);
                rhsValue = $"{Enumerable.Repeat('0', mask.Length - rhsValue.Length).AsStringRepr()}{rhsValue}";
                var memoryAddress = long.Parse(Regex.Match(lhs, @"\d+").Value);
                var binaryValue = mask.Zip(rhsValue)
                    .Select(pair => pair.First == 'X' ? pair.Second : pair.First)
                    .AsStringRepr();
                var value = Convert.ToInt64(binaryValue, 2);
                memoryStorage.AddOrUpdateExisting(memoryAddress, value);
            }
        }

        private static void PopulateFloatingMemoryStorage(
            this Dictionary<long, long> floatingMemoryStorage,
            List<string> inputs)
        {
            var mask = Enumerable.Repeat('0', 64).AsStringRepr();
            var offsetValuesFromMask = new List<long>();
            foreach (var input in inputs)
            {
                var split = input.Split("=");
                var lhs = split[0].Trim();
                var rhs = split[1].Trim();
                if (lhs.Contains("mask"))
                {
                    mask = rhs;
                    var xIndexes = mask
                        .Select((c, i) => (c, i))
                        .Where(tuple => tuple.c == 'X')
                        .Select(tuple => tuple.i)
                        .ToList();
                    var baseMask = mask.Replace('X', '0');
                    var baseMaskValue = Convert.ToInt64(baseMask, 2);
                    var floatValuesFromMask = new List<string> { baseMask };
                    foreach (var index in xIndexes)
                    {
                        var newMasks = new List<string>();
                        if (floatValuesFromMask.Count == 1)
                        {
                            floatValuesFromMask.Add(floatValuesFromMask.First().ReplaceAt(index, '1'));
                            continue;
                        }
                        foreach (var m in floatValuesFromMask)
                        {
                            var newMask = m.ReplaceAt(index, '0');
                            newMasks.Add(newMask);
                            newMask = m.ReplaceAt(index, '1');
                            newMasks.Add(newMask);
                        }

                        floatValuesFromMask = newMasks;
                    }

                    offsetValuesFromMask = floatValuesFromMask
                        .Select(value => Convert.ToInt64(value, 2) - baseMaskValue)
                        .ToList();
                    continue;
                }

                var memoryAddress = long.Parse(Regex.Match(lhs, @"\d+").Value);
                var lhsValue = Convert.ToString(memoryAddress, 2);
                lhsValue = $"{Enumerable.Repeat('0', mask.Length - lhsValue.Length).AsStringRepr()}{lhsValue}";
                var rhsValue = long.Parse(rhs);
                var binaryValue = mask.Zip(lhsValue)
                    .Select(pair =>
                    {
                        if (pair.First == 'X')
                            return '0';
                        return pair.First == '0' ? pair.Second : pair.First;
                    })
                    .AsStringRepr();
                var baseValue = Convert.ToInt64(binaryValue, 2);
                var allFloatValues = offsetValuesFromMask
                    .Select(offset => offset + baseValue)
                    .ToList();
                foreach (var floatAddress in allFloatValues)
                {
                    floatingMemoryStorage.AddOrUpdateExisting(floatAddress, rhsValue);
                }
                
            }
        }
    }
}
