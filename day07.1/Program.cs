var numbers = File.ReadLines(Environment.GetCommandLineArgs()[1])
    .First().Split(',').Select(int.Parse).ToArray();

// Median is the solution
Array.Sort(numbers);
int half = numbers.Length / 2;
int best = numbers.Length % 2 == 1 ? numbers[half] : ((numbers[half] + numbers[half]) / 2);

int fuel = numbers.Select(x => Math.Abs(best - x)).Sum();
// Console.WriteLine(best);
Console.WriteLine(fuel);
