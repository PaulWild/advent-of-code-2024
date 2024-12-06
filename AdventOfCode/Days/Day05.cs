namespace AdventOfCode.Days;

public class Day05 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var (printOuts, instructions) = ParseInput(input.ToList());

        return printOuts.Sum(printOut => IsValidPrintout(printOut, instructions)).ToString();
        
    }

    private (List<List<int>> printOuts, Dictionary<int, HashSet<int>> instructions) ParseInput(List<string> input)
    {
        var parsingInstruction = true;
        var instructions = new Dictionary<int, HashSet<int>> ();
        var printOuts = new List<List<int>>();
        
        foreach (var row in input)
        {
            if (row.Trim() == string.Empty)
            {
                parsingInstruction = false;
            } else if (parsingInstruction)
            {
                var split = row.Split("|");
                var page = int.Parse(split[1]);
                var before = int.Parse(split[0]);

                if (instructions.TryGetValue(page, out var value))
                {
                    value.Add(before);
                }
                else
                {
                    instructions.Add(page, [before]);
                }
            }
            else
            {
                printOuts.Add(row.Split(",").Select(int.Parse).ToList());
            }
        }
        
        return (printOuts, instructions);
    }

    private int IsValidPrintout(List<int> printOut, Dictionary<int, HashSet<int>> instructions)
    {
        for (var i =0; i< printOut.Count; i++)
        {
            var test = printOut[i];
            var rest = printOut.Skip(i+1).ToHashSet();
            
            instructions.TryGetValue(test, out var conditions);

            if (conditions == null)
            {
                continue;
            }
            
            rest.IntersectWith(conditions);

            if (rest.Count > 0)
            {
                return 0;
            }
            
        }

        return printOut[(printOut.Count-1)/2];
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var (printOuts, instructions) = ParseInput(input.ToList());

        var invalidLists = printOuts.Where(printOut => IsValidPrintout(printOut, instructions) == 0).ToList();

        foreach (var invalidList in invalidLists)
        {
            for (var i = 0; i < invalidList.Count; i++)
            {
                var test = invalidList[i];
                var rest = invalidList.Skip(i + 1).ToHashSet();

                instructions.TryGetValue(test, out var conditions);

                if (conditions == null)
                {
                    continue;
                }

                rest.IntersectWith(conditions);

                if (rest.Count > 0)
                {
                    invalidList[invalidList.IndexOf(rest.Last())] = test;
                    invalidList[i] = rest.Last();
                   
                    //ewww to reset the list you have to reset it to -1 not 0
                    i = -1;
                }
            }
        }

        return invalidLists.Sum(printOut => printOut[(printOut.Count - 1) / 2]).ToString();
    }

    public int Day => 05;
}
