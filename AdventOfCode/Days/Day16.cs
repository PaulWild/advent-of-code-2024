using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day16 : ISolution
{
    public record Node(Location Location, Grid.Direction Direction) : Node<Location>(Location);

    private class AStar(Dictionary<Location, char> grid) : AStarSearch<Node, Location>
    {
        private Grid.Direction NewDirection(Location old, Location newLocation) 
        {
            return (newLocation.x - old.x,newLocation.y - old.y) switch
            {
                (1,0) => Grid.Direction.East,
                (-1,0) => Grid.Direction.West,
                (0,1) => Grid.Direction.South,
                (0,-1) => Grid.Direction.North,
                _ => throw new Exception("Can't find new direction")
            };
        }
        
        protected override int H(Node currentNode, Location end)
        {
            return Math.Abs(end.x - currentNode.Location.x) + Math.Abs(end.y - currentNode.Location.y) ;
        }

        protected override int G(int currentCost, Node node, Node oldNode)
        {
            return currentCost + (node.Direction == oldNode.Direction ? 1 : 1001);
        }

        protected override IEnumerable<Node> NextNodes(Node node)
        {
            var neighbours = grid.DirectNeighboursNotBackwards(node.Location, node.Direction).ToList();

            foreach (var neighbour in neighbours)
            {
                var newDirection = NewDirection(node.Location, neighbour);
                yield return new Node(neighbour, newDirection);
            }
        }
    }
    public string PartOne(IEnumerable<string> input)
    {
        var (grid, start, end) = ParseGrid(input.ToList());
        
        var search = new AStar(grid);
        var (_, cost) = search.Search(new Node(start, Grid.Direction.East), end);
        
        
        return cost.ToString();
    }

    
    public string PartTwo(IEnumerable<string> input)
    {
        var (grid, start, end) = ParseGrid(input.ToList());
        
        var search = new AStar(grid);
        var (path,_) = search.Search(new Node(start, Grid.Direction.East), end);

        return path.SelectMany(n => n).Distinct().Count().ToString();
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

    public int Day => 16;
}


