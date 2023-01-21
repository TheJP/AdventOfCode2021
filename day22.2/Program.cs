List<Cube> Intersect(List<Cube> left, List<Cube> right)
{
    List<Cube> intersections = new();

    for (int l = 0; l < left.Count; ++l)
    {
        for (int r = 0; r < right.Count; ++r)
        {
            var lr = left[l].Intersect(right[r]);
            if (!lr.HasValue) continue;

            var (intersection, leftRest, rightRest) = lr.Value;
            intersections.Add(intersection);
            if (rightRest.Any())
            {
                right[r] = rightRest.First();
                right.AddRange(rightRest.Skip(1));
            }
            else
            {
                right.RemoveAt(r);
                --r;
            }
            if (leftRest.Any())
            {
                left[l] = leftRest.First();
                left.AddRange(leftRest.Skip(1));
            }
            else
            {
                left.RemoveAt(l);
                --l;
                break;
            }
        }
    }

    return intersections;
}

var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

List<Cube> cubes = new();

var firstOn = false;
foreach (var line in input)
{
    // Console.WriteLine($"[{cubes.Select(c => c.Area).Sum()}] {string.Join(", ", cubes)}");
    // Console.WriteLine();
    var on = line.StartsWith("on");
    if (!firstOn && !on) continue;
    var parts = line[line.IndexOf("x=")..]
        .Split(',').Select(axis => axis[2..]
            .Split("..")
            .Select(int.Parse)
            .ToArray()
        )
        .ToArray();

    // for (int i = 0; i < 3; ++i)
    //     if (parts[i][0] > parts[i][1]) Console.WriteLine(line);

    var from = new Vector(parts[0][0], parts[1][0], parts[2][0]);
    var to = new Vector(parts[0][1], parts[1][1], parts[2][1]);
    var nextCube = new Cube(from, to, on);
    if (!firstOn && on)
    {
        firstOn = true;
        cubes.Add(nextCube);
        continue;
    }

    List<Cube> nextCubes = new() { nextCube };
    var intersections = Intersect(cubes, nextCubes);
    // Console.WriteLine($"{(on ? "Add" : "Remove")} {nextCube}");
    // Console.WriteLine($"intersection: [{intersections.Select(c => c.Area).Sum()}] {string.Join(", ", intersections)}");
    // Console.WriteLine($"new: [{nextCubes.Select(c => c.Area).Sum()}] {string.Join(", ", nextCubes)}");
    if (!on) continue;

    cubes.AddRange(nextCubes);
    cubes.AddRange(intersections.Select(c => new Cube(c.From, c.To, true)));
}

long area = cubes.Select(c => c.Area).Sum();
Console.WriteLine(area);
