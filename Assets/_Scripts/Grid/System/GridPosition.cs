//* This struct was created to make math easier since Vector2 uses x and y coordinates
//* but this game would use x and z coordinates
using System;
using UnityEngine.Serialization;

[System.Serializable]
public struct GridPosition : IEquatable<GridPosition>
{
    [FormerlySerializedAs("X")] public int x;
    [FormerlySerializedAs("Z")] public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return $"X: {x} - Z: {z} ";
    }

    //* Auto-generated
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    //* Auto-generated
    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public static bool operator == (GridPosition a, GridPosition b)
    {
        return (a.x == b.x) && (a.z == b.z);
    }

    public static bool operator != (GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}
