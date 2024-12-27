namespace AdventOfCode.Days;

public class Day24 : ISolution
{
    public record Operation(string Left, string Right, string Result, string Gate);
    
    public class OperationGraph(Operation operation)
    {
        public Operation Operation { get; } = operation;

        public OperationGraph? Left { get; set; }
        
        public OperationGraph? Right { get; set; }

        public List<Operation> InvalidOperations(int? depth = null, string? previousOperation = null)
        {
            var root = depth == null;
            depth ??= int.Parse(Operation.Result[1..]);

            var depthString = depth < 10 ? $"0{depth}" : depth.ToString();
            var depthPlusOneString = depth+1 < 10 ? $"0{depth+1}" : (depth+1).ToString();
            // we are at the root so let us do root things 
            if (root)
            {
                //root operation should always be an XOR.
                if (Operation.Gate != "XOR")
                {
                    return [Operation];
                }

                // we are doing the 0 one and it's fine;
                if (depthString == "00" && Left == null && Right == null)
                {
                    return [];
                }
            }

            // need to do terminal nodes
            var terminal = Left == null && Right == null;

            switch (terminal)
            {
                case true when depth > 0:
                {
                    var terminalString = previousOperation == "XOR" ? "XOR" : previousOperation == "AND" ? "XOR" : "AND";

                    if (depthPlusOneString != null && (!Operation.Left.Contains(depthPlusOneString) ||
                                                       !Operation.Right.Contains(depthPlusOneString) ||
                                                       Operation.Gate != terminalString))
                    {
                        return [Operation];
                    }
    
                    return [];
                    
                }
                case true:
                {
                    var validEndMinusOne = depthPlusOneString != null && (Operation.Left.Contains(depthPlusOneString) ||
                                                                          Operation.Right.Contains(depthPlusOneString) ||
                                                                          Operation.Gate == "AND");
                
                    var validEnd = depthString != null && (Operation.Left.Contains(depthString) ||
                                                           Operation.Right.Contains(depthString) ||
                                                           Operation.Gate == "XOR");

                    if (!validEnd && !validEndMinusOne)
                    {
                        return [Operation];
                    }
                 
                    return [];
                    
                }
            }

            // need to do expando nodes

            var expandoGate = previousOperation switch
            {
                null => "XOR",
                "XOR" => "OR",
                "AND" => "OR",
                _ => "AND"
            };
            
            var invalidExpando = Operation.Gate != expandoGate;
            
            if (invalidExpando)
            {
               return [Operation];
            }
            
            var a = Left?.InvalidOperations(Operation.Gate.Contains("OR") ? depth-1 : depth, Operation.Gate) ?? [];
            var b = Right?.InvalidOperations(Operation.Gate.Contains("OR") ? depth-1 : depth, Operation.Gate) ?? [];

            var toReturn = new List<Operation>();
            toReturn.AddRange(a);
            toReturn.AddRange(b);
            
            return toReturn;
        }
    }
    public string PartOne(IEnumerable<string> input)
    {
        var (values, instructions) = Dictionary(input);

        ConnectWires(instructions, values);

        return Convert.ToInt64(string.Join("", values.Where(kvp => kvp.Key.StartsWith("z")).OrderByDescending(kvp => int.Parse(kvp.Key.Substring(1))).Select(kvp => kvp.Value)),2).ToString();
        
    }

    private static void ConnectWires(List<Operation> instructions, Dictionary<string, int> values)
    {
        var resultsParsed = false;

        while (!resultsParsed)
        {
            resultsParsed = true;

            foreach (var instruction in instructions)
            {

                if (values.ContainsKey(instruction.Left) && values.ContainsKey(instruction.Right) &&
                    values.ContainsKey(instruction.Result))
                {
                    //Solved this one
                } else if (values.ContainsKey(instruction.Left) && values.ContainsKey(instruction.Right))
                {
                    //Can solve, 
                    var result = 0;
                    switch (instruction.Gate)
                    {
                        case "AND":
                            result = values[instruction.Left] == values[instruction.Right] && values[instruction.Left] == 1 ? 1 : 0;
                            break;
                        case "OR":
                            result = values[instruction.Left] == 1 || values[instruction.Right] == 1 ? 1 : 0;
                            break;
                        case "XOR":
                            result = values[instruction.Left] != values[instruction.Right] ? 1 : 0;
                            break;
                    }
                    values[instruction.Result] = result;
                }
                else
                {
                    resultsParsed = false;
                }
            }
        }
    }

    private static (Dictionary<string, int> values, List<Operation> instructions) Dictionary(IEnumerable<string> input)
    {
        bool parseInstructions = false;
        Dictionary<string, int> values = new ();

        var instructions = new List<Operation>();
        
        foreach (var row in input)
        {
            if (string.IsNullOrEmpty(row))
            {
                parseInstructions = true;
                
                
            } else if (parseInstructions)
            {
                var split = row.Split("->");
                var operationString = split[0];
                var result = split[1].Trim();

                var operations = operationString.Split(" ");
                var left = operations[0]; 
                var right = operations[2];
                var operation = operations[1];
                
                instructions.Add(new Operation(left, right, result, operation));

            }
            else
            {
                var split = row.Split(": ");
                values[split.First()] = int.Parse(split[1]);
            }
        }

        return (values, instructions);
    }

    private static OperationGraph BuildOperationGraph(List<Operation> operations, Operation operation)
    {
        var graph = new OperationGraph(operation);

        if (!(operation.Left.StartsWith('x') || operation.Left.StartsWith('y')))
        {
            graph.Left = BuildOperationGraph(operations, operations.Single(x => x.Result == operation.Left));
        }
        
        if (!(operation.Right.StartsWith('x') || operation.Right.StartsWith('y')))
        {
            graph.Right = BuildOperationGraph(operations, operations.Single(x => x.Result == operation.Right));
        }

        return graph;
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var (values, instructions) = Dictionary(input);
        
        ConnectWires(instructions, values);
        
        var zGraphs = new List<OperationGraph?>();
        foreach (var instruction in instructions.Where(x => x.Result.StartsWith("z")))
        {
            zGraphs.Add(BuildOperationGraph(instructions, instruction));
        }

        var ordered = zGraphs.OrderBy(x => x?.Operation.Result).ToList();

        var toReturn = new List<Operation>();
        foreach (var graph in ordered)
        {
            toReturn.AddRange(graph?.InvalidOperations() ?? []);
        }

        var d = toReturn.Distinct().ToList();
        
        // 45 is an or, kind of makes sense? 
        var foo = d.Where(x => x.Result != "z45").OrderBy(x => x.Result).Select(x => x.Result);
        
    
        return string.Join(",", foo);
    }

    public int Day => 24;
}
