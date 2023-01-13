var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var graph = new Dictionary<string, List<string>>();

void AddEdge(string from, string to)
{
    if (!graph.ContainsKey(from)) graph.Add(from, new());
    graph[from].Add(to);
}

foreach (var line in input)
{
    var parts = line.Split('-');
    AddEdge(parts[0], parts[1]);
    AddEdge(parts[1], parts[0]);
}

int count = 0;
HashSet<string> visited = new();
visited.Add("start");

void DepthFirstSearch(string current)
{
    if (current == "end")
    {
        ++count;
        return;
    }

    foreach (var neighbour in graph[current])
    {
        if (char.IsLower(neighbour[0]))
        {
            if (visited.Contains(neighbour)) continue;
            visited.Add(neighbour);
        }
        DepthFirstSearch(neighbour);
        if (char.IsLower(neighbour[0])) visited.Remove(neighbour);
    }
}

DepthFirstSearch("start");
Console.WriteLine($"{count}");
