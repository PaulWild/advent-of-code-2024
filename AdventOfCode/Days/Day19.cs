namespace AdventOfCode.Days;

public class Day19 : ISolution
{
    bool IsPotential(string towel, string design)
    {
        if (design.EndsWith(towel)) return true;

        return false;
    }
    
    long Solve(Stack<string> usedTowels, List<string> towels, string design)
    {
        var dp = new Dictionary<string, long>(); 
        
        long SolveInt(Stack<string> usedTowels, List<string> towels, string design)
        {
            if (dp.TryGetValue(design, out var i))
            {
                return i;
            }

            if (design.Length == 0)
            {
                return 1;
            }

            long toReturn = 0;
            foreach (var towel in towels)
            {
                if (!IsPotential(towel, design)) continue;

                usedTowels.Push(towel);
                var newDesign = design.Substring(0, design.Length - towel.Length);


                var cnt = SolveInt(usedTowels, towels, newDesign);
                dp[newDesign] = cnt;
                toReturn += cnt;
                usedTowels.Pop();
            }
                
            return toReturn;
        }
        
        return SolveInt(usedTowels, towels, design);
    }

    public string PartOne(IEnumerable<string> input)
    {
        var inputLines = input.ToList();
        var patterns = inputLines[0].Split(",").Select(x => x.Trim()).ToList();

        var designs = inputLines.Skip(2).ToList();

        var possible = 0;
        foreach (var design in designs)
        {
            if (Solve(new Stack<string>(), patterns, design) > 0)
            {
                possible++;
            }
        }

        return possible.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var inputLines = input.ToList();
        var patterns = inputLines[0].Split(",").Select(x => x.Trim()).ToList();

        var designs = inputLines.Skip(2).ToList();

        long possible = 0;
        foreach (var design in designs)
        {

            possible += Solve(new Stack<string>(), patterns, design);
        }

        return possible.ToString();
    }

    public int Day => 19;
}
