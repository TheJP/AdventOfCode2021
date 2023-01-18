var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);

List<List<Vector>> scanners = new();

int i = 0;
while (i < input.Length)
{
    if (!input[i].StartsWith("--- scanner")) throw new InvalidOperationException();
    ++i;
    List<Vector> beacons = new();
    for (; i < input.Length && !string.IsNullOrWhiteSpace(input[i]); ++i)
    {
        var beacon = input[i].Split(',').Select(int.Parse).ToArray();
        beacons.Add(new Vector(beacon[0], beacon[1], beacon[2]));
    }
    scanners.Add(beacons);
    ++i;
}

var transformations = new Matrix[scanners.Count];
var globalPositions = new HashSet<Vector>[scanners.Count];
var found = new bool[scanners.Count];

void Found(int index, Matrix transform)
{
    if (found[index]) throw new InvalidOperationException();
    found[index] = true;
    transformations[index] = transform;
    globalPositions[index] = new();
    foreach (var beacon in scanners[index]) globalPositions[index].Add(transform * beacon);
}

Found(0, Matrix.Identity());
var bfs = new Queue<int>();
bfs.Enqueue(0);

var rotations = new Matrix[24];
rotations[0] = Matrix.Identity();
for (int y = 0; y < 4; ++y)
{
    if (y > 0) rotations[y * 4] = rotations[(y - 1) * 4].RotateY();
    for (int x = 1; x < 4; ++x) rotations[y * 4 + x] = rotations[y * 4 + x - 1].RotateX();
}
{
    int r = 16;
    rotations[r] = rotations[0].RotateZ();
    for (int x = 1; x < 4; ++x) rotations[r + x] = rotations[r + x - 1].RotateX();
    r += 4;
    rotations[r] = rotations[r - 4].RotateZ().RotateZ();
    for (int x = 1; x < 4; ++x) rotations[r + x] = rotations[r + x - 1].RotateX();
}

// foreach (var rotation in rotations)
// {
//     rotation.Print();
// }

void CompareScanners(int current, int i)
{
    // Compare beacons of scanner current with i.
    for (int bc = 0; bc < scanners[current].Count - 11; ++bc) // If only 11 are left, we will not find an overlap of 12.
    {
        for (int bi = 0; bi < scanners[i].Count; ++bi)
        {
            // Assume beacons bc == bi: count overlap.
            foreach (var rotation in rotations)
            {
                var positionNew = (transformations[current] * scanners[current][bc]) - (rotation * scanners[i][bi]); // Assumed global position of scanner i.
                var transform = rotation.Translate(positionNew);

                int overlap = 0;
                for (int beacon = 0; beacon < scanners[i].Count; ++beacon)
                {
                    if (globalPositions[current].Contains(transform * scanners[i][beacon])) ++overlap;
                }

                if (overlap >= 12)
                {
                    Found(i, transform);
                    bfs.Enqueue(i);
                    return;
                }
            }
        }
    }
}

while (bfs.Count > 0)
{
    var current = bfs.Dequeue();

    for (i = 0; i < scanners.Count; ++i)
    {
        if (found[i]) continue;
        CompareScanners(current, i);
    }
}

if (found.Count(f => f) < scanners.Count) Console.WriteLine("Did not find every scanner");
HashSet<Vector> beaconPositions = new();
foreach (var beacon in globalPositions.Where(p => p != null).SelectMany(p => p))
{
    beaconPositions.Add(beacon);
}
Console.WriteLine($"Task 1: {beaconPositions.Count}");

int max = 0;
for (i = 0; i < transformations.Length; ++i)
{
    var position = transformations[i] * Vector.Zeros; // Global position of scanner i.
    for (int j = 0; j < transformations.Length; ++j)
    {
        var distance = position - (transformations[j] * Vector.Zeros);
        var manhatten = Math.Abs(distance.X) + Math.Abs(distance.Y) + Math.Abs(distance.Z);
        max = Math.Max(max, manhatten);
    }
}
Console.WriteLine($"Task 2: {max}");
