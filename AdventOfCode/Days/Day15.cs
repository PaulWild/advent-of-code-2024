using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day15 : ISolution
{
    Dictionary<char, Location> _map = new()
    {
        { '>', (1,0)},
        { '<', (-1,0)},
        { '^', (0,-1)},
        { 'v', (0,1)},
    };

    public string PartOne(IEnumerable<string> input)
    {
        var (grid, robotPosition) = ParseGrid(input.TakeWhile(x => x.Trim() != "").ToList());
        var instruction = input.SkipWhile(x => x.Trim() != "").Skip(1).SelectMany(x => x).ToList();
        
        var width = grid.Select(x => x.Key.x).Max();
        var height = grid.Select(x => x.Key.x).Max();

        
        foreach (var instructionLine in instruction)
        {
            // Console.ForegroundColor = ConsoleColor.Green;
            // Console.Clear();
            //
            // for (var y = 0; y <= height; y++)
            // {
            //     Console.WriteLine();
            //     for (var x = 0; x <= width; x++)
            //     {
            //
            //         if (robotPosition == (x, y))
            //         {
            //             Console.Write("@");
            //         }
            //         else if (grid.ContainsKey((x, y)))
            //         {
            //             Console.Write(grid[(x, y)]);
            //         }
            //         else {Console.Write(" ");}
            //     }
            // }
            // Thread.Sleep(500);
            

            var delta = _map[instructionLine];
            Location nextPosition = (robotPosition.x + delta.x, robotPosition.y + delta.y);

            if (!grid.TryGetValue(nextPosition, out var value))
            {
                robotPosition = nextPosition;
            }
            else if (value == '#')
            {
                continue;
            }
            else
            {
                var potentialMoves = new Stack<Location>();
                potentialMoves.Push(nextPosition);
                
                //move some boxes about
                var loop = true;
                while (loop) 
                {
                    
                    var last = potentialMoves.Peek();
                    Location nextPotentialMove = (last.x + delta.x, last.y  + delta.y);

                    if (!grid.TryGetValue(nextPotentialMove, out var nextPotentialSquare))
                    {
                        grid.Remove( potentialMoves.Last());
                        robotPosition = potentialMoves.Last();
                        grid.Add(nextPotentialMove, 'O');
                        loop=false;
                    }
                    else if (nextPotentialSquare == 'O')
                    {
                        potentialMoves.Push(nextPotentialMove);
                    }
                    else if (nextPotentialSquare == '#')
                    {
                        loop = false;
                    }
                }
            }
        }

        return grid.Where(x => x.Value == 'O').Select(x => x.Key.x + (x.Key.y * 100)).Sum().ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var (grid, robotPosition) = ParseGridPart2(input.TakeWhile(x => x.Trim() != "").ToList());
        var instruction = input.SkipWhile(x => x.Trim() != "").Skip(1).SelectMany(x => x).ToList();
        
        var width = grid.Select(x => x.Key.x).Max();
        var height = grid.Select(x => x.Key.y).Max();

        
        var iter = 0;
        
        foreach (var instructionLine in instruction)
        {
            iter++;
            
            var delta = _map[instructionLine];
            Location nextPosition = (robotPosition.x + delta.x, robotPosition.y + delta.y);

            if (!grid.TryGetValue(nextPosition, out var value))
            {
                robotPosition = nextPosition;
            }
            else if (value == '#')
            {
                continue;
            }
            else
            {
                if (instructionLine == '>' || instructionLine == '<')
                {
                    var potentialMoves = new Stack<Location>();
                    potentialMoves.Push(nextPosition);

                    //move some boxes about
                    var loop = true;
                    while (loop)
                    {

                        var last = potentialMoves.Peek();
                        Location nextPotentialMove = (last.x + delta.x, last.y + delta.y);

                        if (!grid.TryGetValue(nextPotentialMove, out var nextPotentialSquare))
                        {
                            var newRobotPosition = potentialMoves.Last();
                            var meh = grid[newRobotPosition];
                            grid.Add(nextPotentialMove, meh == ']' ? '[' : ']');
                            grid.Remove(newRobotPosition);
                            robotPosition = newRobotPosition;


                            while (potentialMoves.Count > 1)
                            {
                                var toSwap = potentialMoves.Pop();
                                grid[toSwap] = grid[toSwap] == ']' ? '[' : ']';
                            }

                            loop = false;
                        }
                        else if (nextPotentialSquare == '[' || nextPotentialSquare == ']')
                        {
                            potentialMoves.Push(nextPotentialMove);
                        }
                        else if (nextPotentialSquare == '#')
                        {
                            loop = false;
                        }
                    }
                } 
                else  {          
                    var potentialMoves = new Stack<List<Location>>();

                    var nextPositions = new List<Location>();
                    if (value == ']')
                    {
                        nextPositions.Add((nextPosition.x - 1, nextPosition.y)); 
                        nextPositions.Add(nextPosition);
                    }
                    else
                    {
                        nextPositions.Add(nextPosition);
                        nextPositions.Add((nextPosition.x + 1 , nextPosition.y)); 
                    }
                    
                    potentialMoves.Push(nextPositions);

                    //move some boxes about
                    var loop = true;
                    while (loop)
                    {

                        var lastMoves = potentialMoves.Peek();
                        List<Location> nextPotentialMoves = lastMoves.Select(move => (move.x + delta.x, move.y + delta.y)).ToList();

                        if (nextPotentialMoves.All(x => !grid.ContainsKey(x)))
                        {
                            
                            while (potentialMoves.Count > 0)
                            {
                                var toDelete = potentialMoves.Pop();
                                var toSwap = toDelete.Select(loc => (loc.x + delta.x, loc.y + delta.y));
                                foreach (var move in toDelete)
                                {
                                    grid.Remove(move);
                                }

                                var insert = '[';
                                foreach (var move in toSwap)
                                {
                                    grid[move]= insert;
                                    insert = insert == '[' ? ']' : '[';
                                }
                            }
                            robotPosition = nextPosition;

                            loop = false;
                        }
                        else if (nextPotentialMoves.Any(x => grid.ContainsKey(x) && grid[x] == '#'))
                        {
                            loop = false;
                        }
                        else
                        {
                            nextPotentialMoves = nextPotentialMoves.Where(x => grid.ContainsKey(x)).ToList();
                            if (grid.ContainsKey(nextPotentialMoves.First()) && grid[nextPotentialMoves.First()] == ']')
                            {
                                nextPotentialMoves.Insert(0, (nextPotentialMoves.First().x -1, nextPotentialMoves.First().y));
                            }
                            
                            if( grid.ContainsKey(nextPotentialMoves.Last()) && grid[nextPotentialMoves.Last()] == '[')
                            {
                                nextPotentialMoves.Add( (nextPotentialMoves.Last().x+1, nextPotentialMoves.Last().y));
                            }
                            potentialMoves.Push(nextPotentialMoves);
                            // have to build a list of moved things
                     
                        }

                    }}
            }
            
            // Console.ForegroundColor = ConsoleColor.Green;
            // Console.Clear();
            //
            // for (var y = 0; y <= height; y++)
            // {
            //     Console.WriteLine();
            //     for (var x = 0; x <= width; x++)
            //     {
            //
            //         if (robotPosition == (x, y))
            //         {
            //             Console.Write("@");
            //         }
            //         else if (grid.ContainsKey((x, y)))
            //         {
            //             Console.Write(grid[(x, y)]);
            //         }
            //         else {Console.Write(" ");}
            //     }
            // }
            //
            // Console.WriteLine("");
            // Console.WriteLine($"Move: {instructionLine}, iteration {iter}");
            // Console.ReadLine();
            //

        }

        return grid.Where(x => x.Value == '[' ).Select(x => x.Key.x + (x.Key.y * 100)).Sum().ToString();
 
    }
    
    private static (Dictionary<Location, char> grid, Location robotPosition) ParseGrid(List<string> input)
    {
        
        //Parse into grid
        Dictionary<Location, char> grid = [];
        Location robotPosition = new Location();
        foreach (var (row, y) in input.WithIndex()) 
        {
            foreach (var (col, x) in row.WithIndex())
            {
                switch (col)
                {
                    case '.':
                        break;
                    case '@':
                        robotPosition = new Location(x, y);
                        break;
                    default:
                        grid.Add((x, y), col);
                        break;
                }

                
            }
        }

        return (grid, robotPosition);
    }

    private static (Dictionary<Location, char> grid, Location robotPosition) ParseGridPart2(List<string> input)
    {
        
        //Parse into grid
        Dictionary<Location, char> grid = [];
        Location robotPosition = new Location();
        foreach (var (row, y) in input.WithIndex()) 
        {
            foreach (var (col, x) in row.WithIndex())
            {
                switch (col)
                {
                    case '.':
                        break;
                    case '@':
                        robotPosition = new Location(x*2, y);
                        break;
                    case '#':
                        grid.Add((x*2, y), '#');
                        grid.Add((x*2 + 1, y), '#');
                        break;
                    default:
                        grid.Add((x*2, y), '[');
                        grid.Add((x*2 + 1, y), ']');
                        break;
                }

                
            }
        }

        return (grid, robotPosition);
    }

    public int Day => 15;
}
