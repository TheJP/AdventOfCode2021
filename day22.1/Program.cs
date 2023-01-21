const int Size = 50;
const int Dimensions = 2 * Size + 1;
var cubes = new bool[Dimensions, Dimensions, Dimensions];

var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

foreach (var line in input)
{
    var on = line.StartsWith("on");
    var parts = line[line.IndexOf("x=")..]
        .Split(',').Select(axis => axis[2..]
            .Split("..")
            .Select(int.Parse)
            .ToArray()
        )
        .ToArray();

    // for (int i = 0; i < 3; ++i)
    //     if (parts[i][0] > parts[i][1]) Console.WriteLine(line);

    var min = (
        x: Math.Max(0, parts[0][0] + Size),
        y: Math.Max(0, parts[1][0] + Size),
        z: Math.Max(0, parts[2][0] + Size)
    );

    var max = (
        x: Math.Min(Dimensions - 1, parts[0][1] + Size),
        y: Math.Min(Dimensions - 1, parts[1][1] + Size),
        z: Math.Min(Dimensions - 1, parts[2][1] + Size)
    );

    for (int z = min.z; z <= max.z; ++z)
        for (int y = min.y; y <= max.y; ++y)
            for (int x = min.x; x <= max.x; ++x)
                cubes[z, y, x] = on;
}

int count = 0;
for (int z = 0; z < Dimensions; ++z)
    for (int y = 0; y < Dimensions; ++y)
        for (int x = 0; x < Dimensions; ++x)
            if (cubes[z, y, x]) ++count;

Console.WriteLine(count);
