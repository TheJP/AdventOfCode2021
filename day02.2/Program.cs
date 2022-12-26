var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

int x = 0;
int depth = 0;
int aim = 0;

foreach (var line in input)
{
    var parts = line.Split();
    var amount = int.Parse(parts[1]);
    switch (parts[0])
    {
        case "forward":
            x += amount;
            depth += aim * amount;
            break;
        case "up":
            aim -= amount;
            break;
        case "down":
            aim += amount;
            break;
        default:
            throw new InvalidOperationException();
    }
}

Console.WriteLine($"{depth * x}");
