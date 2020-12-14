using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day11
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day11.txt");
            var seatMap = lines
                .Select((line, y) => line.Select((p, x) => (new Position {X = x, Y = y}, p)))
                .SelectMany(pair => pair)
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.p);

            var filledSeatMap = FillPeopleInSeats(seatMap.ShallowClone());
            var seatsCount = filledSeatMap.Count(entry => entry.Value == '#');
            Console.WriteLine(seatsCount);

            var filledSeatMapFromView = FillPeopleInSeatsFromView(seatMap.ShallowClone());
            var seatsCountFromView = filledSeatMapFromView.Count(entry => entry.Value == '#');
            Console.WriteLine(seatsCountFromView);
        }

        private static Dictionary<Position, char> FillPeopleInSeats(Dictionary<Position, char> seatMap)
        {
            var nextMap = seatMap.ToDictionary(entry => entry.Key, entry => entry.Value);
            PrintMap(nextMap);

            var step = 0;
            
            while (true)
            {
                foreach (var p in seatMap.Keys)
                {
                    if (nextMap[p] == '.') continue;
                    if (p.Neighbors().All(neighbor => seatMap.GetValueOrDefault(neighbor) != '#')
                        && seatMap[p] == 'L')
                    {
                        nextMap[p] = '#';
                    }

                    if (p.Neighbors().Count(neighbor => seatMap.GetValueOrDefault(neighbor) == '#') > 3
                        && seatMap[p] == '#')
                    {
                        nextMap[p] = 'L';
                    }
                }

                if (IsSimilarMap(nextMap, seatMap))
                {
                    Console.WriteLine($"Settled after {step}");
                    PrintMap(nextMap);
                    break;
                }

                step++;
                seatMap = nextMap.ToDictionary(entry => entry.Key, entry => entry.Value);
            }

            return nextMap;
        }

        private static Dictionary<Position, char> FillPeopleInSeatsFromView(
            Dictionary<Position, char> seatMap)
        {
            PrintMap(seatMap);
            var step = 0;
            var nextMap = seatMap.ToDictionary(entry => entry.Key, entry => entry.Value);
            while (true)
            {
                foreach (var p in seatMap.Keys)
                {
                    if (nextMap[p] == '.') continue;
                    var seatsCountFromView = p.OccupiedSeatsCountFromView(seatMap);
                    if (seatsCountFromView == 0 && seatMap[p] == 'L') 
                        nextMap[p] = '#';
                    if (seatsCountFromView > 4 && seatMap[p] == '#') 
                        nextMap[p] = 'L';
                }
                //PrintMap(nextMap);
                if (IsSimilarMap(nextMap, seatMap))
                {
                    Console.WriteLine($"Settled after {step}");
                    PrintMap(nextMap);
                    break;
                }

                step++;
                seatMap = nextMap.ToDictionary(entry => entry.Key, entry => entry.Value);
            }

            return nextMap;
        }

        private static bool IsSimilarMap(
            Dictionary<Position, char> currentMap, 
            Dictionary<Position, char> previousMap
            ) => currentMap.Keys.All(key => currentMap[key] == previousMap[key]);


        private static void PrintMap(Dictionary<Position, char> seatMap)
        {
            Console.WriteLine("=======================");
            var minX = seatMap.Min(entry => entry.Key.X);
            var minY = seatMap.Min(entry => entry.Key.Y);
            var maxX = seatMap.Max(entry => entry.Key.X) + 1;
            var maxY = seatMap.Max(entry => entry.Key.Y) + 1;

            for (var y = minY; y < maxY; y++)
            {
                for (var x = minX; x < maxX; x++)
                {
                    Console.Write(seatMap[new Position {X = x, Y = y}]);
                }

                Console.WriteLine("");
            }

            Console.WriteLine("=======================");
        }
        
        private struct Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        private static List<Position> Neighbors(this Position p)
        {
            return new List<Position>
            {
                new Position {X = p.X, Y = p.Y - 1},
                new Position {X = p.X, Y = p.Y + 1},
                new Position {X = p.X - 1, Y = p.Y},
                new Position {X = p.X + 1, Y = p.Y},
                new Position {X = p.X - 1, Y = p.Y - 1},
                new Position {X = p.X - 1, Y = p.Y + 1},
                new Position {X = p.X + 1, Y = p.Y - 1},
                new Position {X = p.X + 1, Y = p.Y + 1}
            };
        }

        private static int OccupiedSeatsCountFromView(this Position p, Dictionary<Position, char> seatMap)
        {
            var seatSeenFromView = new bool[8];

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y == p.Y && entry.Key.X > p.X)
                .OrderBy(entry => entry.Key.X))
            {
                if (value == '#') 
                    seatSeenFromView[0] = true;
                if (value != '.') break;
            }

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y == p.Y && entry.Key.X < p.X)
                .OrderByDescending(entry => entry.Key.X))
            {
                if (value == '#')
                    seatSeenFromView[1] = true;
                if (value != '.') break;
            }

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y > p.Y && entry.Key.X == p.X)
                .OrderBy(entry => entry.Key.Y))
            {
                if (value == '#')
                    seatSeenFromView[2] = true;
                if (value != '.') break;
            }

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y < p.Y && entry.Key.X == p.X)
                .OrderByDescending(entry => entry.Key.Y))
            {
                if (value == '#')
                    seatSeenFromView[3] = true;
                if (value != '.') break;
            }

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y < p.Y && entry.Key.X - p.X == entry.Key.Y - p.Y)
                .OrderByDescending(entry => entry.Key.Y))
            {
                if (value == '#')
                    seatSeenFromView[4] = true;
                if (value != '.') break;
            }

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y > p.Y && entry.Key.X - p.X == entry.Key.Y - p.Y)
                .OrderBy(entry => entry.Key.Y))
            {
                if (value == '#')
                    seatSeenFromView[5] = true;
                if (value != '.') break;
            }

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y < p.Y && entry.Key.X - p.X == -(entry.Key.Y - p.Y))
                .OrderByDescending(entry => entry.Key.Y))
            {
                if (value == '#')
                    seatSeenFromView[6] = true;
                if (value != '.') break;
            }

            foreach (var (_, value) in seatMap
                .Where(entry => entry.Key.Y > p.Y && entry.Key.X - p.X == -(entry.Key.Y - p.Y))
                .OrderBy(entry => entry.Key.Y))
            {
                if (value == '#')
                    seatSeenFromView[7] = true;
                if (value != '.') break;
            }

            return seatSeenFromView.Count(s => s);
        }
    }
}
