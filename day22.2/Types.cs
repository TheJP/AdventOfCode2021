public record struct Vector(long X, long Y, long Z)
{
    public long this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new IndexOutOfRangeException(),
    };
}

public record struct Cube(Vector From, Vector To, bool On = false)
{
    public bool Contains(Vector v) =>
        From.X <= v.X && v.X <= To.X &&
        From.Y <= v.Y && v.Y <= To.Y &&
        From.Z <= v.Z && v.Z <= To.Z;

    public bool IsCorner(Vector v) =>
        (From.X == v.X || v.X == To.X) &&
        (From.Y == v.Y || v.Y == To.Y) &&
        (From.Z == v.Z || v.Z == To.Z);

    public bool ContainsNotCorner(Vector v) => Contains(v) && !IsCorner(v);

    public Vector[] Corners => new[]
    {
        From,
        new Vector(From.X, From.Y, To.Z),
        new Vector(From.X, To.Y, From.Z),
        new Vector(From.X, To.Y, To.Z),
        new Vector(To.X, From.Y, From.Z),
        new Vector(To.X, From.Y, To.Z),
        new Vector(To.X, To.Y, From.Z),
        To,
    };

    public long Area =>
        (To.X - From.X + 1) *
        (To.Y - From.Y + 1) *
        (To.Z - From.Z + 1);

    public bool HasIntersection(Cube other) =>
        (
            (From.X <= other.From.X && other.From.X <= To.X) ||
            (From.X <= other.To.X && other.To.X <= To.X) ||
            (other.From.X <= From.X && From.X <= other.To.X) ||
            (other.From.X <= To.X && To.X <= other.To.X)
        ) && (
            (From.Y <= other.From.Y && other.From.Y <= To.Y) ||
            (From.Y <= other.To.Y && other.To.Y <= To.Y) ||
            (other.From.Y <= From.Y && From.Y <= other.To.Y) ||
            (other.From.Y <= To.Y && To.Y <= other.To.Y)
        ) && (
            (From.Z <= other.From.Z && other.From.Z <= To.Z) ||
            (From.Z <= other.To.Z && other.To.Z <= To.Z) ||
            (other.From.Z <= From.Z && From.Z <= other.To.Z) ||
            (other.From.Z <= To.Z && To.Z <= other.To.Z)
        );

    public (Cube intersection, IEnumerable<Cube> thisRest, IEnumerable<Cube> otherRest)? Intersect(Cube other)
    {
        if (!HasIntersection(other)) return null;

        var axes = new long[3][];
        for (int i = 0; i < 3; ++i)
        {
            axes[i] = new[] { From[i], To[i], other.From[i], other.To[i] };
            Array.Sort(axes[i]);
        }

        var intersection = new Cube(
            new Vector(axes[0][1], axes[1][1], axes[2][1]),
            new Vector(axes[0][2], axes[1][2], axes[2][2])
        );

        List<Cube> thisRest = new();
        List<Cube> otherRest = new();
        for (int z = 0; z < 3; ++z)
        {
            for (int y = 0; y < 3; ++y)
            {
                for (int x = 0; x < 3; ++x)
                {
                    if (z == 1 && y == 1 && x == 1) continue;
                    var xFrom = axes[0][x] + (x == 2 ? 1 : 0);
                    var yFrom = axes[1][y] + (y == 2 ? 1 : 0);
                    var zFrom = axes[2][z] + (z == 2 ? 1 : 0);
                    var xTo = axes[0][x + 1] + (x == 0 ? -1 : 0);
                    var yTo = axes[1][y + 1] + (y == 0 ? -1 : 0);
                    var zTo = axes[2][z + 1] + (z == 0 ? -1 : 0);
                    if (xFrom > xTo || yFrom > yTo || zFrom > zTo) continue;
                    var newCube = new Cube(
                        new Vector(xFrom, yFrom, zFrom),
                        new Vector(xTo, yTo, zTo),
                        On
                    );
                    if (HasIntersection(newCube)) thisRest.Add(newCube);
                    else if (other.HasIntersection(newCube))
                    {
                        if (On != other.On) newCube = new Cube(newCube.From, newCube.To, other.On);
                        otherRest.Add(newCube);
                    }
                }
            }
        }

        return (intersection, thisRest, otherRest);
    }

    public override string ToString() => $"({From.X}, {From.Y}, {From.Z})-({To.X}, {To.Y}, {To.Z})";
}
