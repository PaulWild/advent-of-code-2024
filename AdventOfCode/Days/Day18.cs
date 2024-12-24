using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day18 : ISolution
{
    public record Node(Location Location) : Node<Location>(Location);

    private class AStar(HashSet<Location> grid, int maxSize) : AStarSearchOrigBase<Node, Location>
    {
        protected override int H(Node currentNode, Location end)
        {
            return Math.Abs(end.x - currentNode.Location.x) + Math.Abs(end.y - currentNode.Location.y) ;
        }

        protected override int G(int currentCost, Node node)
        {
            return currentCost + 1;
        }

        private readonly List<Location> _deltas =
        [
            (0, 1),
            (1, 0),
            (-1, 0),
            (0, -1)
        ];
        
        protected override IEnumerable<Node> NextNodes(Node node)
        {
            foreach (var delta in _deltas)
            {
                Location newLoc = (node.Location.x + delta.x, node.Location.y + delta.y);
                if (newLoc.x >= 0 && newLoc.y >= 0 && newLoc.x <= maxSize && newLoc.y <= maxSize &&
                    !grid.Contains(newLoc))
                {
                    yield return new Node(newLoc);
                }
            }
        }
    }
    public string PartOne(IEnumerable<string> input)
    {
        var grid = CorruptedSpots(input.ToList());
        var maxSize = 70;
        var search = new AStar(grid.Take(1024).ToHashSet(), maxSize).Search(new Node((0,0)), (maxSize, maxSize));

        return (search.Count - 1).ToString();

    }

    public string PartTwo(IEnumerable<string> input)
    {
        var grid = CorruptedSpots(input.ToList());
        var maxSize = 70;
        for (var i = 1025; i < grid.Count; i++)
        {
            var search = new AStar(grid.Take(i).ToHashSet(), maxSize).Search(new Node((0, 0)), (maxSize, maxSize));
            if (search.Count == 0)
            {
                return grid[i - 1].ToString();
            }
        }
        
        throw new Exception("Nope");

    }

    private List<Location>  CorruptedSpots(List<string> input)
    {
        
        List<Location> grid = [];
        foreach (var row in input) 
        {
            var nums = Regex.Matches(row, @"\d+").Select(m => int.Parse(m.Groups[0].Value)).ToArray();
            grid.Add((nums[0], nums[1]));
        }

        return grid;
    }
    
    public int Day => 18;
}
