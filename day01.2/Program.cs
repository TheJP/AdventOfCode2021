var input = File.ReadLines(Environment.GetCommandLineArgs()[1]).Select(int.Parse).ToArray();

var previous = int.MaxValue;
int score = 0;
for (int i = 3; i <= input.Length; ++i) {
    var sum = input[(i-3)..i].Sum();
    if (sum > previous) ++score;
    previous = sum;
}

Console.WriteLine($"{score}");
