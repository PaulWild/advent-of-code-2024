namespace AdventOfCode.Days;

public class Day07 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
       return ParseInput(input)
           .Where(row => CanCombine(row.Item1, row.Item2.Skip(1).ToList(), row.Item2[0], false))
           .Sum(row => row.Item1)
           .ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        return ParseInput(input)
            .Where(row => CanCombine(row.Item1, row.Item2.Skip(1).ToList(), row.Item2[0], true))
            .Sum(row => row.Item1)
            .ToString();
    }
    
    private static IEnumerable<(long, List<long>)> ParseInput(IEnumerable<string> input)
    {
        return from row in input select row.Split(": ") 
            into split let total = long.Parse(split[0]) 
            let numbers = split[1].Split(" ").Select(long.Parse).ToList() 
            select (total, numbers);
    }
    
    private static bool CanCombine(long total, List<long> numbers, long currentTotal, bool part2)
    {
        if (currentTotal > total)
            return false;
        if (total == currentTotal && numbers.Count == 0)
            return true;
        if (numbers.Count == 0)
            return false;
        
        var newList = numbers.Skip(1).ToList();
        
        return CanCombine(total, newList, currentTotal * numbers[0], part2) 
               || CanCombine(total, newList, currentTotal + numbers[0], part2)
               || (part2 && CanCombine(total, newList, long.Parse(string.Join("", [currentTotal, numbers[0]])), part2));
    }

    public int Day => 07;
}
