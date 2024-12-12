using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day10 : ISolution
{
    private static int FindTrailheadRatings(Dictionary<Location, int> grid, Location startingPoint, bool partTwo = false)
    {
        var locations = new Stack<Location>([startingPoint]) ;
        var peaks = new List<Location>();
        while (locations.Count > 0)
        {
            var location = locations.Pop();

            if (grid[location] == 9)
            {
                peaks.Add(location);
            }
            else
            {
                var nextSpots = grid.DirectNeighbours(location).ToList();
                foreach (var nextSpot in nextSpots.Where(nextSpot => grid[nextSpot] - grid[location] == 1))
                {
                    locations.Push(nextSpot);
                }
            }
        }

        return partTwo ? peaks.Count : peaks.Distinct().Count();
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        var (grid, startingPoints) = ParseGrid(input.ToList());

        return startingPoints.Sum(startingPoint => FindTrailheadRatings(grid, startingPoint)).ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var (grid, startingPoints) = ParseGrid(input.ToList());

        return startingPoints.Sum(startingPoint => FindTrailheadRatings(grid, startingPoint,true)).ToString();

    }
    
    private static (Dictionary<Location, int> grid, List<Location> startingPoints) ParseGrid(List<string> input)
    {
        
        //Parse into grid
        Dictionary<Location, int> grid = [];
        List<Location> startingLocations = new();
        foreach (var (row, y) in input.WithIndex()) 
        {
            foreach (var (col, x) in row.WithIndex())
            {
                grid.Add((x, y), int.Parse(col.ToString()));
                if (col == '0')
                {
                    startingLocations.Add(new(x, y));
                }
            }
        }

        return (grid, startingLocations);
    }

    public int Day => 10;
}
