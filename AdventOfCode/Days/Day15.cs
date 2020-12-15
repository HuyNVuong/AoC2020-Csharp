using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day15
    {
        // ReSharper disable InconsistentNaming
        public static void Solve()
        {
            var numbers = FileReader.ParseDataFromFile<int>("Inputs/Day15.txt", ',');

            
            var last2020thSpokenNumber = FindLastSpokenNumberFromTurn(numbers, 2020);
            Console.WriteLine(last2020thSpokenNumber);

            var last30000000thSpokenNumber = FindLastSpokenNumberFromTurn(numbers, 30000000);
            Console.WriteLine(last30000000thSpokenNumber);
        }

        private static int FindLastSpokenNumberFromTurn(List<int> numbers, int endTurn)
        {
            var numbersCalledInTurn = numbers
                .Select((n, i) => (n, i))
                .ToDictionary(tuple => tuple.n, tuple => new List<int> {tuple.i + 1});
            var turn = numbers.Count + 1;
            var lastSpoken = numbers.Last();
            for (;turn <= endTurn; turn++)
            {
                var timeStampLastSpokenNumber = numbersCalledInTurn[lastSpoken];
                int nextNumber;
                if (timeStampLastSpokenNumber.Count < 2)
                {
                    nextNumber = 0;
                }
                else
                {
                    var lastSpokenNumberLastSpoken = timeStampLastSpokenNumber.Last();
                    var secondLastSpokenNumberLastSpoken = timeStampLastSpokenNumber[^2];
                    nextNumber = lastSpokenNumberLastSpoken - secondLastSpokenNumberLastSpoken;
                }
                
                if (!numbersCalledInTurn.ContainsKey(nextNumber))
                    numbersCalledInTurn.Add(nextNumber, new List<int>());
                numbersCalledInTurn[nextNumber].Add(turn);
                lastSpoken = nextNumber;
            }

            return lastSpoken;
        }
    }
}
