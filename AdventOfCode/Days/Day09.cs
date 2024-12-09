using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day09 : ISolution
{
    private record struct Data(int? Id, int Start, int Length, string Type);
    
    public string PartOne(IEnumerable<string> input)
    {
        var hardDrive = ExpandHardDrive(input);

        var start = hardDrive.First;
        var end =hardDrive.Last;
        
        
        while (start != end)
        {
            if (start!.Value.Type == "data")
            {
                start = start.Next;
            } else if (end!.Value.Type == "space")
            {
                end = end.Previous;
            }
            else 
            {
                if (start.Value.Length < end.Value.Length)
                {
                    start.Value = start.Value with { Id = end.Value.Id, Type = "data"};
                    end.Value = end.Value with { Length = end.Value.Length - start.Value.Length };
                }
                else
                {
                    var originalLength = start.Value.Length;
                    var oldEnd = end;
                    start.Value = start.Value with { Id = end.Value.Id, Length = end.Value.Length, Type = "data"};
                    if (originalLength - end.Value.Length > 0)
                    {
                        hardDrive.AddAfter(start,
                            new Data(null, start.Value.Start + end.Value.Length, originalLength - end.Value.Length,
                                "space"));
                    }

                    end = end.Previous;
                    hardDrive.Remove(oldEnd);
                    
                }
            }
        }
        
        return CalculateChecksum(hardDrive);
    }

    private static string CalculateChecksum(LinkedList<Data> hardDrive)
    {
        long checkSum = 0;
        
        var data = hardDrive.First;
        do
        {
            for (long i = data!.Value.Start; i < data.Value.Start + data.Value.Length; i++)
            {
                long? curr = data.Value.Id;
                if (curr != null)
                {
                    checkSum += i * (long)curr;
                }
            }


            data = data.Next;
        } while (data != null);

        return checkSum.ToString();
    }

    private static LinkedList<Data> ExpandHardDrive(IEnumerable<string> input)
    {
        var compact= input.First().ToCharArray().Select(x => int.Parse(char.ToString(x))).ToList();
        
        LinkedList<string> operations = new (["data", "space"]);

        var operation = operations.First;
        var id = 0;
        var hardDrive = new LinkedList<Data>();
        var location = 0;
        
        foreach (var size in compact)
        {
            hardDrive.AddLast(new Data(operation!.Value == "data" ? id : null, location, size, operation.Value));
            location += size;

            if (operation.Value == "data")
            {
                id++;
            }
            operation = operation.NextOrFirst();
        }

        return hardDrive;
    }

    private static void MergeSpaces(LinkedList<Data> hardDrive)
    {
        var start = hardDrive.First;
        var next = start!.Next;

        while (next != null)
        {
            if (start!.Value.Type == "space" && next.Value.Type == "space")
            {
                start.Value = start.Value with { Length = start.Value.Length + next.Value.Length };
                hardDrive.Remove(next);
            }
            
            start = start.Next;
            next = start?.Next;
        }
    }
    public string PartTwo(IEnumerable<string> input)
    {
        var hardDrive = ExpandHardDrive(input);

        var start = hardDrive.First;
        var end =hardDrive.Last;

        var id = hardDrive.Max(x => x.Id);
        while (id >= 0) 
        {
            if (end!.Value.Id != id)
            {
                end = end.Previous;
            }
            else
            {
                while (start != end)
                {
                    if (start!.Value.Type == "space" && end.Value.Length <= start.Value.Length)
                    {
                        var originalLength = start.Value.Length;
                        start.Value = start.Value with { Id = end.Value.Id, Length = end.Value.Length, Type = "data" };
                        if (originalLength - end.Value.Length > 0)
                        {
                            hardDrive.AddAfter(start,
                                new Data(null, start.Value.Start + end.Value.Length, originalLength - end.Value.Length,
                                    "space"));
                        }

                        end.Value = end.Value with { Type = "space", Id = null };
                        MergeSpaces(hardDrive);


                        break;
                    }

                    start = start.Next;

                }


                id--;
                end = hardDrive.Last;
                start = hardDrive.First;
            }
        }

        return CalculateChecksum(hardDrive);
    }

    public int Day => 09;
}
