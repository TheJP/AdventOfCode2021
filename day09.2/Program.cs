var grid = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);
(int x, int y)[] perms = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

for (int i = 0; i < grid.Length; ++i)
{
    grid[i] = $"9{grid[i]}9";
}
var border = new string('9', grid[0].Length);
grid = grid.Prepend(border).Append(border).ToArray();

var colour = grid.Select(row => row.Select(_ => -1).ToArray()).ToArray();
var currentColour = 0;
var counts = new List<int>();
for (int y = 1; y < grid.Length - 1; ++y)
{
    for (int x = 1; x < grid[y].Length - 1; ++x)
    {
        if (colour[y][x] >= 0 || grid[y][x] == '9') continue;
        var count = 1;
        var bfs = new Queue<(int x, int y)>();
        colour[y][x] = currentColour;
        bfs.Enqueue((x, y));
        while (bfs.Count > 0)
        {
            var current = bfs.Dequeue();
            foreach (var perm in perms)
            {
                var (nx, ny) = (current.x + perm.x, current.y + perm.y);
                if (colour[ny][nx] >= 0 || grid[ny][nx] == '9') continue;
                colour[ny][nx] = currentColour;
                bfs.Enqueue((nx, ny));
                ++count;
            }
        }
        counts.Add(count);
        ++currentColour;
    }
}

counts.Sort();
counts.Reverse();
Console.WriteLine($"{counts.Take(3).Aggregate(1, (a, b) => a * b)}");
