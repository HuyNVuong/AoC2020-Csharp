using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day17
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day17.txt");
            var cubes = lines.GetCubes();

            for (var i = 0; i < 6; i++)
            {
                cubes = cubes.Cycle();
            }

            var activeCubesCount = cubes.Count;
            Console.WriteLine(activeCubesCount);
            
            cubes = lines.GetCubes();
            for (var i = 0; i < 6; i++)
            {
                Console.WriteLine($"Cycle: {i}");
                cubes = cubes.HyperCycle();
            }

            var activeHyperCubesCount = cubes.Count;
            Console.WriteLine(activeHyperCubesCount);
        }

        private static HashSet<Cube> Cycle(this HashSet<Cube> cubes)
        {
            var candidates = cubes
                .Select(cube => cube.Neighbors())
                .SelectMany(cluster => cluster.Where(n => !cubes.Contains(n)))
                .ToHashSet();

            var nextBatch = cubes.Where(cube =>
            {
                var neighborCount = cube.Neighbors().Count(cubes.Contains);
                return neighborCount == 2 || neighborCount == 3;
            }).ToHashSet();

            foreach (
                var candidate in from candidate in candidates
                let neighborCount = candidate.Neighbors().Count(cubes.Contains)
                where neighborCount == 3
                select candidate)
            {
                nextBatch.Add(candidate);
            }

            return nextBatch;
        }
        
        private static HashSet<Cube> HyperCycle(this HashSet<Cube> cubes)
        {
            var candidates = cubes
                .Select(cube => cube.HyperNeighbors())
                .SelectMany(cluster => cluster.Where(n => !cubes.Contains(n)))
                .ToHashSet();

            var nextBatch = cubes.Where(cube =>
            {
                var neighborCount = cube.HyperNeighbors().Count(cubes.Contains);
                return neighborCount == 2 || neighborCount == 3;
            }).ToHashSet();

            foreach (
                var candidate in from candidate in candidates
                let neighborCount = candidate.HyperNeighbors().Count(cubes.Contains)
                where neighborCount == 3
                select candidate)
            {
                nextBatch.Add(candidate);
            }

            return nextBatch;
        }

        private struct Cube
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int W { get; set; }
        }

        private static List<Cube> Neighbors(this Cube cube)
        {
            var cubes = new List<Cube>();
            for (var x = cube.X - 1; x <= cube.X + 1; x++)
            {
                for (var y = cube.Y - 1; y <= cube.Y + 1; y++)
                {
                    for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
                    {
                        var neighbor = new Cube {X = x, Y = y, Z = z};
                        if (Equals(cube, neighbor)) continue;
                        cubes.Add(neighbor);
                    }
                }
            }

            return cubes;
        }
        
        private static List<Cube> HyperNeighbors(this Cube cube)
        {
            var cubes = new List<Cube>();
            for (var x = cube.X - 1; x <= cube.X + 1; x++)
            {
                for (var y = cube.Y - 1; y <= cube.Y + 1; y++)
                {
                    for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
                    {
                        for (var w = cube.W - 1; w <= cube.W + 1; w++)
                        {
                            var neighbor = new Cube {X = x, Y = y, Z = z, W = w};
                            if (Equals(cube, neighbor)) continue;
                            cubes.Add(neighbor);
                        }
                    }
                }
            }

            return cubes;
        }

        private static HashSet<Cube> GetCubes(this IEnumerable<string> lines)
        {
            return lines
                .Select((line, y) => line
                    .Select((c, x) => (new Cube
                    {
                        X = x,
                        Y = y,
                        Z = 0
                    }, c)))
                .SelectMany(tuple => tuple.Where(t => t.c == '#'))
                .Select(tuple => tuple.Item1)
                .ToHashSet();
        }
    }
}
