using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day22 : ISolution
{
    public int Mix(int secret, int toMix)
    {
        return toMix ^ secret ;
    }

    public int Prune(int secret)
    {
        return Grid.Mod(secret, 16777216);
    }
    public string PartOne(IEnumerable<string> input)
    {
        long toReturn = 0;
        List<int> changes = new List<int>();
        List<int> sellPrices = new List<int>();
        
        
        foreach (var line in input)
        {
            var secret = int.Parse(line);

            for (var i = 0; i <= 2000; i++)
            {
                secret = Prune(Mix(secret, secret * 64));
                secret = Prune(Mix(secret, secret / 32));
                secret = Prune(Mix(secret, secret * 2048));

                var price = secret % 10;
                
                sellPrices.Add(price);
                if (i == 0)
                {
                    changes.Add(Int32.MinValue);
                    
                }
                else
                {
                    changes.Add(price - changes[i-1]);
                }
            }
            
            
            
            toReturn += secret;
        }

        return toReturn.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        Dictionary<(int, int, int, int), int> AllPrices = new();
        foreach (var line in input)
        {
            var secret = int.Parse(line);
            List<int> changes = new List<int>();
            List<int> sellPrices = new List<int>();
            
            Dictionary<(int, int, int, int), int> prices = new Dictionary<(int, int, int, int), int>();

            changes.Add(Int32.MinValue);
            sellPrices.Add(secret %10);
            for (var i = 1; i <= 2000; i++)
            {
                secret = Prune(Mix(secret, secret * 64));
                secret = Prune(Mix(secret, secret / 32));
                secret = Prune(Mix(secret, secret * 2048));

                var price = secret % 10;
                
                sellPrices.Add(price);
                changes.Add(price - sellPrices[i-1]);
                
                
                if (i > 4)
                {
                    var key = (changes[i - 3], changes[i - 2], changes[i - 1], changes[i]);
                    if (!prices.ContainsKey(key))
                    {
                        prices.Add((changes[i-3], changes[i-2], changes[i-1], changes[i]), price);  
                    }
                }
                
            }

            foreach (var kvp in prices)
            {
                if (AllPrices.ContainsKey(kvp.Key))
                {
                    AllPrices[kvp.Key] = kvp.Value + AllPrices[kvp.Key];
                }
                else
                {
                    AllPrices[kvp.Key] = kvp.Value; 
                    
                }
            }
        }

        return AllPrices.Values.Max().ToString();
    }

    public int Day => 22;
}
