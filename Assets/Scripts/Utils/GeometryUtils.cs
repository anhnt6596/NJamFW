using System.Collections.Generic;
using UnityEngine;

public static class GeometryUtils
{
    public static bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        int count = polygon.Count;
        bool inside = false;

        for (int i = 0, j = count - 1; i < count; j = i++)
        {
            Vector2 pi = polygon[i];
            Vector2 pj = polygon[j];

            bool intersect = ((pi.y > point.y) != (pj.y > point.y)) &&
                             (point.x < (pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y + Mathf.Epsilon) + pi.x);

            if (intersect)
                inside = !inside;
        }

        return inside;
    }
}
