using System.Drawing;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day12 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var grid = ParseGrid(input.ToList());
        var total = 0;
        
        var visited = new HashSet<Location>();

        foreach (var kvp in grid)
        {
            var perimiter = 0;
            var area = 0;
            var patch = new Stack<Location>([kvp.Key]);

            while (patch.Count != 0)
            {
                var location = patch.Pop();
                if (!visited.Add(location))
                {
                    continue;
                }
 
                var neighbours = grid.DirectNeighbours(location)
                    .Where(neighbour =>  grid[neighbour] == kvp.Value)
                    .ToList();

                foreach (var neighbour in neighbours.Where(neighbour => !visited.Contains(neighbour)))
                {
                    patch.Push(neighbour);
                }
                
                perimiter += 4 - neighbours.Count;
                area ++;
            }
            
            total += perimiter * area;
        }

        return total.ToString();
      
    }

    [Flags]
    enum Corners
    {
        TL = 1,
        TR = 2,
        BL = 4,
        BR = 8 
    };
    
    public string PartTwo(IEnumerable<string> input)
    {
        var grid = ParseGrid(input.ToList());
        var total = 0;
        
        var visited = new HashSet<Location>();

        foreach (var kvp in grid)
        {
            var area = 0;
            var patch = new Stack<Location>([kvp.Key]);
            var patchSet = new HashSet<Location>();

            while (patch.Count != 0)
            {
                var location = patch.Pop();
                if (!visited.Add(location))
                {
                    continue;
                }
 
                patchSet.Add(location);
                var neighbours = grid.DirectNeighbours(location)
                    .Where(neighbour =>  grid[neighbour] == kvp.Value)
                    .ToList();

                foreach (var neighbour in neighbours.Where(neighbour => !visited.Contains(neighbour)))
                {
                    patch.Push(neighbour);
                }
                
                area ++;
            }

            var corners = CalculateCorners(patchSet);
            total += corners * area;
        }

        return total.ToString();
    }

    private int CalculateCorners(HashSet<Location> patchSet)
    {
        var corners = new Dictionary<Location, Corners>();
        foreach (var location in patchSet)
        {
            //tl 
            if (!corners.TryAdd(location, Corners.TL))
            {
                corners[location] |= Corners.TL;
            }

            var tr = (location.x + 1, location.y);
            if (!corners.TryAdd(tr, Corners.TR))
            {
                corners[tr] |= Corners.TR;
            }

            var br = (location.x + 1, location.y + 1);
            if (!corners.TryAdd(br, Corners.BR))
            {
                corners[br] |= Corners.BR;
            }

            var bl = (location.x, location.y + 1);
            if (!corners.TryAdd(bl, Corners.BL))
            {
                corners[bl] |= Corners.BL;
            }
        }

        return corners.Sum(corner => corner.Value switch
        {
            (Corners.TR | Corners.BR) or (Corners.TL | Corners.BL) => 0,
            (Corners.TL | Corners.BR) or (Corners.TR | Corners.BL) => 2,
            (Corners.TR | Corners.TL) or (Corners.BR | Corners.BL) or (Corners.BR | Corners.BL | Corners.TR | Corners.TL) => 0,
            _ => 1
        });
    }

    private static Dictionary<Location, char> ParseGrid(List<string> input)
    {
        
        //Parse into grid
        Dictionary<Location, char> grid = [];
        foreach (var (row, y) in input.WithIndex()) 
        {
            foreach (var (col, x) in row.WithIndex())
            {
                grid.Add((x, y), col);
            }
        }

        return grid;
    }

    public int Day => 12;
}
