using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day14 : ISolution
{
    public record  Robot(Location StartingPosition, Location Velocity);
    
    public string PartOne(IEnumerable<string> input)
    {
        var robots = ParseRobots(input);
        var width = 101;
        var height = 103;
            

        for (var i = 1; i <= 100; i++)
        {
            var robot = robots.First;

            while (robot is not null)
            {
                var newX = Grid.Mod((robot.Value.StartingPosition.x + robot.Value.Velocity.x), width);
                var newY = Grid.Mod((robot.Value.StartingPosition.y + robot.Value.Velocity.y), height);

                robot.Value = robot.Value with { StartingPosition = (newX, newY) };
                
                robot = robot.Next;
            }
        }

        var positions = robots.Select(x => x.StartingPosition).ToList();
        var midX = (width - 1) / 2;
        var midY = (height - 1) / 2;

        var q1 = positions.Count(position => position.x < midX && position.y < midY);
        var q2 = positions.Count(position => position.x > midX && position.y < midY);
        var q3 = positions.Count(position => position.x < midX && position.y > midY);
        var q4 = positions.Count(position => position.x > midX && position.y > midY);
        
        return (q1 * q2 * q3 * q4).ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var robots = ParseRobots(input);
        var width = 101;
        var height = 103;
        var printTree = false;


        var firstPositions =robots.Select(x => x.StartingPosition).ToHashSet();
        var potential = new HashSet<Location>();
        var potentialIteration = 0;
        
        var currentDif = int.MinValue;
        for (var i = 1;; i++)
        {
            var robot = robots.First;

            while (robot is not null)
            {
                var newX = Grid.Mod((robot.Value.StartingPosition.x + robot.Value.Velocity.x), width);
                var newY = Grid.Mod((robot.Value.StartingPosition.y + robot.Value.Velocity.y), height);

                robot.Value = robot.Value with { StartingPosition = (newX, newY) };

                robot = robot.Next;
            }

            var positions = robots.Select(x => x.StartingPosition).ToHashSet();
            var midX = (width - 1) / 2;
            var midY = (height - 1) / 2;


            var q1 = positions.Count(position => position.x < midX && position.y < midY);
            var q2 = positions.Count(position => position.x > midX && position.y < midY);
            var q3 = positions.Count(position => position.x < midX && position.y > midY);
            var q4 = positions.Count(position => position.x > midX && position.y > midY);


            var diff = new List<int>([q1, q2, q3, q4]).Max();
            if (diff > currentDif)
            {
                currentDif = diff;
                potential = positions;
                potentialIteration = i;
            }

            //We have reached a loop
            if (!firstPositions.Except(positions).Any())
            {
                break;
            }
        }
        
        if (printTree)
        {
            Console.Clear();
            var c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            for (var y = 0; y < height; y++)
            {
                Console.WriteLine();
                for (var x = 0; x < width; x++)
                {
                    Console.Write(potential.Contains((x, y)) ? '#' : ' ');
                }
            }


            Console.Write(potentialIteration);
            Console.Write($" {currentDif} ");
            Thread.Sleep(500);
            Console.ForegroundColor = c;
        }

  
        return potentialIteration.ToString();
    }
    
    private static LinkedList<Robot> ParseRobots(IEnumerable<string> input)
    {
        return new LinkedList<Robot>(input
            .Select(row => Regex.Matches(row, @"-?\d+").Select(m => int.Parse(m.Groups[0].Value)).ToArray())
            .Select(nums => new Robot((nums[0], nums[1]), (nums[2], nums[3])))
            );
    }

    public int Day => 14;
}
