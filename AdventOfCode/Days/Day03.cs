using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day03
    {
        private const char Tree = '#';

        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day03.txt");
            var tiles = lines
                .Select((line, y) => line.Select((c, x) => (new Tile {X = x, Y = y}, c)))
                .SelectMany(x => x)
                .ToDictionary(entry => entry.Item1, entry => entry.c);

            var treeCount = CountTreeFromTraversingToBottom(tiles);
            Console.WriteLine(treeCount);

            var treeCountForAllVelocities = CountTreeFromTraversingToBottom(tiles, 1, 1)
                                            * CountTreeFromTraversingToBottom(tiles, 3, 1)
                                            * CountTreeFromTraversingToBottom(tiles, 5, 1)
                                            * CountTreeFromTraversingToBottom(tiles, 7, 1)
                                            * CountTreeFromTraversingToBottom(tiles, 1, 2);
            Console.WriteLine(treeCountForAllVelocities);
        }

        private static long CountTreeFromTraversingToBottom(Dictionary<Tile, char> tiles, int dx = 3, int dy = 1)
        {
            var position = new Tile {X = 0, Y = 0};
            var m = tiles.Keys.Max(key => key.Y) + 1;
            var n = tiles.Keys.Max(key => key.X) + 1;
            var treeCount = 0;
            while (position.Y < m)
            {
                if (tiles[position] == Tree)
                    treeCount++;
                position.Y += dy;
                position.X = (position.X + dx) % n;
            }

            return treeCount;
        }

        private struct Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
