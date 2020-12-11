using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day10
    {
        public static void Solve()
        {
            var adapters = FileReader.ParseDataFromFile<int>("Inputs/Day10.txt");
            var deviceJoltageAdapter = adapters.Max() + 3;
            adapters.Add(0);
            adapters.Add(deviceJoltageAdapter);

            var joltDifferences = CountJoltDifferences(adapters);
            Console.WriteLine(joltDifferences);

            var totalAdapterCombination = CountTotalAdaptersCombination(adapters);
            Console.WriteLine(totalAdapterCombination);
        }

        public static int CountJoltDifferences(List<int> adapters)
        {
            adapters = adapters.OrderBy(x => x).ToList();
            var oneDifferenceCount = 0;
            var threeDifferenceCount = 0;
            for (var i = 1; i < adapters.Count; i++)
            {
                if (adapters[i] - adapters[i - 1] == 1)
                    oneDifferenceCount++;
                if (adapters[i] - adapters[i - 1] == 3)
                    threeDifferenceCount++;
            }

            return oneDifferenceCount * threeDifferenceCount;
        }

        public static long CountTotalAdaptersCombination(List<int> adapters)
        {
            var combinations = adapters.Last();
            var dp = new long[combinations + 1].ToList();
            dp[0] = 1;
            dp[1] = 1;
            dp[2] = 2;

            for (var n = 3; n <= combinations; n++)
            {
                if (!adapters.Contains(n)) continue;
                dp[n] = dp[n - 3] + dp[n - 2] + dp[n - 1];
            }
           
            return dp[combinations];
        }
    }
}
