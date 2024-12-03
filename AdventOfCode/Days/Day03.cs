using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public partial class Day03 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var matches= MultipliersRegex().Matches(string.Join("",input));
        return matches
            .Select(match => long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[2].Value))
            .Sum()
            .ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var bigInput = string.Join("", input);
        var donts = bigInput.Split("don't()");

        var split = donts.Select(dont => dont.Split("do()")).ToList();
        List<string> newInput = [];
        newInput.Add(split.First()[0]);
        newInput.AddRange(from s in split where s.Length >1  select string.Join("", s.Skip(1)));

        var matches= MultipliersRegex().Matches(string.Join("",newInput));
        return matches
            .Select(match => long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[2].Value))
            .Sum()
            .ToString();
    }

    public int Day => 03;

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MultipliersRegex();
}
