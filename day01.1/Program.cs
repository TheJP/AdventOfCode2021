var input = File.ReadLines(Environment.GetCommandLineArgs()[1]).Select(int.Parse);

var previous = input.First();
int score = 0;
foreach (var number in input.Skip(1))
{
    if (number > previous) ++score;
    previous = number;
}

Console.WriteLine($"{score}");
