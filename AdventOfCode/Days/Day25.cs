using System.Runtime.InteropServices.JavaScript;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day25 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var inputStrings = input.ToList();

        List<int[]> keys = new List<int[]>();
        List<int[]> locks = new List<int[]>();

        foreach (var inputGrid in inputStrings.Chunk(8))
        {
            var grid = inputGrid.Take(7).ToArray();
            var isLock = grid[0].All(x => x == '#');

            var values = new int[grid[0].Length];
            foreach (var row in grid)
            {
                foreach (var (cell, idx) in row.WithIndex())
                {
                    if (cell == '#')
                    {
                        values[idx] += 1;
                    }
                }
            }
            
            //Remove the edge
            values = values.Select(x => x-1).ToArray();

            if (isLock)
            {
                locks.Add(values);
            }
            else
            {
                keys.Add(values);
            }

        }

        var toReturn = 0;
        foreach (var key in keys)
        {
            foreach (var aLock in locks)
            {
                var isFit = key.Zip(aLock, (x, y) => x + y).All(x => x <=5);
                if (isFit)
                {
                    toReturn++;
                }
            }
        }

        return toReturn.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        return "AOC_2025_DONE";
    }

    public int Day => 25;
}
