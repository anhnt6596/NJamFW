using System.Collections.Generic;
using UnityEngine;

public static class PolygonHelper
{
    public static bool PointInPoly(Vector2 p, List<Vector2> poly)
    {
        // Ray-casting (odd-even)
        bool inside = false;
        int n = poly.Count;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            Vector2 a = poly[j], b = poly[i];
            bool intersect = ((a.y > p.y) != (b.y > p.y)) &&
                             (p.x < (b.x - a.x) * (p.y - a.y) / Mathf.Max(1e-6f, (b.y - a.y)) + a.x);
            if (intersect) inside = !inside;
        }
        return inside;
    }

    static bool SegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
        // ccw test
        float o1 = Cross(q1 - p1, p2 - p1);
        float o2 = Cross(q2 - p1, p2 - p1);
        float o3 = Cross(p1 - q1, q2 - q1);
        float o4 = Cross(p2 - q1, q2 - q1);

        if ((o1 == 0 && OnSegment(p1, q1, p2)) ||
            (o2 == 0 && OnSegment(p1, q2, p2)) ||
            (o3 == 0 && OnSegment(q1, p1, q2)) ||
            (o4 == 0 && OnSegment(q1, p2, q2)))
            return true;

        return (o1 > 0) != (o2 > 0) && (o3 > 0) != (o4 > 0);
    }

    static float Cross(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

    static bool OnSegment(Vector2 a, Vector2 b, Vector2 c)
    {
        return Mathf.Min(a.x, c.x) - 1e-6f <= b.x && b.x <= Mathf.Max(a.x, c.x) + 1e-6f &&
               Mathf.Min(a.y, c.y) - 1e-6f <= b.y && b.y <= Mathf.Max(a.y, c.y) + 1e-6f;
    }

    static bool RectContainsPoint(Vector2 min, Vector2 max, Vector2 p)
    {
        return p.x >= min.x && p.x <= max.x && p.y >= min.y && p.y <= max.y;
    }

    public static bool RectIntersectsPoly(Vector2 rMin, Vector2 rMax, List<Vector2> poly)
    {
        // 1) Bất kỳ góc ô nằm trong polygon?
        Vector2[] corners = new Vector2[] {
        new Vector2(rMin.x, rMin.y),
        new Vector2(rMax.x, rMin.y),
        new Vector2(rMax.x, rMax.y),
        new Vector2(rMin.x, rMax.y),
    };
        foreach (var c in corners)
            if (PointInPoly(c, poly)) return true;

        // 2) Bất kỳ đỉnh polygon nằm trong ô?
        foreach (var v in poly)
            if (RectContainsPoint(rMin, rMax, v)) return true;

        // 3) Bất kỳ cạnh polygon cắt cạnh ô?
        Vector2[] re = new Vector2[] {
        corners[0], corners[1],
        corners[1], corners[2],
        corners[2], corners[3],
        corners[3], corners[0],
    };
        for (int i = 0; i < poly.Count; i++)
        {
            Vector2 a = poly[i];
            Vector2 b = poly[(i + 1) % poly.Count];
            for (int e = 0; e < 8; e += 2)
                if (SegmentsIntersect(a, b, re[e], re[e + 1])) return true;
        }

        // Không giao
        return false;
    }

    // Bao gồm cả điểm nằm đúng trên cạnh biên polygon (tránh rỗ mép/“sọc”)
    public static bool PointInPolyInclusive(Vector2 p, List<Vector2> poly)
    {
        // Nếu p nằm trên bất kỳ cạnh nào → coi là "inside"
        for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
        {
            Vector2 a = poly[j], b = poly[i];
            if (PointOnSegment(p, a, b)) return true;
        }
        // Odd-even như cũ
        return PointInPoly(p, poly);
    }

    static bool PointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        // khoảng cách vuông góc nhỏ + nằm giữa a..b
        const float EPS = 1e-5f;
        Vector2 ab = b - a, ap = p - a;
        float ab2 = Vector2.Dot(ab, ab);
        if (ab2 < EPS) return (p - a).sqrMagnitude < EPS;
        float t = Mathf.Clamp01(Vector2.Dot(ap, ab) / ab2);
        Vector2 proj = a + t * ab;
        return (proj - p).sqrMagnitude <= (EPS * EPS);
    }

    public static List<List<Vector3>> ToPolygonList(List<IPolygon> polygons)
    {
        var result = new List<List<Vector3>>();
        if (polygons == null) return result;

        foreach (var p in polygons)
        {
            if (p == null) continue;
            var poly = p.GetPolygon();
            if (poly == null || poly.Count < 3) continue;
            result.Add(new List<Vector3>(poly));
        }
        return result;
    }

}