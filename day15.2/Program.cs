using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);

var grid = new List<int[]>();

string? line;
while ((line = input.ReadLine()) != null)
{
    var row = line.Select(c => (int)(c - '0')).ToList();
    var count = row.Count;
    for (int i = 1; i < 5; ++i)
    {
        for (int j = 0; j < count; ++j)
        {
            row.Add((row[j] - 1 + i) % 9 + 1);
        }
    }
    row.Insert(0, (int)short.MaxValue);
    row.Add((int)short.MaxValue);
    grid.Add(row.ToArray());
}

var height = grid.Count;
for (int i = 1; i < 5; ++i)
{
    for (int y = 0; y < height; ++y)
    {
        var row = (int[])grid[y].Clone();
        for (int x = 1; x < row.Length - 1; ++x)
        {
            row[x] = (row[x] - 1 + i) % 9 + 1;
        }
        grid.Add(row);
    }
}

grid.Insert(0, grid[0].Select(_ => (int)short.MaxValue).ToArray());
grid.Add(grid[0]);

int[][] distance = grid.Select(row => row.Select(_ => int.MaxValue).ToArray()).ToArray();

var pq = new PriorityQueue<(int, int), int>();
pq.Enqueue((1, 1), 0);

while (pq.Count > 0)
{
    pq.TryDequeue(out var position, out var d);
    var (x, y) = position;
    if (x == 0 || y == 0 || x == grid[0].Length - 1 || y == grid.Count - 1)
    {
        continue;
    }

    if (y == grid.Count - 2 && x == grid[0].Length - 2)
    {
        Console.WriteLine("{0}", d);
        break;
    }

    var dx = new int[] { 0, 0, -1, 1 };
    var dy = new int[] { -1, 1, 0, 0 };
    for (int i = 0; i < 4; ++i)
    {
        var newY = y + dy[i];
        var newX = x + dx[i];
        var newD = d + grid[newY][newX];
        if (newD < distance[newY][newX])
        {
            distance[newY][newX] = newD;
            pq.Enqueue((newX, newY), newD);
        }
    }
}

// for (int y = 0; y < distance.Length; ++y)
// {
//     for (int x = 0; x < distance[y].Length; ++x)
//     {
//         Console.Write("{0} ", grid[y][x]);
//     }
//     Console.WriteLine();
// }
// Console.WriteLine();
