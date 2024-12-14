using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public class Day13 : ISolution
{
    public record Claw((long x, long y) A, (long x, long y) B, (long x, long y) Prize);
    public string PartOne(IEnumerable<string> input)
    {
        var games = GetGames(input);
        return CalculateTokenSpend(games);
    }

    private static List<Claw> GetGames(IEnumerable<string> input, long prizeLocationOffset = 0)
    {
        var games = new List<Claw>();
        foreach (var game in input.Chunk(4))
        {
            (long x, long y) GetLocation(string row, long add=0)
            {
                var nums = Regex.Matches(row, @"\d+").Select(m => long.Parse(m.Groups[0].Value)).ToArray();
                return new (nums[0] + add, nums[1]+ add);
            }
     
            games.Add(new Claw(GetLocation(game[0]), GetLocation(game[1]), GetLocation(game[2], prizeLocationOffset)));
            
        }

        return games;
    }

    private static string CalculateTokenSpend(List<Claw> games)
    {
        long total = 0;
        foreach (var game in games)
        {
            var b = (game.A.y * game.Prize.x - game.Prize.y * game.A.x) / (double)(game.A.y*game.B.x - game.B.y*game.A.x);
            var a = (game.Prize.x - b * game.B.x) / game.A.x;

            if (a % 1 == 0 && b % 1 ==0)
            {
                total += (long)a * 3 + (long)b * 1;
            }
        }
        return total.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var games = GetGames(input, 10000000000000);
        return CalculateTokenSpend(games);
    }

    public int Day => 13;
}
