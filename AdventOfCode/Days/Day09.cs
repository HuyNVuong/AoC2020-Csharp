using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day09
    {
        public static void Solve()
        {
            var sequence = FileReader.ParseDataFromFile<long>("Inputs/Day09.txt");
            var preambleSequence = BuildPreambleSequence(sequence);

            var firstInvalidProperty = preambleSequence.First(tuple => !tuple.IsValidProperty).Number;
            Console.WriteLine(firstInvalidProperty);

            var (starting, ending) = FindEncryptionWeakness(sequence, firstInvalidProperty);
            var weakestChain = sequence.GetRange(starting, ending - starting + 1);
            Console.WriteLine(weakestChain.Sum());
            Console.WriteLine(weakestChain.Min() + weakestChain.Max());
        }

        private static List<(long Number, bool IsValidProperty)> BuildPreambleSequence(List<long> sequence)
        {
            var preambleSequence = sequence.Select((number, i) =>
            {
                if (i < 25) return (number, true);
                var preamble = sequence.GetRange(i - 25, 25);
                var hasProperty = TwoSum(preamble, number);

                return (number, hasProperty);
            }).ToList();

            return preambleSequence;
        }

        private static (int, int) FindEncryptionWeakness(List<long> sequence, long weakness)
        {
            var currentSum = sequence[0];
            var startingIndex = 0;

            for (var i = 1; i <= sequence.Count; i++)
            {
                while (currentSum > weakness && startingIndex < i - 1)
                {
                    currentSum -= sequence[startingIndex];
                    startingIndex++;
                }

                if (currentSum == weakness) 
                    return (startingIndex, i - 1);

                if (i < sequence.Count)
                    currentSum += sequence[i];
            }

            return (-1, -1);
        }

        private static bool TwoSum(List<long> values, long expect)
        {
            var lookup = values.ToDictionary(value => expect - value, value => value);
            var hasProperty = !lookup.FirstOrDefault(entry => values.Contains(entry.Key))
                .Equals(default(KeyValuePair<long, long>));

            return hasProperty;
        }
    }
}
