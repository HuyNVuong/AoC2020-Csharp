using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day07
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day07.txt");
            var unWeightedGraph = lines.Select(line =>
            {
                var splits = line.Split("contain", StringSplitOptions.RemoveEmptyEntries);
                var sourceBag = splits[0].Trim().Split().Take(2).AsStringRepr();

                if (splits[1].Contains("no")) return (sourceBag, new List<string>());

                var edgeBags = splits[1].Trim().Split(',')
                    .Select(bagData => bagData.Trim().Split().Skip(1).Take(2).AsStringRepr())
                    .ToList();
                return (sourceBag, edgeBags);
            }).ToDictionary(tuple => tuple.sourceBag, tuple => tuple.Item2);

            var weightedGraph = lines.Select(line =>
            {
                var splits = line.Split("contain", StringSplitOptions.RemoveEmptyEntries);
                var sourceBag = splits[0].Trim().Split().Take(2).AsStringRepr();

                if (splits[1].Contains("no")) return (sourceBag, new List<Bag>());

                var edgeBags = splits[1].Trim().Split(',')
                    .Select(bagData =>
                    {
                        var bagDataSplits = bagData.Trim().Split();
                        var requirementFromSource = int.Parse(bagDataSplits[0]);
                        var edgeBag = bagDataSplits.Skip(1).Take(2).AsStringRepr();
                        return new Bag
                        {
                            ColorCode = edgeBag,
                            Requirement = requirementFromSource
                        };
                    })
                    .ToList();
                return (sourceBag, edgeBags);
            }).ToDictionary(tuple => tuple.sourceBag, tuple => tuple.Item2);

            Console.WriteLine(BagsContainsShinyGoldBagCount(unWeightedGraph));

            var bagContainsInShinyGoldBag = BagsContainsInShinyGoldBag(weightedGraph);
        }

        private static int BagsContainsShinyGoldBagCount(IReadOnlyDictionary<string, List<string>> unWeightedGraph)
        {
            var bagCount = 0;
            foreach (var bag in unWeightedGraph.Keys)
            {
                if (Bfs(unWeightedGraph, bag, "shinygold"))
                    bagCount++;
            }

            return bagCount;
        }

        private static int BagsContainsInShinyGoldBag(IReadOnlyDictionary<string, List<Bag>> weightedGraph)
        {
            var queue = new Queue<Bag>();
            var seen = new HashSet<string>();
            queue.Enqueue(new Bag{ColorCode = "shinygold", Requirement = 1});
            var totalRequirement = 0;
            while (queue.Any())
            {
                var currentBag = queue.Dequeue();
                totalRequirement += currentBag.Requirement;

                if (!weightedGraph.ContainsKey(currentBag.ColorCode)) continue;
                foreach (var edge in weightedGraph[currentBag.ColorCode])
                {
                    //if (seen.Contains(edge.ColorCode)) continue;
                    queue.Enqueue(new Bag{ColorCode = edge.ColorCode, Requirement = currentBag.Requirement * edge.Requirement});
                }

                seen.Add(currentBag.ColorCode);
            }

            return totalRequirement - 1;
        }

        private static bool Bfs(
            IReadOnlyDictionary<string, List<string>> unWeightedGraph, 
            string source,
            string destination)
        {
            var seen = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue(source);
            while (queue.Any())
            {
                var bag = queue.Dequeue();
           
                if (!unWeightedGraph.ContainsKey(bag)) continue;
                foreach (var edge in unWeightedGraph[bag])
                {
                    if (edge == destination) return true;
                    if (seen.Contains(edge)) continue;
                    queue.Enqueue(edge);
                }

                seen.Add(bag);
            }

            return false;
        }

        private class Bag
        {
            public string ColorCode { get; set; }
            public int Requirement { get; set; }
        }
    }
}
