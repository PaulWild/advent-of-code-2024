using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day21 : ISolution
{
    
    Dictionary<Location, char> numPad = new()
    {
        { (0, 0), '7' },
        { (1, 0), '8' },
        { (2, 0), '9' },
        { (0, 1), '4' },
        { (1, 1), '5' },
        { (2, 1), '6' },
        { (0, 2), '1' },
        { (1, 2), '2' },
        { (2, 2), '3' },
        { (1, 3), '0' },
        { (2, 3), 'A' },
    };
        
    Dictionary<Location, char> arrowPad = new()
    {

        { (1, 0), '^' },
        { (2, 0), 'A' },
        { (0, 1), '<' },
        { (1, 1), 'v' },
        { (2, 1), '>' },
    };
    public record Node(Location Location, Grid.Direction direction) : Node<Location>(Location);

    private class AStar(Dictionary<Location, char> grid) : AStarSearch<Node, Location>
    {
        protected override int H(Node currentNode, Location end)
        {
            return 0 ;
        }

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

        
        protected override int G(int currentCost, Node node, Node oldNode)
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
                var newDirection = NewDirection(node.Location, newLoc);
                if (grid.ContainsKey(newLoc))
                {
                    yield return new Node(newLoc, newDirection);
                }
            }
        }
    }

    private  Dictionary<Location, char> instructionMap = new()
    {
        { (1, 0), '>' },
        { (-1, 0), '<' },
        { (0, 1), 'v' },
        { (0, -1), '^' },
    };
    
    List<char> ToInstructionSet(List<Location> locations)
    {
        var instructions = new List<char>();
        for (var i = 0; i < locations.Count-1; i++)
        {
           var diff = (locations[i+1].x - locations[i].x, locations[i+1].y - locations[i].y); 
           var instruction = instructionMap[diff];
           instructions.Add(instruction);
           
        }
        instructions.Add('A');
        return instructions;
    }
    public string PartOne(IEnumerable<string> input)
    {
        var toReturn = 0;
        foreach (var row in input)
        {
            var arr =  row.ToCharArray();
            var ins= arr.Prepend('A').ToList();
            var list = new List<List<char>>();
            list.Add(ins);
            
            foreach (var instructionSet in list)
            {
                var min = 0;
                var lists = new List<List<char>>();
                lists.Add(instructionSet);
                for (int i = 0; i < 3; i++)
                {
                    var newLists = lists.SelectMany(GenerateInstructions).ToList();
                    min = newLists.Select(l => l.Count()).Min();
                    
                    lists = newLists.Select(x =>  x.Prepend('A').ToList()).ToList();
                }
                
                var num = Convert.ToInt32(Regex.Match(row, @"\d+").Value);
                toReturn += num * min;
                
            }

        }

        return toReturn.ToString();
    }

    private List<List<char>> GenerateInstructions(List<char> list)
    {
        var gridToUse = list.Any(char.IsDigit) ? numPad : arrowPad;
        var instructionSets = new List<List<char>>();
        
        for (var ins = 0; ins < list.Count - 1; ins++)
        {
            var start = list[ins];
            var end = list[ins + 1];

            List<List<Location>> paths = [];
            if (start != end)
            {
                // TODO This is not working as expected. we are not getting all the paths
                // Not sure how day 16 ever workeds
                var (p, cost) = new AStar(gridToUse).Search(
                    new Node(gridToUse.Single(kvp => kvp.Value == start).Key, Grid.Direction.North),
                    gridToUse.Single(kvp => kvp.Value == end).Key);
                
                paths = p.ToList();
                
                var newInstructionSets = new List<List<char>>();
                foreach (var path in paths)
                {
                    var newInstrucation = ToInstructionSet(path);
                    if (instructionSets.Count == 0)
                    {
                        newInstructionSets.Add(newInstrucation);
                    }
                    else
                    {
                        foreach (var instructions in instructionSets)
                        {
                            var n = instructions.Select(x => x).ToList();
                            n.AddRange(newInstrucation);
                            newInstructionSets.Add(n);
                        }
                    }
                }
                instructionSets = newInstructionSets;
            }
            else
            {
                var newInstructionSets = new List<List<char>>();
                var newInstrucation = new List<char>() {'A'};
                if (instructionSets.Count == 0)
                {
                    newInstructionSets.Add(newInstrucation);
                }
                else
                {
                    foreach (var instructions in instructionSets)
                    {
                        var n = instructions.Select(x => x).ToList();
                        n.AddRange(newInstrucation);
                        newInstructionSets.Add(n);
                    }
                }
                instructionSets = newInstructionSets;
            }
            
        }
        
        return instructionSets.ToList();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        throw new NotImplementedException();
    }

    public int Day => 21;
}
