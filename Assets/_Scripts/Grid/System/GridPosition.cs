//* This struct was created to make math easier since Vector2 uses x and y coordinates
//* but this game would use x and z coordinates
using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int X;
    public int Z;

    public GridPosition(int x, int z)
    {
        X = x;
        Z = z;
    }

    public override string ToString()
    {
        return $"X: {X} - Z: {Z} ";
    }

    //* Auto-generated
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               X == position.X &&
               Z == position.Z;
    }

    //* Auto-generated
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public static bool operator == (GridPosition a, GridPosition b)
    {
        return (a.X == b.X) && (a.Z == b.Z);
    }

    public static bool operator != (GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X + b.X, a.Z + b.Z);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X - b.X, a.Z - b.Z);
    }
}
