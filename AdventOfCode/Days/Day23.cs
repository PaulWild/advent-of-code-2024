namespace AdventOfCode.Days;

public class Day23 : ISolution
{
 
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
    {
        int i = 0;
        foreach (var item in items)
        {
            if (count == 1)
                yield return new T[] { item };
            else
            {
                foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                    yield return new T[] { item }.Concat(result);
            }

            ++i;
        }
    }
    
    class Network
    {
        public Network(string computer)
        {
            Computer = computer;
        }

        public string Computer { get; }

        public List<Network> Connections { get; set; } = new();

        public List<string> FullNetwork()
        {
            Dictionary<string, HashSet<string>> argh = new();
            var computers = Connections.Select(c => c.Computer).ToHashSet();
            computers.Add(Computer);
            argh.Add(Computer, computers);
            
            foreach (var connection in Connections)
            {
                var connectionConnections = connection.Connections.Select(x => x.Computer).ToHashSet();
                connectionConnections.Add(connection.Computer);
                argh.Add(connection.Computer, connectionConnections);
            }
            
            for (int i = computers.Count - 1; i >= 2; i--)
            {
                var foo = GetPermutations(computers.ToList(), i).ToList();
                

                foreach (var combo in foo)
                {
                    var test = combo.Select(x => argh[x]).ToList();

                    var isNetworked = combo.All(x => test.All(y => y.Contains(x)));

                    if (isNetworked)
                    {
                        return combo.ToList();
                    }
                }
            }

            throw new Exception("Nope");
        }

        public IEnumerable<List<string>> ComputersInNetowrk(int networkLink, List<string>? computersInNetowrk = null, string startComputer = "")
        {
            if (computersInNetowrk is null)
            {
                computersInNetowrk = new List<string>();
            }

            if (startComputer == "")
            {
                startComputer = Computer;
            }

            if (networkLink == 0 && startComputer == Computer)
            {
                yield return computersInNetowrk;
            }
            else if (networkLink != 0)
            {
                computersInNetowrk.Add(Computer);

                foreach (var connection in Connections)
                {
                    var newList = computersInNetowrk.Select(x => x).ToList();
                    var huh = connection.ComputersInNetowrk(networkLink - 1, newList, startComputer);
                    foreach (var result in huh)
                    {
                        yield return result;
                    }
                }
            }
        }

    }
    public string PartOne(IEnumerable<string> input)
    {
        var networks = CreateNetworks(input);

        var foo = networks.SelectMany(x => x.ComputersInNetowrk(3)).Where(x => x.Any(y => y.StartsWith("t"))).ToList();
        
        // 6 is the number of combinations that 3 things can have
        return (foo.Count/6).ToString();
    }

    private static List<Network> CreateNetworks(IEnumerable<string> input)
    {
        List<Network> networks = new();
        foreach (var line in input)
        {
            var computers = line.Split("-");

            var computer1 = networks.FirstOrDefault(x => x.Computer == computers[0]) ?? new Network(computers[0]);
            var computer2 = networks.FirstOrDefault(x => x.Computer == computers[1]) ?? new Network(computers[1]);
            
            computer1.Connections.Add(computer2);
            computer2.Connections.Add(computer1);

            if (networks.All(x => x.Computer != computers[0]))
            {
                networks.Add(computer1);      
            }

            if (networks.All(x => x.Computer != computers[1]))
            {
                networks.Add(computer2);      
            }
        }

        return networks;
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var networks = CreateNetworks(input);
        var fullNetworks = networks.Select(x => x.FullNetwork()).ToList();
        var max= fullNetworks.OrderByDescending(x => x.Count).First().ToList();
        max.Sort();
        
        return string.Join(',', max);
    }

    public int Day => 23;
}
