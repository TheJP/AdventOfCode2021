var numbers = File.ReadLines(Environment.GetCommandLineArgs()[1])
    .First().Split(',').Select(int.Parse).ToArray();

int best = int.MaxValue;
int max = numbers.Max();

for (int i = numbers.Min(); i <= max; ++i)
{
    int fuel = numbers.Select(x => { var n = Math.Abs(i - x); return (n * (n + 1)) / 2; }).Sum();
    best = Math.Min(best, fuel);
}

Console.WriteLine(best);
