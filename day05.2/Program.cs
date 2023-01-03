const int Length = 1000;

var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var grid = new int[Length, Length];

foreach (var line in input)
{
    var parts = line.Split("->");
    var coords = parts.Select(coord => coord.Split(',').Select(int.Parse).ToArray()).ToArray();
    var (x1, y1) = (coords[0][0], coords[0][1]);
    var (x2, y2) = (coords[1][0], coords[1][1]);

    if (x1 != x2 && y1 != y2)
    {
        if (Math.Abs(x1 - x2) != Math.Abs(y1 - y2)) throw new InvalidOperationException();
        // Console.WriteLine($"{x1}/{y1} -> {x2}/{y2}");
        for (int dx = 0, dy = 0; dx <= Math.Abs(x1 - x2); ++dx, ++dy)
        {
            var (x, y) = (x1 + dx * Math.Sign(x2 - x1), y1 + dy * Math.Sign(y2 - y1));
            // Console.WriteLine($"{x}/{y}");
            ++grid[y, x];
        }
        continue;
    }

    var (xMin, yMin) = (Math.Min(x1, x2), Math.Min(y1, y2));
    var (xMax, yMax) = (Math.Max(x1, x2), Math.Max(y1, y2));

    for (int y = yMin; y <= yMax; ++y)
    {
        for (int x = xMin; x <= xMax; ++x)
        {
            ++grid[y, x];
        }
    }
}

int score = 0;
for (int y = 0; y < Length; ++y)
{
    for (int x = 0; x < Length; ++x)
    {
        if (grid[y, x] > 1) ++score;
        // if (grid[y, x] > 0) Console.Write(grid[y, x]);
        // else Console.Write('.');
    }
    // Console.WriteLine();
}

Console.WriteLine($"{score}");
