var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var zeros = new int[input.First().Length];
var ones = new int[input.First().Length];

foreach (var line in input)
{
    for (int i = 0; i < line.Length; ++i)
    {
        if (line[i] == '0') ++zeros[i];
        if (line[i] == '1') ++ones[i];
    }
}

var gamma = Convert.ToInt32(new string(zeros
    .Zip(ones)
    .Select(pair => pair.First > pair.Second ? '0' : '1')
    .ToArray()
), 2);

var epsilon = Convert.ToInt32(new string(zeros
    .Zip(ones)
    .Select(pair => pair.Second < pair.First ? '1' : '0')
    .ToArray()
), 2);

Console.WriteLine($"{gamma * epsilon}");