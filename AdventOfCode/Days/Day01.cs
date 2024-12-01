namespace AdventOfCode.Days;

public class Day01 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var (left, right) = ParseInput(input);

        left.Sort();
        right.Sort();
        
        return left.Zip(right, (l, r) => Math.Abs(l - r)).Sum().ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var (left, right) = ParseInput(input);
        var freq = right.AggregateBy(r => r, 0, (group, _) => group+1).ToDictionary();

        return left.Select(x =>
        {
            freq.TryGetValue(x, out var value);
            return x * value;
        }).Sum().ToString();
    }
    
    private static (List<int>left , List<int>right ) ParseInput(IEnumerable<string> input)
    {
        List<int> left = [];
        List<int> right = [];
        foreach (var row in input)
        {
            var foo= row.Split("   ");
            left.Add(int.Parse(foo[0]));
            right.Add(int.Parse(foo[1]));
        }

        return (left, right);
    }

    public int Day => 01;
}
