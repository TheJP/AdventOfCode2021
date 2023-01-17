var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

Package ParseLiteral(int version, int type, ref string body)
{
    long result = 0;
    int size = 6;

    while (true)
    {
        bool last = body[0] == '0';

        result |= Convert.ToInt64(body[1..5], 2);

        body = body[5..];
        Console.Write($"^{new string('-', 4)}");
        size += 5;

        if (last)
        {
            break;
        }
        else
        {
            result <<= 4;
        }
    }

    return new Literal(version, type, size, result);
}

Package ParseOperator(int version, int type, ref string body)
{
    bool isLengthTypeZero = body[0] == '0';
    int lengthBits = isLengthTypeZero ? 15 : 11;
    int length = Convert.ToInt32(body[1..(lengthBits + 1)], 2);
    body = body[(lengthBits + 1)..];
    Console.Write($"l{new string('L', lengthBits)}");

    var packages = new List<Package>();
    int subSize = 0;
    if (isLengthTypeZero)
    {
        while (subSize < length)
        {
            var package = Parse(ref body);
            subSize += package.Size;
            packages.Add(package);
        }

        if (subSize != length) throw new InvalidOperationException();
    }
    else
    {
        for (int i = 0; i < length; ++i)
        {
            packages.Add(Parse(ref body));
        }
        subSize = packages.Sum(p => p.Size);
    }

    return new Operator(version, type, 6 + 1 + lengthBits + subSize, packages);
}

Package Parse(ref string binary)
{
    var version = Convert.ToInt32(binary[..3], 2);
    var type = Convert.ToInt32(binary[3..6], 2);
    binary = binary[6..];
    Console.Write("VVVTTT");
    return type switch
    {
        4 => ParseLiteral(version, type, ref binary),
        _ => ParseOperator(version, type, ref binary),
    };
}

long Evaluate(Package package)
{
    switch (package)
    {
        case Literal literal:
            return literal.Value;
        case Operator o:
            var values = o.Packages.Select(Evaluate);
            return o.Type switch
            {
                0 => values.Sum(),
                1 => values.Aggregate(1L, (x, y) => x * y),
                2 => values.Min(),
                3 => values.Max(),
                5 => values.First() > values.Last() ? 1 : 0,
                6 => values.First() < values.Last() ? 1 : 0,
                7 => values.First() == values.Last() ? 1 : 0,
                _ => throw new InvalidOperationException(),
            };
        default:
            throw new InvalidOperationException();
    }
}

foreach (var line in input)
{
    var binary = line
        .Select(c => Convert.ToString(Convert.ToInt32($"{c}", 16), 2).PadLeft(4, '0'))
        .Aggregate("", (a, b) => $"{a}{b}");
    Console.WriteLine(binary);
    var package = Parse(ref binary);
    Console.WriteLine();
    Console.WriteLine("Task 1: {0}", package.Flatten().Sum(p => p.Version));
    Console.WriteLine("Task 2: {0}", Evaluate(package));
}
