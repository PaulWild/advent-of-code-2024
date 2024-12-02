namespace AdventOfCode.Days;

public class Day02 : ISolution
{
    private static bool CheckSafety(List<int> report)
    {
        var firstDifference = report[0] - report[1];
        if (firstDifference == 0) return false; 
        var direction = firstDifference < 0 ? -1 : 1;

  
        for (var i = 0; i < report.Count-1; i++)
        {
            var diff = report[i] - report[i + 1];
            if (Math.Abs(diff) is > 3 or 0) return false;

            var step = diff < 0 ? -1 : 1;
            if (step != direction) return false;    
        }

        return true;
    }
    public string PartOne(IEnumerable<string> input)
    {
        var reports = input.Select(row => row.Split(" ").Select(int.Parse).ToList()).ToList();

        return reports.Where(CheckSafety).Count().ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var reports = input.Select(row => row.Split(" ").Select(int.Parse).ToList()).ToList();
        var count = 0;
        
        foreach (var report in reports)
        {

            var testReports = new List<List<int>> { report };
            for (var i = 0; i < report.Count; i++)
            {
                var newReport = report.Select(x => x).ToList();
                newReport.RemoveAt(i);
                testReports.Add(newReport);
            }

            if (testReports.Any(CheckSafety)) count++;
        }
        return count.ToString();
    }

    public int Day => 02;
}
