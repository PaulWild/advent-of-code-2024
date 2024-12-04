using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day04 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
 
            var grid = ParseGrid(input.ToList());

            List<Location> paths = [(0, 1), (0, -1), (1, 0), (-1, 0), (1,1), (-1,1), (1,-1),(-1,-1)];
            List<char> xmas = ['X', 'M', 'A', 'S'];
            
            var xs = grid.Where(kvp => kvp.Value == 'X').ToList();

            var foundCount = 0;
            foreach (var x in xs)
            foreach (var path in paths)
            {
                var step = 1;
                var found = true;
                while (step < xmas.Count)
                {
                    grid.TryGetValue((x.Key.x + (path.x * step), x.Key.y + (path.y * step)),
                        out var letter);

                    if (letter == xmas[step])
                    {
                        step++; 
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }

                if (found) foundCount++;

            }

            return foundCount.ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var grid = ParseGrid(input.ToList());

        List<(Location, Location)> paths = [((-1, -1), (1, 1)), ((-1, 1), (1, -1))];

        var aaas = grid.Where(kvp => kvp.Value == 'A').ToList();

        var foundCount = 0;
        foreach (var a in aaas)
        {
            //Diag one. 
            grid.TryGetValue((a.Key.x + (paths[0].Item1.x), a.Key.y + (paths[0].Item1.y)),
                out var topLeft);
            //Diag one. 
            grid.TryGetValue((a.Key.x + (paths[0].Item2.x), a.Key.y + (paths[0].Item2.y)),
                out var bottomRight);
            //Diag two. 
            grid.TryGetValue((a.Key.x + (paths[1].Item1.x), a.Key.y + (paths[1].Item1.y)),
                out var bottomLeft);
            //Diag two. 
            grid.TryGetValue((a.Key.x + (paths[1].Item2.x), a.Key.y + (paths[1].Item2.y)),
                out var topRight);

            var isDiagOneMas = (topLeft == 'M' && bottomRight == 'S') || (topLeft == 'S' && bottomRight == 'M');
            var isDiagTowMas = (bottomLeft == 'M' && topRight == 'S') || (bottomLeft == 'S' && topRight == 'M');
            if (isDiagOneMas && isDiagTowMas) foundCount++;
            
        }

        return foundCount.ToString();
    }
    
    private static Dictionary<Location, char> ParseGrid(List<string> input)
    {
        
        //Parse into grid
        Dictionary<Location, char> grid = new();
        foreach (var (row, x) in input.WithIndex()) 
        {
            foreach (var (col, y) in row.WithIndex())
            {
                grid.Add((x,y), col);
            }
        }

        return grid;
    }


    public int Day => 04;
}
