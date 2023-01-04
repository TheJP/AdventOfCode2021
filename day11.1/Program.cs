var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);
var grid = input.Select(row => row.Select(c => (int)(c - '0')).ToArray()).ToArray();
(int x, int y)[] perms = new[]
{
    (-1, -1), (0, -1), (1, -1),
    (-1, 0), (1, 0),
    (-1, 1), (0, 1), (1, 1),
};

int total = 0;

for (int i = 0; i < 100; ++i)
{
    int flashes = 0;
    var didFlash = grid.Select(row => row.Select(_ => false).ToArray()).ToArray();
    var bfs = new Queue<(int x, int y)>();

    for (int y = 0; y < grid.Length; ++y)
    {
        for (int x = 0; x < grid[y].Length; ++x)
        {
            ++grid[y][x];
            if (grid[y][x] > 9)
            {
                didFlash[y][x] = true;
                ++flashes;
                bfs.Enqueue((x, y));
            }
        }
    }

    while (bfs.Count > 0)
    {
        var current = bfs.Dequeue();
        grid[current.y][current.x] = 0;
        foreach (var perm in perms)
        {
            var (nx, ny) = (current.x + perm.x, current.y + perm.y);
            if (nx < 0 || ny < 0 || nx >= grid[0].Length || ny >= grid.Length) continue;
            if (didFlash[ny][nx]) continue;

            ++grid[ny][nx];
            if (grid[ny][nx] <= 9) continue;

            didFlash[ny][nx] = true;
            ++flashes;
            bfs.Enqueue((nx, ny));
        }
    }

    total += flashes;
}

Console.WriteLine(total);
