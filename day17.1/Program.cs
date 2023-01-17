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
for (int x = xMin; x >= 1; --x)
{
    int simulation = x;
    int velocity = x - 1;
    int count = 1;
    while (simulation <= xMax && velocity > 0)
    {
        if (xMin <= simulation && simulation <= xMax)
        {
            xHits.Add(count);
            if (velocity <= 1) xAllAbove = Math.Min(xAllAbove, count);
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

var highest = new List<int>();
for (int y = 1; y < 10000; ++y)
{
    int simulation = y;
    int velocity = y - 1;
    int count = 1;
    int best = simulation;

    while (simulation >= yMin)
    {
        if (yMin <= simulation && simulation <= yMax &&
            (count > xAllAbove || xHits.Contains(count)))
        {
            highest.Add(best);
            break;
        }
        simulation += velocity;
        --velocity;
        best = Math.Max(best, simulation);
        ++count;
    }
}

Console.WriteLine("{0}", highest.Max());
