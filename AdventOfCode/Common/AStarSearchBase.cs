namespace AdventOfCode.Common;

public abstract record Node<TS>(TS Location);

public abstract class AStarSearch<T,TS> where T : Node<TS> where TS : notnull
{
    private static List<List<TS>> ReconstructAllPaths(Dictionary<T, HashSet<T>> cameFrom, T end, T start, List<TS> currentPath )
    {
        if (start.Equals(end))
        {
            return [currentPath];
        }
      
  
        var t = new List<List<TS>>();

            foreach (var tada in cameFrom[end])
            {
                var p = currentPath.Select(x => x).ToList();
                p.Add(tada.Location);
                t.AddRange(ReconstructAllPaths(cameFrom, tada, start, p.Select(x => x).ToList()));
            }
        

        return t;
    }
    
    private static List<List<TS>> ReconstructAllPaths(Dictionary<T, HashSet<T>> cameFrom, TS end, T start)
    {
        
        var current = cameFrom.Keys.FirstOrDefault(x => Equals(x.Location,  end));

        if (current == null)
        {
            throw new KeyNotFoundException();
        }

        
        var t = new List<List<TS>>();
        
        foreach (var tada in cameFrom[current])
        {
            t.AddRange(ReconstructAllPaths(cameFrom, tada, start, [end, tada.Location]));
        }
        

         t.ForEach(x => x.Reverse());
        return t;
    }
    
    protected abstract int H(T currentNode, TS end);

    protected abstract int G(int currentCost, T node, T oldNode);
    
    protected abstract IEnumerable<T> NextNodes(T node);
    
    public (IEnumerable<List<TS>> paths, int cost) Search(T start, TS end)
    {
        var openSet = new PriorityQueue<T, int>();
        openSet.Enqueue(start, 0);
        var cameFrom = new Dictionary<T, HashSet<T>>();
        Dictionary<T, int> costs = new()
        {
            [start] = 0
        };

        while (openSet.Count != 0)
        {
            var node = openSet.Dequeue();
            var currentCost = costs[node];

            foreach (var newNode in NextNodes(node))
            {
                var newCost = G(currentCost, newNode, node);

                if (Equals(node.Location, end))
                {
                    return (ReconstructAllPaths(cameFrom, end, start), currentCost);
                }

                if ((!costs.TryGetValue(newNode, out var value) || value < newCost) &&
                    costs.ContainsKey(newNode)) continue;
                
                costs[newNode] =  newCost;
                if (value == newCost && cameFrom.ContainsKey(newNode))
                {
                    cameFrom[newNode].Add(node);
                }
                else
                {
                    cameFrom[newNode] = [node];
                }

                openSet.Enqueue(newNode, newCost + H(newNode, end));
            }
        }

        throw new Exception("Could not solve search");
    }
}