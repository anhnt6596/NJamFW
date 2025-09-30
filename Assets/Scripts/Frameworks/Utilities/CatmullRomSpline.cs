using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CatmullRomSpline : MonoBehaviour
{
    [Header("control points (min 4)")]
    public Transform[] controlPoints;

    [Header("Số đoạn nhỏ để vẽ mượt hơn")]
    public int segmentsPerCurve = 20;

    [Header("Gizmo Color")]
    public Color curveColor = Color.green;
    public Color pointColor = Color.red;

    private List<float> arcLengths = new List<float>();
    private float totalLength = 0f;

    private void OnDrawGizmos()
    {
        if (controlPoints == null || controlPoints.Length < 4) return;

        Gizmos.color = pointColor;
        foreach (var p in controlPoints)
        {
            if (p != null) Gizmos.DrawSphere(p.position, 0.1f);
        }

        Gizmos.color = curveColor;
        PrecomputeLengths();

        Vector3 prev = GetPoint(0f);
        int steps = segmentsPerCurve * (controlPoints.Length - 3);
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 pos = GetPoint(t);
            Gizmos.DrawLine(prev, pos);
            prev = pos;
        }
    }

    private Vector3 GetCatmullRomPos(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    public Vector3 GetPoint(float t)
    {
        if (controlPoints.Length < 4) return Vector3.zero;

        int numSections = controlPoints.Length - 3;
        t = Mathf.Clamp01(t) * numSections;

        int currPt = Mathf.Min(Mathf.FloorToInt(t), numSections - 1);
        float u = t - currPt;

        return GetCatmullRomPos(
            controlPoints[currPt].position,
            controlPoints[currPt + 1].position,
            controlPoints[currPt + 2].position,
            controlPoints[currPt + 3].position,
            u
        );
    }

    private void PrecomputeLengths()
    {
        arcLengths.Clear();
        totalLength = 0f;

        Vector3 prev = GetPoint(0f);
        int steps = segmentsPerCurve * (controlPoints.Length - 3);

        arcLengths.Add(0f);
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 pos = GetPoint(t);
            totalLength += Vector3.Distance(prev, pos);
            arcLengths.Add(totalLength);
            prev = pos;
        }
    }

    public Vector3 GetPointByDistance(float d)
    {
        if (arcLengths.Count == 0) PrecomputeLengths();
        if (totalLength <= 0f) return GetPoint(0f);

        d = Mathf.Clamp(d, 0f, totalLength);

        int low = 0, high = arcLengths.Count - 1;
        while (low < high)
        {
            int mid = (low + high) / 2;
            if (arcLengths[mid] < d) low = mid + 1;
            else high = mid;
        }

        int i1 = Mathf.Max(low - 1, 0);
        int i2 = Mathf.Min(low, arcLengths.Count - 1);

        float len1 = arcLengths[i1];
        float len2 = arcLengths[i2];

        float t = (i1 + (d - len1) / (len2 - len1)) / (arcLengths.Count - 1);

        return GetPoint(t);
    }

    public float GetTotalLength() => totalLength;
}
