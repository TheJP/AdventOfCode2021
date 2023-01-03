var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var map = new Dictionary<char, (char, int)>()
{
    { '(', (')', 3) },
    { '[', (']', 57) },
    { '{', ('}', 1197) },
    { '<', ('>', 25137) },
};
var scoreTable = map.ToDictionary(pair => pair.Value.Item1, pair => pair.Value.Item2);

int score = 0;
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

            score += scoreTable[c];
            break;
        }
    }
}

Console.WriteLine(score);
