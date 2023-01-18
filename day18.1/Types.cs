interface Snail
{
    long Magnitude { get; }
    Snail? Parent { get; set; }
}

class SnailValue : Snail
{
    public long Value { get; set; }
    public Snail? Parent { get; set; }

    public long Magnitude => Value;

    public override string ToString() => $"{Value}";
}

class SnailPair : Snail
{
    public Snail? Parent { get; set; }
    public Snail Left { get; set; }
    public Snail Right { get; set; }

    public SnailPair(Snail left, Snail right)
    {
        Left = left;
        Right = right;
    }

    public long Magnitude => 3 * Left.Magnitude + 2 * Right.Magnitude;

    public override string ToString() => $"[{Left},{Right}]";
}