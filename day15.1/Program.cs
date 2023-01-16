using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);

var grid = new List<int[]>();

string? line;
while ((line = input.ReadLine()) != null)
{
    grid.Add(line.Select(c => (int)(c - '0')).Prepend((int)short.MaxValue).Append((int)short.MaxValue).ToArray());
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

    distance[y][x] = d;
    if (y == grid.Count - 2 && x == grid[0].Length - 2)
    {
        Console.WriteLine("{0}", d);
        break;
    }

    var dx = new int[] { 0, 0, -1, 1 };
    var dy = new int[] { -1, 1, 0, 0 };
    for (int i = 0; i < 4; ++i)
    {
        var newD = d + grid[y + dy[i]][x + dx[i]];
        if (newD < distance[y + dy[i]][x + dx[i]])
        {
            pq.Enqueue((x + dx[i], y + dy[i]), newD);
        }
    }
}

// for (int y = 0; y < distance.Length; ++y)
// {
//     for (int x = 0; x < distance[y].Length; ++x)
//     {
//         Console.Write("{0} ", distance[y][x]);
//     }
//     Console.WriteLine();
// }
