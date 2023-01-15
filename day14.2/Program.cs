// Task 1
// const int Iterations = 10;
// Task 2
const int Iterations = 40;

using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
var template = input.ReadLine() ?? throw new InvalidOperationException();
input.ReadLine();

var map = new Dictionary<string, char>();

string? line;
while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) return;
    if (line.Length != 7) throw new InvalidOperationException();

    map.Add(line[..2], line[6]);
}

var memoization = new Dictionary<string, Dictionary<char, long>>[Iterations];
for (int i = 0; i < Iterations; ++i)
{
    memoization[i] = new();
}

Dictionary<char, long> Combine(Dictionary<char, long> bucket1, Dictionary<char, long> bucket2)
{
    var result = new Dictionary<char, long>(bucket1);
    foreach (var entry in bucket2)
    {
        if (result.ContainsKey(entry.Key)) result[entry.Key] += entry.Value;
        else result.Add(entry.Key, entry.Value);
    }
    return result;
}

Dictionary<char, long> Solve(string input, int iteration)
{
    if (memoization[iteration].ContainsKey(input)) return memoization[iteration][input];

    var next = map[input];
    Dictionary<char, long> result;
    if (iteration + 1 >= Iterations) {
        if (input[0] == next) result = new() { { next, 2 } };
        else result = new()
        {
            { input[0], 1 },
            { next, 1 },
        };
    } else {
        var bucket1 = Solve($"{input[0]}{next}", iteration + 1);
        var bucket2 = Solve($"{next}{input[1]}", iteration + 1);
        result = Combine(bucket1, bucket2);
    }

    memoization[iteration].Add(input, result);
    return result;
}

var combined = new Dictionary<char, long>();
for (int i = 1; i < template.Length; ++i)
{
    var step = Solve(template[(i - 1)..(i + 1)], 0);
    combined = Combine(combined, step);
}

if (combined.ContainsKey(template[^1])) {
    ++combined[template[^1]];
} else {
    combined.Add(template[^1], 1);
}

Console.WriteLine("{0}", combined.Values.Max() - combined.Values.Min());
