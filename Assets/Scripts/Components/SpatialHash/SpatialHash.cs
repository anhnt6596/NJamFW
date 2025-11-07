using System.Collections.Generic;
using UnityEngine;

public abstract class SpatialHash
{
    public int Count { get; protected set; }
    public abstract void Add<T>(T obj) where T : MonoBehaviour, ISpatialKeyed;
    public abstract void Remove<T>(T obj) where T : MonoBehaviour, ISpatialKeyed;
    public abstract void Move<T>(T obj) where T : MonoBehaviour, ISpatialKeyed;
    public abstract void Query<T>(Vector3 pos, float range, List<T> results) where T : MonoBehaviour, ISpatialKeyed;
    public abstract T GetNearest<T>(Vector3 pos, float range) where T : MonoBehaviour, ISpatialKeyed;
}

public sealed class SpatialHash<T> : SpatialHash
    where T : MonoBehaviour, ISpatialKeyed
{
    readonly float _cell;
    readonly Dictionary<GridKey, List<T>> _buckets = new();

    public SpatialHash(float cellSize) { _cell = Mathf.Max(0.01f, cellSize); }

    static GridKey Key(Vector3 p, float c) => new GridKey(
        (int)Mathf.Floor(p.x / c),
        (int)Mathf.Floor(p.z / c));

    void AddToBucket(T obj, GridKey k)
    {
        if (!_buckets.TryGetValue(k, out var list)) _buckets[k] = list = new List<T>(8);
        list.Add(obj);
    }

    public override void Add<T1>(T1 obj) => Add(obj as T);
    public override void Remove<T1>(T1 obj) => Remove(obj as T);
    public override void Move<T1>(T1 obj) => Move(obj as T);
    public override void Query<T1>(Vector3 pos, float range, List<T1> results) => Query(pos, range, results as List<T>);
    public override T1 GetNearest<T1>(Vector3 pos, float range) => GetNearest(pos, range) as T1;

    void Add(T obj)
    {
        var k = Key(obj.transform.position, _cell);
        AddToBucket(obj, k);
        obj.SpatialKey = k;
        Count++;
    }

    public void Remove(T obj)
    {
        var k = obj.SpatialKey;
        if (_buckets.TryGetValue(k, out var list)) list.Remove(obj);
        obj.SpatialKey = new GridKey(int.MinValue, int.MinValue);
        Count--;
    }

    public void Move(T obj)
    {
        var cur = obj.SpatialKey;
        var k = Key(obj.transform.position, _cell);
        if (cur == k) return;

        if (_buckets.TryGetValue(cur, out var oldList)) oldList.Remove(obj);
        AddToBucket(obj, k);

        obj.SpatialKey = k;
    }

    public void Query(Vector3 pos, float range, List<T> results)
    {
        results.Clear();
        var k = Key(pos, _cell);
        int r = Mathf.CeilToInt(range / _cell);
        float r2 = range * range;

        for (int dz = -r; dz <= r; dz++)
            for (int dx = -r; dx <= r; dx++)
            {
                //if (!CircleIntersectsCell(pos, range, dx, dz)) continue;
                if (!_buckets.TryGetValue(k + (dx, dz), out var list)) continue;
                for (int i = 0; i < list.Count; i++)
                {
                    var e = list[i];
                    var p = e is Component c ? c.transform.position : default;
                    float dxz = p.x - pos.x; float dz2 = p.z - pos.z;
                    if (dxz * dxz + dz2 * dz2 <= r2) results.Add(e);
                }
            }
    }

    T GetNearest(Vector3 pos, float range)
    {
        T result = null;
        float resultDist2 = Mathf.Infinity;
        var k = Key(pos, _cell);
        int r = Mathf.CeilToInt(range / _cell);
        float r2 = range * range;

        for (int dz = -r; dz <= r; dz++)
            for (int dx = -r; dx <= r; dx++)
            {
                if (!_buckets.TryGetValue(k + (dx, dz), out var list)) continue;
                for (int i = 0; i < list.Count; i++)
                {
                    var e = list[i];
                    var p = e is Component c ? c.transform.position : default;
                    float dxz = p.x - pos.x;
                    float dz2 = p.z - pos.z;
                    var dist2 = dxz * dxz + dz2 * dz2;
                    if (dist2 <= r2 && dist2 < resultDist2)
                    {
                        result = e;
                        resultDist2 = dist2;
                    }
                }
            }
        return result;
    }
}