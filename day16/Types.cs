abstract class Package
{
    public int Version { get; }
    public int Type { get; }
    public int Size { get; }

    public Package(int version, int type, int size)
    {
        Version = version;
        Type = type;
        Size = size;
    }

    public abstract IList<Package> Flatten();
}

class Literal : Package
{
    public long Value { get; }

    public Literal(int version, int type, int size, long value) : base(version, type, size)
    {
        Value = value;
    }

    public override IList<Package> Flatten()
    {
        return new List<Package>() { this };
    }
}

class Operator : Package
{
    public List<Package> Packages { get; }

    public Operator(int version, int type, int size, List<Package> packages) : base(version, type, size)
    {
        Packages = packages;
    }

    public override IList<Package> Flatten()
    {
        var result = new List<Package>();
        result.Add(this);
        foreach (var package in Packages)
        {
            result.AddRange(package.Flatten());
        }
        return result;
    }
}
