using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day13
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day13.txt");

            var departTime = long.Parse(lines[0]);
            var busServiceIds = lines[1].Split(',')
                .Select(bus => bus == "x" ? "-1" : bus)
                .Select(long.Parse)
                .ToList();

            var (id, time) = FindClosestBusIdToDepartTime(departTime, busServiceIds.Where(bus => bus > 0));
            var value = id * (time - departTime);
            Console.WriteLine(value);

            var earliestMatchingTime = FindEarliestMatching(busServiceIds);
            Console.WriteLine(earliestMatchingTime);
        }

        private static (long, long) FindClosestBusIdToDepartTime(long departTime, IEnumerable<long> busServiceIds)
        {
            var closestMultiples = busServiceIds.Select(id =>
            {
                var n = departTime;
                n += id / 2;
                n -= n % id;

                return (id, n > departTime ? n : n + id);
            });

            var closestId = closestMultiples.OrderBy(tuple => tuple.Item2 - departTime).First();

            return closestId;
        }

        private static long FindEarliestMatching(List<long> busServiceIds)
        {
            var busServiceIdDictionary = busServiceIds
                .Select((id, i) => (id, i))
                .Where(tuple => tuple.id != -1)
                .ToDictionary(tuple => tuple.i, tuple => tuple.id);

            long increment = 1;
            long solution = 0;
            foreach (var (i, id) in busServiceIdDictionary)
            {
                while ((solution + i) % id != 0)
                {
                    solution += increment;
                }

                increment *= id;
            }

            return solution;
        }
    }
}
