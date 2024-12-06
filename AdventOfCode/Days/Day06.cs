using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day06 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var (blocks, guardLocation)= ParseGrid(inputList.ToList());
        var maxX = inputList.First().Length;
        var maxY = inputList.Count;

        var visited = GetVistedLocations(guardLocation, maxX, maxY, blocks);

        return (visited.Count).ToString();
    }

    private static HashSet<Location> GetVistedLocations(Location guardLocation, int maxX, int maxY, HashSet<Location> blocks)
    {
        var directions = new LinkedList<Location>([(0, -1), (1, 0), (0, 1), (-1, 0)]);

        var currentDirection = directions.First;
        var visited = new HashSet<Location>();

        while (guardLocation.x >= 0 && guardLocation.x < maxX && guardLocation.y >= 0 && guardLocation.y < maxY)
        {
            visited.Add(guardLocation);

            var nextLocation = (guardLocation.x + currentDirection!.Value.x, guardLocation.y + currentDirection.Value.y);

            if (blocks.Contains(nextLocation))
            {
                currentDirection = currentDirection.NextOrFirst();
            }
            else
            {
                guardLocation = nextLocation;
            }
        }

        return visited;
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var (blocks, originalGuardLocation)= ParseGrid(inputList.ToList());
        var maxX = inputList.First().Length;
        var maxY = inputList.Count;

        var directions = new LinkedList<Location>([(0, -1), (1, 0), (0, 1), (-1, 0)]);
        var potentialBlocks = GetVistedLocations(originalGuardLocation, maxX, maxY, blocks);


        var loops = 0;

        foreach (var potentialBlock in potentialBlocks)
        {

            var newBlocks = blocks.Select(x => x).ToHashSet();
            newBlocks.Add(potentialBlock);
            var guardLocation = originalGuardLocation;

            var currentDirection = directions.First;
            var visited = new HashSet<(Location, Location)>();

            while (guardLocation.x >= 0 && guardLocation.x < maxX && guardLocation.y >= 0 && guardLocation.y < maxY)
            {
                if (!visited.Add((guardLocation, currentDirection!.Value)))
                {
                    loops++;
                    break;
                }

                var nextLocation = (guardLocation.x + currentDirection.Value.x,
                    guardLocation.y + currentDirection.Value.y);

                if (newBlocks.Contains(nextLocation))
                {
                    currentDirection = currentDirection.NextOrFirst();
                }
                else
                {
                    guardLocation = nextLocation;
                }
            }
        }


        return (loops).ToString();
    }
    
    private static (HashSet<Location>, Location) ParseGrid(List<string> input)
    {
        
        //Parse into grid
        HashSet<Location> grid = [];
        Location currentLocation = new();
        foreach (var (row, y) in input.WithIndex()) 
        {
            foreach (var (col, x) in row.WithIndex())
            {
                switch (col)
                {
                    case '#':
                        grid.Add((x, y));
                        break;
                    case '^':
                        currentLocation = (x, y);
                        break;
                }
            }
        }

        return (grid, currentLocation);
    }


    public int Day => 06;
}
