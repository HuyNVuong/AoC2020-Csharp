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
                .Where(bus => bus != "x")
                .Select(long.Parse)
                .ToList();

            var (id, time) = FindClosestBusIdToDepartTime(departTime, busServiceIds);
            var value = id * (time - departTime);
            Console.WriteLine(value);
        }

        private static (long, long) FindClosestBusIdToDepartTime(long departTime, List<long> busServiceIds)
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
    }
}
