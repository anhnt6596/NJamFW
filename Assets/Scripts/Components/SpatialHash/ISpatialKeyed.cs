using System;
using UnityEngine;

public interface ISpatialKeyed
{
    GridKey SpatialKey { get; set; }
    SpatialHash SpatialHash { get; set; }
}

public struct GridKey : IEquatable<GridKey>
{
    public GridKey(int x, int z) { this.x = x; this.z = z; }
    public int x, z;

    public static bool operator ==(GridKey a, GridKey b) => a.x == b.x && a.z == b.z;

    public static bool operator !=(GridKey a, GridKey b) => !(a == b);

    public static GridKey operator +(GridKey a, GridKey b) =>
        new GridKey(a.x + b.x, a.z + b.z);

    public static GridKey operator -(GridKey a, GridKey b) =>
        new GridKey(a.x - b.x, a.z - b.z);

    public static GridKey operator +(GridKey a, (int dx, int dz) o) =>
        new GridKey(a.x + o.dx, a.z + o.dz);

    public static GridKey operator -(GridKey a, (int dx, int dz) o) =>
        new GridKey(a.x - o.dx, a.z - o.dz);

    public bool Equals(GridKey other) => x == other.x && z == other.z;

    public override bool Equals(object obj) => obj is GridKey other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (x * 397) ^ z;
        }
    }

    public override string ToString() => $"({x}, {z})";
}