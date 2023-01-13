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
bool visitedTwice = false;

void DepthFirstSearch(string current)
{
    if (current == "end")
    {
        ++count;
        return;
    }

    foreach (var neighbour in graph[current])
    {
        bool removeVisitTwice = false;
        if (char.IsLower(neighbour[0]))
        {
            if (visited.Contains(neighbour))
            {
                if (visitedTwice || neighbour == "start") continue;
                else
                {
                    visitedTwice = true;
                    removeVisitTwice = true;
                }
            }
            else visited.Add(neighbour);
        }
        DepthFirstSearch(neighbour);
        if (char.IsLower(neighbour[0]))
        {
            if (removeVisitTwice) visitedTwice = false;
            else visited.Remove(neighbour);
        }
    }
}

DepthFirstSearch("start");
Console.WriteLine($"{count}");
