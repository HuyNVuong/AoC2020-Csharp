using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day19
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string, string>(
                "Day19.txt",
                Environment.NewLine,
                Environment.NewLine);

            var rules = lines[0].Split('\n');
            var messages = lines[1].Split('\n').ToList();
        }

        private static void CountValidMessage(List<string> rules, List<string> messages)
        {
            var trie = new Trie();
            foreach (var message in messages)
            {
                trie.Add(message);
            }

            var rulesMap = new Dictionary<int, List<List<int>>>();
            foreach (var rule in rules)
            {
                var split = rule.Split(':');
                var root = int.Parse(split[0].Trim());
                if (!Regex.IsMatch(split[1], "[0-9]*")) continue;
                if (split[1].Contains("|"))
                {
                    
                }
                else
                {
                    rulesMap.PutIfAbsent(root, new List<List<int>>());
                    rulesMap[root].Add(split[1].Trim().Split().Select(int.Parse).ToList());
                }
            }
        }

        private class Trie
        {
            public TrieNode Root { get; set; } = new TrieNode();

            public void Add(string str)
            {
                var currentNode = Root;
                foreach (var c in str.ToCharArray())
                {
                    if (!currentNode.Leaves.ContainsKey(c))
                        currentNode.Leaves.Add(c, new TrieNode());

                    currentNode = currentNode.Leaves[c];
                }

                currentNode.IsEnd = true;
            }
        }

        private class TrieNode
        {
            public Dictionary<char, TrieNode> Leaves { get; set; } = new Dictionary<char, TrieNode>();
            public bool IsEnd { get; set; } = false;
        }
    }
}