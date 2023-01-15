// Task 1
const int Iterations = 11;
// Task 2
// const int Iterations = 41;

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

var start = new int[Iterations];
var end = new int[Iterations];
var stages = new char[Iterations][];

start[0] = 0;
end[0] = template.Length;
stages[0] = template.ToCharArray();

for (int i = 1; i < Iterations; ++i)
{
    stages[i] = new char[4];
    stages[i][0] = stages[0][0];
    end[i] = 1;
}

var buckets = new Dictionary<char, int>();
for (char c = 'A'; c <= 'Z'; ++c)
{
    buckets.Add(c, 0);
}
++buckets[stages[0][0]];

// var debug = Enumerable.Range(0, Iterations).Select(_ => $"{stages[0][0]}").ToArray();
// debug[0] = template;

var empty = false;
while (!empty)
{
    empty = true;
    for (int i = 0; i < Iterations - 1; ++i)
    {
        if (i < Iterations - 2 && end[i + 1] - start[i + 1] >= 2)
        {
            empty = false;
            continue;
        }

        if (end[i] - start[i] < 2)
        {
            continue;
        }

        var index1 = start[i] % stages[i].Length;
        var index2 = (index1 + 1) % stages[i].Length;
        var char1 = stages[i][index1];
        var char2 = stages[i][index2];
        var next = map[$"{char1}{char2}"];
        ++start[i];

        // debug[i + 1] += $"{next}{char2}";
        if (i == Iterations - 2)
        {
            ++buckets[next];
            ++buckets[char2];
        }
        else
        {
            stages[i + 1][end[i + 1] % stages[i + 1].Length] = next;
            ++end[i + 1];
            stages[i + 1][end[i + 1] % stages[i + 1].Length] = char2;
            ++end[i + 1];
            empty = false;
        }
    }
}

// for (int i = 0; i < Iterations; ++i)
// {
//     Console.WriteLine("{0}", debug[i]);
// }

Console.WriteLine("{0}", buckets.Values.Max() - buckets.Values.Where(c => c > 0).Min());
