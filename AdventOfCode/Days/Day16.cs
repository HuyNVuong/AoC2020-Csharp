using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day16
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string, string>(
                "Inputs/Day16.txt", 
                Environment.NewLine, 
                Environment.NewLine);
            var fields = lines[0].Split('\n').Select(field =>
            {
                var fieldName = field.Split(':')[0].Trim();
                var values = field.Split(':')[1].Trim().Split("or").Select(s => s.Trim());
                return values.Select(value =>
                {
                    var lowerUpper = value.Split('-');

                    return (int.Parse(lowerUpper[0].Trim()), int.Parse(lowerUpper[1].Trim()), fieldName);
                }).ToList();
            }).SelectMany(x => x).ToList();

            var yourTicket = lines[1].Split('\n').Skip(1).ToList()[0]
                .Split(',').Select(int.Parse).ToList();

            var nearbyTickets = lines[2]
                .Split('\n').Skip(1)
                .Select(line => line.Trim().Split(','))
                .Select(line => line.Select(int.Parse).ToList())
                .ToList();

            var errorRate = FindScanningErrorRate(fields, nearbyTickets);
            Console.WriteLine(errorRate);

            var departureValues = FindDepartureProducts(fields, yourTicket, nearbyTickets);
            Console.WriteLine(departureValues);
        }

        private static int FindScanningErrorRate(
            IReadOnlyList<(int Lower, int Upper, string Name)> fields,
            IReadOnlyList<List<int>> nearbyTickets)
        {
            var errorRate = nearbyTickets
                .SelectMany(x => x)
                .Where(ticket => fields
                    .All(field => ticket < field.Lower || ticket > field.Upper))
                .Sum();

            return errorRate;
        }

        private static long FindDepartureProducts(
            IReadOnlyList<(int Lower, int Upper, string Name)> fields,
            IReadOnlyList<int> yourTicket,
            IReadOnlyList<List<int>> nearbyTickets)
        {
            var validTickets = nearbyTickets
                .Where(ticket => ticket.All(ticketValue => fields
                    .Any(field => ticketValue >= field.Item1 && ticketValue <= field.Item2)))
                .ToList();

            var validTicketTranspose = validTickets.Transpose();

            var candidatesForTicketValue = validTicketTranspose
                .Select(column => column
                    .Select(ticketValue => fields
                        .Where(field => ticketValue >= field.Item1 && ticketValue <= field.Item2)
                        .Select(field => field.Name)
                        .ToList())
                    .ToList())
                .ToList();

            var orderedCandidates = candidatesForTicketValue.Select((f, i) =>
            {
                var intersect = f[0];
                foreach (var next in f)
                {
                    intersect = intersect.Intersect(next).ToList();
                }

                return (intersect, i);
            }).OrderBy(tuple => tuple.intersect?.Count ?? -1).ToList();

            var seen = new HashSet<string>();
            var labelMap = new Dictionary<int, string>();
            foreach (var (intersect, index) in orderedCandidates)
            {
                var label = intersect.First(l => !seen.Contains(l));
                labelMap.Add(index, label);
                seen.Add(label);
            }

            var yourTicketToValue = labelMap.ToDictionary(entry => entry.Value,
                entry => yourTicket[entry.Key]);

            long departureProducts = 1;
            foreach (var (label, value) in yourTicketToValue)
            {
                if (label.Contains("departure"))
                    departureProducts *= value;
            }

            return departureProducts;
        }
    }
}
