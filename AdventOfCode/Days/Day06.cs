using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day06
    {
        public static void Solve()
        {
            var groupData = FileReader.ParseDataFromFile<string>("Inputs/Day06.txt", 
                Environment.NewLine + Environment.NewLine);

            var totalAnswerCount = GetTotalYesAnswerCount(groupData);
            Console.WriteLine(totalAnswerCount);

            var totalAllYesAnswerCount = GetTotalYesAnswerCountForAll(groupData);
            Console.WriteLine(totalAllYesAnswerCount);
        }

        private static int GetTotalYesAnswerCount(List<string> groupData)
        {
            var totalAnswerCount = groupData.Sum(group =>
            {
                var groupAnswerCount = group.Split('\n')
                    .SelectMany(answer => answer.Trim().ToList())
                    .Distinct().Count();

                return groupAnswerCount;
            });
          
            return totalAnswerCount;
        }

        private static int GetTotalYesAnswerCountForAll(List<string> groupData)
        {
            var totalAnswerCount = groupData.Sum(group =>
            {
                var answers = group.Split('\n');
                var count = answers[0].Count(c => answers.All(answer => answer.Contains(c)));

                return count;
            });
            return totalAnswerCount;
        }
    }
}
