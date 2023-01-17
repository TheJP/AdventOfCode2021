var input = File.ReadAllText(Environment.GetCommandLineArgs()[1]);

const string prefix = "target area: x=";
if (!input.StartsWith(prefix)) throw new InvalidOperationException();
input = input[(prefix.Length)..];

var parts = input.Split(',', 2);
var xRange = parts[0].Trim().Split("..", 2);
var yRange = parts[1].Trim().Split("..", 2);

var xMin = int.Parse(xRange[0]);
var xMax = int.Parse(xRange[1]);
var yMin = int.Parse(yRange[0][2..]);
var yMax = int.Parse(yRange[1]);

var xHits = new HashSet<int>();
var xAllAbove = int.MaxValue;
var xVelocities = new Dictionary<int, List<int>>(); // count -> initVelocity[]
var xAllAboveVelocities = new Dictionary<int, List<int>>();
for (int x = xMax; x >= 1; --x)
{
    int simulation = x;
    int velocity = x - 1;
    int count = 1;
    while (simulation <= xMax && velocity > 0)
    {
        if (xMin <= simulation && simulation <= xMax)
        {
            xHits.Add(count);

            if (!xVelocities.ContainsKey(count)) xVelocities.Add(count, new());
            xVelocities[count].Add(x);

            if (velocity <= 1)
            {
                xAllAbove = Math.Min(xAllAbove, count);

                if (!xAllAboveVelocities.ContainsKey(count)) xAllAboveVelocities.Add(count, new());
                xAllAboveVelocities[count].Add(x);
            }
        }
        simulation += velocity;
        --velocity;
        ++count;
    }
}

// foreach (var x in xHits)
// {
//     Console.Write($"{x} || ");
// }
// Console.Write($">{xAllAbove}");

// Console.Write($"{yMin} {yMax}");;

var velocities = new HashSet<(int, int)>();
for (int y = -10_000; y < 10_000; ++y)
{
    int simulation = y;
    int velocity = y - 1;
    int count = 1;
    int best = simulation;

    while (simulation >= yMin)
    {
        if (yMin <= simulation && simulation <= yMax)
        {
            if (xHits.Contains(count))
            {
                foreach (var x in xVelocities[count]) velocities.Add((x, y));
            }
            if (count > xAllAbove)
            {
                foreach (var xCount in xAllAboveVelocities.Keys)
                {
                    if (count > xCount)
                    {
                        foreach (var x in xAllAboveVelocities[xCount]) velocities.Add((x, y));
                    }
                }
            }
        }
        simulation += velocity;
        --velocity;
        ++count;
    }
}

// foreach (var pair in velocities)
// {
//     Console.WriteLine($"{pair.Item1}/{pair.Item2}");
// }

Console.WriteLine("{0}", velocities.Count);
