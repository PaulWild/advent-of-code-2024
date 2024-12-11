using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day11 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var pebbleDictionary = new  Dictionary<(long,int), long>();
        return input.First().Split(' ').Select(long.Parse).ToList().Sum(x => ExpandPebble(x, 25, pebbleDictionary)).ToString();
    }

    private static long ExpandPebble(long pebble, int expansions, Dictionary<(long,int), long> pebbleDictionary)
    {
        if (pebbleDictionary.ContainsKey((pebble, expansions)))
        {
            return pebbleDictionary[(pebble, expansions)];
        }
        if (expansions ==0)
        {
            return 1;
        }
        if (pebble == 0)
        {
            return ExpandPebble(1, expansions - 1, pebbleDictionary);
        }
        if (pebble.ToString().Length % 2 == 0)
        {
            var str = pebble.ToString();
            var result = ExpandPebble(long.Parse(str[..(str.Length / 2)]),expansions - 1, pebbleDictionary) + ExpandPebble(long.Parse(str[(str.Length / 2)..]), expansions -1, pebbleDictionary);
            pebbleDictionary[(pebble, expansions)] = result;
            return result;
        }
        else
        {
            var result = ExpandPebble(pebble * 2024, expansions - 1, pebbleDictionary);
            pebbleDictionary[(pebble, expansions)] = result;
            return result;
        }

    }
    

    public string PartTwo(IEnumerable<string> input)
    {
        var pebbleDictionary = new  Dictionary<(long,int), long>();
        return input.First().Split(' ').Select(long.Parse).ToList().Sum(x => ExpandPebble(x, 75, pebbleDictionary)).ToString();
    }

    public int Day => 11;
}
