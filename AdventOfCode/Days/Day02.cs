using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day02
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day02.txt");
            var passwords = lines.Select(line =>
            {
                var split = line.Split(':');
                var requirement = split[0].Split()
                    .Select(x => x.Trim()).ToList();
                var pair = requirement[0].Trim().Split('-');

                return new Password
                {
                    Code = split[1],
                    Requirement = new PasswordRequirement
                    {
                        RequiredLetter = requirement[1].First(),
                        Min = int.Parse(pair[0]),
                        Max = int.Parse(pair[1])
                    }
                };
            }).ToList();

            var validPasswordCount = FindNumberOfValidPasswords(passwords);
            Console.WriteLine(validPasswordCount);

            var validPasswordCountWithActualRules = FindNumberOfValidPasswordsWithActualRules(passwords);
            Console.WriteLine(validPasswordCountWithActualRules);
        }

        private static int FindNumberOfValidPasswords(List<Password> passwords)
        {
            var validPassword = passwords.Where(password =>
            {
                var counter = password.Code.GroupBy(c => c)
                    .ToDictionary(group => group.Key, group => group.Count());
                var requiredLetterCount = counter.GetValueOrDefault(password.Requirement.RequiredLetter, -1);

                return requiredLetterCount >= password.Requirement.Min && requiredLetterCount <= password.Requirement.Max;
            }).ToList();

            return validPassword.Count;
        }

        private static int FindNumberOfValidPasswordsWithActualRules(List<Password> passwords)
        {
            var validPassword = passwords
                .Where(password => password.Code[password.Requirement.Min - 1] == password.Requirement.RequiredLetter 
                                 ^ password.Code[password.Requirement.Max - 1] == password.Requirement.RequiredLetter)
                .ToList();

            return validPassword.Count;
        }

        private class Password
        {
            public string Code { get; set; }
            public PasswordRequirement Requirement { get; set; }
        }

        private class PasswordRequirement
        {
            public char RequiredLetter { get; set; }
            public int Min { get; set; }
            public int Max { get; set; }
        }
    }
}
