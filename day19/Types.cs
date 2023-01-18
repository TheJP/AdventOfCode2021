public record struct Vector(int X, int Y, int Z)
{
    public static Vector Zeros { get; } = new Vector(0, 0, 0);
    public static Vector Ones { get; } = new Vector(1, 1, 1);
    public static Vector operator +(Vector v, int s) => new Vector(v.X + s, v.Y + s, v.Z + s);
    public static Vector operator +(int s, Vector v) => new Vector(v.X + s, v.Y + s, v.Z + s);
    public static Vector operator -(Vector v) => new Vector(-v.X, -v.Y, -v.Z);
    public static Vector operator -(Vector v, int s) => new Vector(v.X - s, v.Y - s, v.Z - s);
    public static Vector operator -(int s, Vector v) => new Vector(s - v.X, s - v.Y, s - v.Z);
    public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vector operator *(Vector v, int s) => new Vector(v.X * s, v.Y * s, v.Z * s);
    public static Vector operator *(int s, Vector v) => new Vector(v.X * s, v.Y * s, v.Z * s);
}

public struct Matrix
{
    private readonly int[] weights = new int[12];

    public Matrix() { }

    public Matrix Copy()
    {
        var result = new Matrix();
        Array.Copy(weights, result.weights, weights.Length);
        return result;
    }

    public static Matrix Identity()
    {
        var result = new Matrix();
        result.weights[0] = 1;
        result.weights[5] = 1;
        result.weights[10] = 1;
        return result;
    }

    public static Vector operator *(Matrix m, Vector v) => new Vector(
        v.X * m.weights[0] + v.Y * m.weights[1] + v.Z * m.weights[2] + m.weights[3],
        v.X * m.weights[4] + v.Y * m.weights[5] + v.Z * m.weights[6] + m.weights[7],
        v.X * m.weights[8] + v.Y * m.weights[9] + v.Z * m.weights[10] + m.weights[11]
    );

    public Matrix Translate(Vector t) => Translate(t.X, t.Y, t.Z);

    public Matrix Translate(int x, int y, int z)
    {
        var result = Copy();
        result.weights[3] += x;
        result.weights[7] += y;
        result.weights[11] += z;
        return result;
    }

    //  1  0  0  0
    //  0  0 -1  0
    //  0  1  0  0
    //  0  0  0  1
    public Matrix RotateX()
    {
        var result = Copy();
        // row 0 and 3 are unchanged
        // row 1 and 2 swap
        // row 1 is negated (this is old row 2)
        result.weights[4] = -weights[8];
        result.weights[5] = -weights[9];
        result.weights[6] = -weights[10];
        result.weights[7] = -weights[11];
        result.weights[8] = weights[4];
        result.weights[9] = weights[5];
        result.weights[10] = weights[6];
        result.weights[11] = weights[7];
        return result;
    }

    //  0  0  1  0
    //  0  1  0  0
    // -1  0  0  0
    //  0  0  0  1
    public Matrix RotateY()
    {
        var result = Copy();
        // row 1 and 3 are unchanged
        // row 0 and 2 swap
        // row 2 is negated (this is old row 0)
        result.weights[0] = weights[8];
        result.weights[1] = weights[9];
        result.weights[2] = weights[10];
        result.weights[3] = weights[11];
        result.weights[8] = -weights[0];
        result.weights[9] = -weights[1];
        result.weights[10] = -weights[2];
        result.weights[11] = -weights[3];
        return result;
    }

    //  0 -1  0  0
    //  1  0  0  0
    //  0  0  1  0
    //  0  0  0  1
    public Matrix RotateZ()
    {
        var result = Copy();
        // row 2 and 3 are unchanged
        // row 0 and 1 swap
        // row 0 is negated (this is old row 1)
        result.weights[0] = -weights[4];
        result.weights[1] = -weights[5];
        result.weights[2] = -weights[6];
        result.weights[3] = -weights[7];
        result.weights[4] = weights[0];
        result.weights[5] = weights[1];
        result.weights[6] = weights[2];
        result.weights[7] = weights[3];
        return result;
    }

    public void Print()
    {
        for (int y = 0; y < 3; ++y)
        {
            for (int x = 0; x < 4; ++x)
            {
                Console.Write($"{weights[y * 4 + x]}\t");
            }
            Console.WriteLine();
        }
        Console.WriteLine("0\t0\t0\t1");
        Console.WriteLine();
    }
}