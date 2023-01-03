var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var digits = new Dictionary<string, int>()
{
    {"abcefg", 0},
    {"cf", 1},
    {"acdeg", 2},
    {"acdfg", 3},
    {"bcdf", 4},
    {"abdfg", 5},
    {"abdefg", 6},
    {"acf", 7},
    {"abcdefg", 8},
    {"abcdfg", 9},
};

var digitSegments = digits.ToDictionary(kv => kv.Value, kv => kv.Key);

void SortInternal(string[] a)
{
    for (int i = 0; i < a.Length; ++i)
    {
        var chars = a[i].ToCharArray();
        Array.Sort(chars);
        a[i] = new string(chars);
    }
}

int score = 0;
foreach (var line in input)
{
    var parts = line.Split('|');
    var output = parts[1].Trim().Split();
    var segments = parts[0].Trim().Split().Concat(output).ToArray();
    SortInternal(output);
    SortInternal(segments);

    var assignments = new HashSet<(char, char)>();
    for (char a = 'a'; a <= 'g'; ++a)
    {
        for (char b = 'a'; b <= 'g'; ++b) assignments.Add((a, b));
    }

    foreach (var segment in segments)
    {
        var originals = digits.Keys.Where(s => s.Length == segment.Length);
        string original = string.Join(' ', originals);
        // Console.WriteLine($"{segment} <- {original}");

        for (char o = 'a'; o <= 'g'; ++o)
        {
            if (!original.Contains(o))
            {
                foreach (var s in segment) assignments.Remove((s, o));
            }
            else if (originals.All(orig => orig.Contains(o)))
            {
                for (char s = 'a'; s <= 'g'; ++s)
                {
                    if (!segment.Contains(s)) assignments.Remove((s, o));
                }
            }
        }
    }

    var changed = true;
    while (changed)
    {
        changed = false;
        for (char a = 'a'; a <= 'g'; ++a)
        {
            var fromA = assignments.Where(pair => pair.Item1 == a);
            if (fromA.Count() == 1)
            {
                var charFromA = fromA.First().Item2;
                for (char b = 'a'; b <= 'g'; ++b)
                {
                    if (b != a) changed |= assignments.Remove((b, charFromA));
                }
            }
            var toA = assignments.Where(pair => pair.Item2 == a);
            if (toA.Count() == 1)
            {
                var charToA = toA.First().Item1;
                for (char b = 'a'; b <= 'g'; ++b)
                {
                    if (b != a) changed |= assignments.Remove((charToA, b));
                }
            }
        }
    }

    // Console.WriteLine(assignments.Count);
    // foreach (var pair in assignments) { Console.Write($"{pair} "); }
    // Console.WriteLine();
    if (assignments.Count != 7) throw new InvalidOperationException();

    var converter = assignments.ToDictionary(pair => pair.Item1, pair => pair.Item2);
    int number = 0;
    foreach (var segment in output)
    {
        var converted = segment.Select(c => converter[c]).ToArray();
        Array.Sort(converted);
        number *= 10;
        number += digits[new string(converted)];
    }
    score += number;
}

Console.WriteLine(score);
