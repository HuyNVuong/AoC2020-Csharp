using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day12
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day12.txt");
            var movementInstructions = lines.Select(line =>
            {
                var direction = line[0];
                var moveDistance = int.Parse(line.Substring(1));

                return (direction, moveDistance);
            }).ToList();

            var positions = movementInstructions.Move();
            var finalPositionDistance = positions.Last().ManhattanDistance(positions.First());
            Console.WriteLine(finalPositionDistance);

            var positionsFromWaypoint = movementInstructions.MoveWithWaypoint();
            var finalPositionDistanceFromWaypoint = positionsFromWaypoint.Last().ManhattanDistance(positionsFromWaypoint.First());
            Console.WriteLine(finalPositionDistanceFromWaypoint);
        }

        private static List<Position> Move(this List<(char, int)> movementInstructions)
        {
            var (dx, dy) = (1, 0); // East
            var x = 0;
            var y = 0;
            var positions = new List<Position> {new Position {X = x, Y = y}};
            foreach (var (direction, distance) in movementInstructions)
            {
                switch (direction)
                {
                    case 'N':
                        y -= distance;
                        positions.Add(new Position {X = x, Y = y});
                        break;
                    case 'S':
                        y += distance;
                        positions.Add(new Position {X = x, Y = y});
                        break;
                    case 'E':
                        x += distance;
                        positions.Add(new Position {X = x, Y = y});
                        break;
                    case 'W':
                        x -= distance;
                        positions.Add(new Position {X = x, Y = y});
                        break;
                    case 'L':
                        for (var i = 0; i < distance / 90; i++)
                            (dx, dy) = (dx, dy).TurnLeft();

                        break;
                    case 'R':
                        for (var i = 0; i < distance / 90; i++)
                            (dx, dy) = (dx, dy).TurnRight();

                        break;
                    case 'F':
                        x += dx * distance;
                        y += distance * dy;
                        positions.Add(new Position {X = x, Y = y});
                        break;
                }
            }

            return positions;
        }

        private static List<Position> MoveWithWaypoint(this List<(char, int)> movementInstructions)
        {
            var (dx, dy) = (10, -1); // East
            var x = 0;
            var y = 0;
            var positions = new List<Position> { new Position { X = x, Y = y } };

            foreach (var (direction, distance) in movementInstructions)
            {
                switch (direction)
                {
                    case 'N':
                        dy -= distance;
                        break;
                    case 'S':
                        dy += distance;
                        break;
                    case 'E':
                        dx += distance;
                        break;
                    case 'W':
                        dx -= distance;
                        break;
                    case 'L':
                        for (var i = 0; i < distance / 90; i++)
                            (dx, dy) = (dx, dy).TurnLeft();

                        break;
                    case 'R':
                        for (var i = 0; i < distance / 90; i++)
                            (dx, dy) = (dx, dy).TurnRight();

                        break;
                    case 'F':
                        x += dx * distance;
                        y += distance * dy;
                        positions.Add(new Position { X = x, Y = y });
                        break;
                }
            }

            return positions;
        }

        private class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public int ManhattanDistance(Position other)
            {
                return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
            }
        }

        private static (int, int) TurnRight(this (int Dx, int Dy) v)
        {
            return (-v.Dy, v.Dx);
        }

        private static (int, int) TurnLeft(this (int Dx, int Dy) v)
        {
            return (v.Dy, -v.Dx);
        }
    }
}
