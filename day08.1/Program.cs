var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var uniqueLenghts = new HashSet<int>() { 2, 4, 3, 7 };

int score = 0;
foreach (var line in input)
{
    var parts = line.Split('|');
    var segments = parts[1].Split();
    score += segments.Count(s => uniqueLenghts.Contains(s.Length));
}

Console.WriteLine(score);
