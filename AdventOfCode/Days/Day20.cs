using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day20 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var (grid, start, end) = ParseGrid(input.ToList());

        Dictionary<Location, int> visited = new();
        var current = start;
        for (int i = 0;; i++)
        {
            visited.Add(current, i);
            if (current == end)
            {
                break;
            }

            var next = grid.DirectNeighbours(current).Single(loc => !visited.ContainsKey(loc));
            current = next;
        }
        
        return Solve(start, end, grid, 2);
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var (grid, start, end) = ParseGrid(input.ToList());

        return Solve(start, end, grid, 20);
    }

    private static string Solve(Location start, Location end, Dictionary<Location, char> grid, int cheatDistance)
    {
        Dictionary<Location, int> visited = new();
        var current = start;
        for (int i = 0;; i++)
        {
            visited.Add(current, i);
            if (current == end)
            {
                break;
            }

            var next = grid.DirectNeighbours(current).Single(loc => !visited.ContainsKey(loc));
            current = next;
        }
        
        Dictionary<int, int> CheatTimes = new();

        foreach (var kvp in visited)
        {
            var future = visited.Where(vis => vis.Value > kvp.Value).ToList();

            foreach (var futureStep in future)
            {
                var manhatten = Math.Abs(futureStep.Key.x - kvp.Key.x) + Math.Abs(futureStep.Key.y - kvp.Key.y);

                if (manhatten <= cheatDistance)
                {
                    var visitedTime = futureStep.Value;
                    var cheatTime = kvp.Value + manhatten;
                    
                    if (visitedTime > cheatTime)
                    {
                        var diff = visitedTime - cheatTime;
                        if (CheatTimes.ContainsKey(diff))
                        {
                            CheatTimes[diff]++;
                        }
                        else
                        {
                            CheatTimes.Add(diff, 1);
                        }
                        
                    }
                }
            }
        }
        
        return CheatTimes.Where(kvp => kvp.Key >= 100).Sum(kvp => kvp.Value).ToString();
    }

    private static (Dictionary<Location, char> grid, Location start, Location end) ParseGrid(List<string> input)
    {

        //Parse into grid
        Dictionary<Location, char> grid = [];
        Location startPosition = new Location();
        Location endPosition = new Location();
        foreach (var (row, y) in input.WithIndex())
        {
            foreach (var (col, x) in row.WithIndex())
            {
                switch (col)
                {
                    case '.':
                        grid.Add((x, y), '.');
                        break;
                    case 'S':
                        startPosition = new Location(x, y);
                        grid.Add((x, y), '.');
                        break;
                    case 'E':
                        endPosition = new Location(x, y);
                        grid.Add((x, y), '.');
                        break;
                }


            }
        }

        return (grid, startPosition, endPosition);
    }

    public int Day => 20;
}
