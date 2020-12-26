using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day18
    {
        public static void Solve()
        {
            var questions = FileReader.ParseDataFromFile<string>("Inputs/Day18.txt");
            
            var results = SolveQuestions(questions, BasicEvaluate);
            Console.WriteLine(results);

            var advancedResults = SolveQuestions(questions, AdvanceEvaluate);
            Console.WriteLine(advancedResults);
        }

        private static long SolveQuestions(IEnumerable<string> questions, Func<string, long> evaluate)
        {
            const string rgx = @"\([^\(]+?\)";
            var result = questions.Sum(question =>
            {
                while (Regex.IsMatch(question, rgx))
                {
                    var match = Regex.Match(question, rgx);
                    question = question.Substring(0, match.Index)
                               + $"{evaluate(match.Value.Substring(1, match.Length - 2))}"
                               + question.Substring(match.Index + match.Length);
                }

                return evaluate(question);
            });

            return result;
        }

        private static long BasicEvaluate(string expression)
        {
            var tokens = expression.Split();
            var result = long.Parse(tokens[0]);
            for (var i = 1; i < tokens.Length; i += 2)
            {
                var operation = tokens[i];
                result = operation switch
                {
                    "+" => result + long.Parse(tokens[i + 1]),
                    "*" => result * long.Parse(tokens[i + 1]),
                    _   => throw new ArgumentOutOfRangeException($"Invalid op {operation}")
                };
            }

            return result;
        }
        
        private static long AdvanceEvaluate(string expression)
        {
            while (Regex.IsMatch(expression, @".{2}(\+).{2}"))
            {
                var match = Regex.Match(expression, @"[0-9]*( \+ )[0-9]*");
                expression = expression.Substring(0, match.Index)
                             + $"{BasicEvaluate(match.Value)}"
                             + expression.Substring(match.Index + match.Length);
            }
            
        
            return BasicEvaluate(expression);
        }
    }
}
