using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Days;



public class Day21 : ISolution
{
    public class Expansion
    {
        private readonly Dictionary<Location, char> _grid;

        public Expansion(Dictionary<Location, char> grid)
        {
            _grid = grid;
        }
        public char From { get; set; }
    
        public char To { get; set; }

        private List<List<Expansion>>? _expansions = null;

        public List<List<Expansion>> Exanpsions
        {
            get
            {
                if (_expansions == null)
                {
                    _expansions = ExpandExpansion(From, To, _grid);
                }
                return _expansions;
            }
        }
        
        public long ShortestInstruction(long depth)
        {
            if (Dp.ContainsKey((From, To, depth)))
            {
                return Dp[(From, To, depth)];
            }
            if (depth == 1)
            {
                if (Exanpsions != null)
                {
                    var foo = ((Exanpsions.MinBy(exp => exp.Count) ?? throw new InvalidOperationException()).Count() ) ;
                    return foo;
                }
            }
      
            var sums = new List<long>();
            if (Exanpsions != null)
                foreach (var expansions in Exanpsions)
                {
                    sums.Add(expansions.Sum(x => x.ShortestInstruction(depth - 1)));
                }

            var res = sums.Min();
            Dp[(From, To, depth)] = res;
            return res;

        }
    }
    
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

    private static readonly Dictionary<Location, char> ArrowPad = new()
    {

        { (1, 0), '^' },
        { (2, 0), 'A' },
        { (0, 1), '<' },
        { (1, 1), 'v' },
        { (2, 1), '>' },
    };

    private static readonly Dictionary<(char,char, long), long> Dp = new ();

        
    private static readonly Dictionary<Location, char> InstructionMap = new()
    {
        { (1, 0), '>' },
        { (-1, 0), '<' },
        { (0, 1), 'v' },
        { (0, -1), '^' },
    };
    
    public static List<char> ToInstructionSet(List<Location> locations)
    {
        var instructions = new List<char>();
        for (var i = 0; i < locations.Count-1; i++)
        {
           var diff = (locations[i+1].x - locations[i].x, locations[i+1].y - locations[i].y); 
           var instruction = InstructionMap[diff];
           instructions.Add(instruction);
           
        }
        instructions.Add('A');
        return instructions;
    }

    public static List<List<Expansion>> ExpandExpansion(char start, char end, Dictionary<Location, char> gridToUse)
    {
        
        var foo = Bfs(gridToUse.Single(kvp => kvp.Value == start).Key,
            gridToUse.Single(kvp => kvp.Value == end).Key, gridToUse);
        var instructions=  ShortestPath(gridToUse.Single(kvp => kvp.Value == start).Key,
            gridToUse.Single(kvp => kvp.Value == end).Key, foo).ToList().Select(ToInstructionSet).ToList();


        List<List<Expansion>> toReturn = new List<List<Expansion>>();
        foreach (var instructionSet in instructions)
        {
            var newInstrctionSet = instructionSet.Prepend('A').ToList();
            List<Expansion> expansions = new List<Expansion>();
            for (var i = 0; i < newInstrctionSet.Count-1; i++)
            {
                expansions.Add(new Expansion(ArrowPad) { From = newInstrctionSet[i], To = newInstrctionSet[i + 1] });
            }
            toReturn.Add(expansions);
        }

        return toReturn;
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        long toReturn = 0;
        foreach (var row in input)
        {
            var instructions= row.ToCharArray().Prepend('A').ToList();

            List<Expansion> expansions = new List<Expansion>();
            for (int i = 0; i < instructions.Count-1; i++)
            {
                expansions.Add(new Expansion(numPad) { From = instructions[i], To = instructions[i + 1] });
            }

            var min = expansions.Sum(x => x.ShortestInstruction(3));
            var num = Convert.ToInt64(Regex.Match(row, @"\d+").Value);
            toReturn += num * min;

        }

        return toReturn.ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        long toReturn = 0;
        foreach (var row in input)
        {
            var instructions= row.ToCharArray().Prepend('A').ToList();

            List<Expansion> expansions = new List<Expansion>();
            for (int i = 0; i < instructions.Count-1; i++)
            {
                expansions.Add(new Expansion(numPad) { From = instructions[i], To = instructions[i + 1] });
            }

            var min = expansions.Sum(x => x.ShortestInstruction(26));
            var num = Convert.ToInt64(Regex.Match(row, @"\d+").Value);
            toReturn += num * min;

        }

        return toReturn.ToString();
    }

    public static Dictionary<Location, long> Bfs(Location start, Location end, Dictionary<Location, char> grid)
    {
        var depth = new Dictionary<Location, long> { { start, 0 } };
        var nodes = new Queue<Location>();
        nodes.Enqueue(start);

        while (depth.Count > 0)
        {
            var node = nodes.Dequeue();
            if (node == end)
            {
                return depth;
            }

            foreach (var neighbour in grid.DirectNeighbours(node))
            {
                if (!depth.ContainsKey(neighbour))
                {
                    depth[neighbour] = depth[node] + 1;
                    nodes.Enqueue(neighbour);
                }
            }
        }
        throw new Exception("Nope");
    }
    
    public static IEnumerable<List<Location>> ShortestPath(Location node, Location end, Dictionary<Location, long> depth)
    {
        if (node == end)
        {
            yield return [node];
        }
        else
        {
            foreach (var neighbour in depth.DirectNeighbours(node))
            {
                if (depth[neighbour] == depth[node] + 1)
                {
                    var rest = ShortestPath(neighbour, end, depth);
                    foreach (var path in rest)
                    {
                        yield return path.Prepend(node).ToList();
                    }
                } 
            }
        }
    }


    public int Day => 21;
}
