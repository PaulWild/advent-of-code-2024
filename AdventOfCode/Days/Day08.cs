using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day08 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var grid = ParseGrid(inputList).ToList();
        var maxX = inputList.First().Length;
        var minY = inputList.Count * -1;

        HashSet<Location> distinctPoint = [];
        for (var i = 0; i < grid.Count-1; i++)
        for (var j = i + 1; j < grid.Count; j++)
        {
            var antenna1 = grid[i];
            var antenna2 = grid[j];

            if (antenna1.Value != antenna2.Value) continue;
            
            Location step = (antenna1.Key.x - antenna2.Key.x,antenna1.Key.y - antenna2.Key.y);
            
            var points = PotentialInterferencePoints(antenna1.Key, step)
                .Intersect(PotentialInterferencePoints(antenna2.Key, step));


            foreach (var point in points)
            {
                if (point.x >= 0 && point.x < maxX && point.y <= 0 && point.y > minY)
                {
                    distinctPoint.Add(point);
                }
            }
        }

        return distinctPoint.Count.ToString();

    }
    
    private static IEnumerable<Location> PotentialInterferencePoints(Location start, Location step) {
        
        yield return (start.x + step.x, start.y + step.y);
        yield return (start.x - step.x, start.y - step.y);
        yield return (start.x - 2*step.x, start.y - 2*step.y);
        yield return (start.x + 2*step.x, start.y + 2*step.y);
    }
    
    private static IEnumerable<Location> PotentialInterferencePointsPart2(Location start, Location step, int maxX, int minY)
    {

        var inc = 1;
        while (true)
        {
            Location antiNode = (start.x + inc*step.x, start.y + inc*step.y);
            if (antiNode.x >= 0 && antiNode.x < maxX && antiNode.y <= 0 && antiNode.y > minY)
            {
                yield return antiNode;
                inc++;
            }
            else
            {
                break;
            }
        }

        inc = 1;
        while (true)
        {
            Location antiNode = (start.x - inc*step.x, start.y - inc*step.y);
            if (antiNode.x >= 0 && antiNode.x < maxX && antiNode.y <= 0 && antiNode.y > minY)
            {
                yield return antiNode;
                inc++;
            }
            else
            {
                break;
            }
        }
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var grid = ParseGrid(inputList).ToList();
        var maxX = inputList.First().Length;
        var minY = inputList.Count * -1;

        HashSet<Location> distinctPoint = [];
        for (var i = 0; i < grid.Count; i++)
        for (var j = i + 1; j < grid.Count; j++)
        {
            var antenna1 = grid[i];
            var antenna2 = grid[j];

            if (antenna1.Value != antenna2.Value) continue;
            
            Location step = (antenna1.Key.x - antenna2.Key.x,antenna1.Key.y - antenna2.Key.y);
            
            var points = PotentialInterferencePointsPart2(antenna1.Key, step, maxX, minY)
                .Intersect(PotentialInterferencePointsPart2(antenna2.Key, step, maxX, minY));

            distinctPoint.Add(antenna1.Key);
            distinctPoint.Add(antenna2.Key);

            foreach (var point in points)
            {
                    distinctPoint.Add(point);
            }
        }

        return distinctPoint.Count.ToString();
    }
    
    private static Dictionary<Location, char> ParseGrid(List<string> input)
    {
        
        //Parse into grid with 0,0 in the first spot. 
        Dictionary<Location, char> grid = new();
        foreach (var (row, y) in input.WithIndex()) 
        foreach (var (col, x) in row.WithIndex())
        {
            if (col != '.')
            {
                grid.Add((x, y*-1), col);
            }
        }
        

        return grid;
    }

    public int Day => 08;
}
