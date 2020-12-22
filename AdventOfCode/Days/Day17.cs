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
            var initialCubes = lines
                .Select((line, y) => line
                    .Select((c, x) => (new Cube
                    {
                        X = x,
                        Y = y,
                        Z = 0
                    }, c == '#')))
                .SelectMany(tuple => tuple)
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        private struct Cube
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
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
                        if (Equals(neighbor, cube)) continue;
                        cubes.Add(neighbor);
                    }
                }
            }

            return cubes;
        }
    }
}
