var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var map = new Dictionary<char, (char, int)>()
{
    { '(', (')', 1) },
    { '[', (']', 2) },
    { '{', ('}', 3) },
    { '<', ('>', 4) },
};

var scores = new List<long>();
foreach (var line in input)
{
    var stack = new Stack<char>();
    foreach (var c in line)
    {
        if (map.ContainsKey(c)) stack.Push(c);
        else
        {
            var top = stack.Pop();
            if (c == map[top].Item1) continue;

            stack.Clear();
            break;
        }
    }

    if (stack.Count == 0) continue;

    long score = 0;
    while (stack.Count > 0)
    {
        var top = stack.Pop();
        score *= 5;
        score += map[top].Item2;
    }
    scores.Add(score);
}

scores.Sort();
Console.WriteLine(scores[scores.Count / 2]);
