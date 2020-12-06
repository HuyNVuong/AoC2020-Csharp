using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day05
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day05.txt");

            var seats = GetSeatsFromData(lines);
            var highestId = GetHighestIdFromSeats(seats);
            Console.WriteLine(highestId);

            var designatedSeatId = FindDesignatedSeatId(seats);
            Console.WriteLine(designatedSeatId);
        }

        private static int GetHighestIdFromSeats(List<Seat> seats)
        {
            var highestSeatId = seats.Max(seat => seat.Row * 8 + seat.Column);

            return highestSeatId;
        }

        private static int FindDesignatedSeatId(List<Seat> seats)
        {
            var seatIds = seats.Select(seat => seat.Row * 8 + seat.Column).ToList();
            var low = seatIds.Min();
            var high = seatIds.Max();
            var range = Enumerable.Range(low, high).ToList();
            var designatedSeatId = range.First(id => !seatIds.Contains(id));

            return designatedSeatId;
        }

        private static List<Seat> GetSeatsFromData(List<string> lines)
        {
            var seatRows = lines
                .Select(line => line.Substring(0, 7))
                .Select(line => line.Select(c => c == 'B' ? '1' : '0').ToStringRepr())
                .Select(binary => Convert.ToInt32(binary, 2))
                .ToList();

            var seatColumns = lines
                .Select(line => line.Skip(7).Select(c => c == 'R' ? '1' : '0').ToStringRepr())
                .Select(binary => Convert.ToInt32(binary, 2))
                .ToList();

            var seats = seatRows.Zip(seatColumns)
                .Select(pair => new Seat
                {
                    Row = pair.First,
                    Column = pair.Second
                }).ToList();

            return seats;
        }

        private class Seat
        {
            public int Row { get; set; }
            public int Column { get; set; }
        }
    }
}
