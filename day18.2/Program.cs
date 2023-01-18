var input = File.ReadAllLines(Environment.GetCommandLineArgs()[^1]);

Snail Parse(string snail)
{
    if (snail[0] != '[' || snail[^1] != ']')
    {
        return new SnailValue { Value = long.Parse(snail) };
    }

    int brackets = 0;
    for (int i = 1; i < snail.Length - 1; ++i)
    {
        if (snail[i] == '[') ++brackets;
        if (snail[i] == ']') --brackets;
        if (brackets == 0 && snail[i] == ',')
        {
            return new SnailPair(
                Parse(snail[1..i]),
                Parse(snail[(i + 1)..^1]));
        }
    }

    throw new InvalidOperationException();
}

void PopulateParents(Snail snail, Snail? parent)
{
    snail.Parent = parent;
    if (snail is SnailPair pair)
    {
        PopulateParents(pair.Left, pair);
        PopulateParents(pair.Right, pair);
    }
}

void AddValues(Snail root, Snail reference, long left, long right)
{
    var ordered = new List<SnailValue>();
    var dfs = new Stack<Snail>();
    dfs.Push(root);

    while (dfs.Count > 0)
    {
        var current = dfs.Pop();
        switch (current)
        {
            case SnailValue value:
                ordered.Add(value);
                break;
            case SnailPair pair:
                dfs.Push(pair.Right);
                dfs.Push(pair.Left);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    for (int i = 0; i < ordered.Count; ++i)
    {
        if (ordered[i] != reference) continue;
        if (i > 0) ordered[i - 1].Value += left;
        if (i + 1 < ordered.Count) ordered[i + 1].Value += right;
        return;
    }
}

Snail Reduce(Snail snail)
{
    var dfs = new Stack<(Snail snail, int depth)>();

    // Explode
    dfs.Push((snail, 0));
    while (dfs.Count > 0)
    {
        var (current, depth) = dfs.Pop();
        if (current is not SnailPair pair) continue;

        dfs.Push((pair.Right, depth + 1));
        dfs.Push((pair.Left, depth + 1));

        if (depth < 4) continue;

        var left = pair.Left as SnailValue ?? throw new InvalidOperationException();
        var right = pair.Right as SnailValue ?? throw new InvalidOperationException();
        var parent = pair.Parent as SnailPair ?? throw new InvalidOperationException();
        if (parent.Left == pair)
        {
            parent.Left = new SnailValue() { Value = 0, Parent = parent };
            AddValues(snail, parent.Left, left.Value, right.Value);
        }
        else
        {
            if (parent.Right != pair) throw new InvalidOperationException();
            parent.Right = new SnailValue() { Value = 0, Parent = parent };
            AddValues(snail, parent.Right, left.Value, right.Value);
        }

        // Console.WriteLine("{0} (explode)", snail);
        return Reduce(snail);
    }

    // Split
    dfs.Push((snail, 0));
    while (dfs.Count > 0)
    {
        var (current, depth) = dfs.Pop();
        if (current is SnailPair pair)
        {
            dfs.Push((pair.Right, depth + 1));
            dfs.Push((pair.Left, depth + 1));
            continue;
        }

        if (current is not SnailValue value) throw new InvalidOperationException();

        if (value.Value < 10) continue;

        var parent = value.Parent as SnailPair ?? throw new InvalidOperationException();
        var newPair = new SnailPair(
            new SnailValue() { Value = value.Value / 2 },
            new SnailValue() { Value = value.Value / 2 + value.Value % 2 }
        );
        newPair.Left.Parent = newPair;
        newPair.Right.Parent = newPair;

        if (parent.Left == value) parent.Left = newPair;
        else parent.Right = newPair;
        newPair.Parent = parent;

        // Console.WriteLine("{0} (split)", snail);
        return Reduce(snail);
    }

    return snail;
}

long max = 0;
for (int i = 0; i < input.Length; ++i)
{
    for (int j = 0; j < input.Length; ++j)
    {
        if (i == j) continue;

        var snail = new SnailPair(Parse(input[i]), Parse(input[j]));
        PopulateParents(snail, null);
        max = Math.Max(max, Reduce(snail).Magnitude);
    }
}

Console.WriteLine("{0}", max);
