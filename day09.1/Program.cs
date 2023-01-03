var grid = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);

(int x, int y)[] perms = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

int score = 0;
for (int y = 0; y < grid.Length; ++y)
{
    for (int x = 0; x < grid[y].Length; ++x)
    {
        var lowest = true;
        foreach (var perm in perms)
        {
            var (nx, ny) = (x + perm.x, y + perm.y);
            if (nx < 0 || ny < 0 || nx >= grid[0].Length || ny >= grid.Length) continue;
            if (grid[ny][nx] <= grid[y][x]) lowest = false;
        }

        if (lowest) score += (grid[y][x] - '0') + 1;
    }
}

Console.WriteLine(score);
