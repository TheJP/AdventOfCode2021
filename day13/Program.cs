var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);

List<(int x, int y)> points = new();
int i = 0;
for (; !string.IsNullOrWhiteSpace(input[i]); ++i)
{
    var parts = input[i].Split(',');
    points.Add((int.Parse(parts[0]), int.Parse(parts[1])));
}

List<(char axis, int value)> folds = new();
for (++i; i < input.Length; ++i)
{
    var parts = input[i].Split('=');
    folds.Add((parts[0][^1], int.Parse(parts[1])));
}

void Fold((char axis, int value) fold)
{
    for (int p = 0; p < points.Count; ++p)
    {
        if (fold.axis == 'x')
        {
            if (points[p].x < fold.value) continue;
            points[p] = (fold.value + fold.value - points[p].x, points[p].y);
        }
        if (fold.axis == 'y')
        {
            if (points[p].y < fold.value) continue;
            points[p] = (points[p].x, fold.value + fold.value - points[p].y);
        }
    }
}

void PrintPoints()
{
    var min = (x: points.Min(p => p.x), y: points.Min(p => p.y));
    var max = (x: points.Max(p => p.x), y: points.Max(p => p.y));
    var map = points.ToHashSet();
    for (int y = min.y; y <= max.y; ++y)
    {
        for (int x = min.x; x <= max.x; ++x)
        {
            Console.Write(map.Contains((x, y)) ? '#' : '.');
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

Fold(folds.First());
Console.WriteLine($"Task 1: {points.Distinct().Count()}");
