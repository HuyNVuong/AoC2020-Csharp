using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day04
    {
        public static void Solve()
        {
            var raw = FileReader.ParseDataFromFile<string>("Inputs/Day04.txt", "").First();
            var passportsData = raw.Split(new[] {Environment.NewLine + Environment.NewLine},
                    StringSplitOptions.RemoveEmptyEntries).ToList();
            var passports = passportsData
                .Select(data => data.Split('\n').Select(splits => splits.Trim().Split())
                    .SelectMany(x => x).ToList())
                .ToList();
            var validPassport = FindValidPassports(passports);
            Console.WriteLine(validPassport.Count);

            var validPassportWithValidatedDataCount = FindValidPassportWithValidatedData(validPassport);
            Console.WriteLine(validPassportWithValidatedDataCount);
        }

        private static List<List<string>> FindValidPassports(List<List<string>> passports)
        {
            var validPassports = passports.Where(infos =>
            {
                var passport = infos.ToList();
                return passport.Count == 8 || passport.Count == 7 && passport.All(info => !info.Contains("cid"));
            }).ToList();

            return validPassports;
        }

        private static int FindValidPassportWithValidatedData(List<List<string>> passports)
        {

            var validPassportsWithValidatedData = passports.Where(passport =>
            {
                var isPassportValid = passport.All(info =>
                {
                    var broken = info.Split(':');
                    var key = broken[0].Trim();
                    var value = broken[1].Trim();
                    var _ = int.TryParse(value, out var numValue);

                    static bool HeightTest(string height)
                    {
                        if (height.Contains("cm"))
                        {
                            return height.Length == 5
                                   && int.Parse(height.Substring(0, 3)) >= 150
                                   && int.Parse(height.Substring(0, 3)) <= 193;
                        }
                        if (height.Contains("in"))
                        {
                            return height.Length == 4
                                   && int.Parse(height.Substring(0, 2)) >= 59
                                   && int.Parse(height.Substring(0, 2)) <= 76;
                        }
                        return false;
                    }

                    static bool RegexTest(string str, string pattern)
                    {
                        var regex = new Regex(pattern);
                        return regex.IsMatch(str);
                    }

                    return key switch
                    {
                        "byr" => numValue >= 1920 && numValue <= 2002,
                        "iyr" => numValue >= 2010 && numValue <= 2020,
                        "eyr" => numValue >= 2020 && numValue <= 2030,
                        "hgt" => HeightTest(value),
                        "hcl" => RegexTest(value, "^#[0-9a-f]{6}"),
                        "ecl" => new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(value),
                        "pid" => RegexTest(value, "^[0-9]{9}$"),
                        "cid" => true,
                        _ => true
                    };
                });

                return isPassportValid;
            });

            return validPassportsWithValidatedData.Count();
        }
    }
}
