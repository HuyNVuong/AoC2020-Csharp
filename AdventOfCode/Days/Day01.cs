using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    class Day01
    {
        public static void Solve()
        {
            var values = FileReader.ParseDataFromFile<int>("Inputs/Day01.txt");

            var partAResult = TwoSum(values);
            Console.WriteLine(partAResult);

            var partBResult = ThreeSum(values);
            Console.WriteLine(partBResult);

        }

        private static int TwoSum(List<int> values)
        {
            var lookup = values.ToDictionary(value => 2020 - value, value => value);
            var (first, second) = lookup.First(entry => values.Contains(entry.Key));

            return first * second;
        }

        private static int ThreeSum(List<int> values)
        {
            var sorted = values.Select(x => x).OrderBy(x => x).ToList();
            for (var i = 0; i < sorted.Count; i++)
            {
                var firstNumber = sorted[i];
                var currentExpected = 2020 - firstNumber;
                int l = i + 1, r = sorted.Count - 1;
                while (l < r)
                {
                    if (sorted[l] + sorted[r] < currentExpected)
                        l++;
                    else if (sorted[l] + sorted[r] > currentExpected)
                        r--;
                    else
                    {
                        return sorted[i] * sorted[l] * sorted[r];
                    }
                }
            }



            return -1;
        }
    }
}
