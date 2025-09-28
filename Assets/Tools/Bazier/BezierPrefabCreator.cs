#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

public class BezierPrefabCreator : MonoBehaviour
{
    [Header("Control Points (2, 3 ho?c 4)")]
    public Transform[] controlPoints;

    [Header("Prefab ?? sinh ra (có th? ?? tr?ng)")]
    public GameObject pointPrefab;

    [Header("Kho?ng cách gi?a các ?i?m trên ???ng cong")]
    public float spacing = 1f;

    [Header("?? m?n khi l?y m?u (càng cao ???ng càng chính xác)")]
    public int resolution = 100;

    private List<Vector3> sampledPoints = new List<Vector3>();

    [ContextMenu("Generate And Save Prefab")]
    public void GenerateAndSave()
    {
        if (controlPoints == null || controlPoints.Length < 2)
        {
            Debug.LogError("? Thi?u controlPoints!");
            return;
        }

        // T?o root object cho curve
        GameObject root = new GameObject("BezierCurve");

        // Thêm script l?u danh sách points
        BezierCurveData curveData = root.AddComponent<BezierCurveData>();
        curveData.points = new List<Transform>();

        // L?y m?u ???ng cong
        SampleCurve();

        // Tính t?ng ?? dài
        float curveLength = GetCurveLength(sampledPoints);

        // Sinh các point cách ??u
        float dist = 0f;
        int index = 0;

        while (dist <= curveLength)
        {
            Vector3 pos = GetPointAtDistance(sampledPoints, dist);

            GameObject obj;
            if (pointPrefab != null)
            {
#if UNITY_EDITOR
                obj = PrefabUtility.InstantiatePrefab(pointPrefab) as GameObject;
#else
                obj = Instantiate(pointPrefab);
#endif
            }
            else
            {
                obj = new GameObject();
            }

            obj.transform.SetParent(root.transform);
            obj.transform.position = pos;
            obj.name = "Point_" + index;

            curveData.points.Add(obj.transform);

            index++;
            dist += spacing;
        }

#if UNITY_EDITOR
        // L?u thành prefab trong Assets
        string path = "Assets/BezierCurve.prefab";
        PrefabUtility.SaveAsPrefabAsset(root, path);
        Debug.Log("? Saved curve as prefab: " + path);

        // Xóa root t?m trong scene
        DestroyImmediate(root);
#endif
    }

    // ---- Bezier logic ----
    private Vector3 GetBezierPoint(float t)
    {
        if (controlPoints.Length == 2) // line
        {
            return Vector3.Lerp(controlPoints[0].position, controlPoints[1].position, t);
        }
        else if (controlPoints.Length == 3) // quadratic
        {
            return Mathf.Pow(1 - t, 2) * controlPoints[0].position +
                   2 * (1 - t) * t * controlPoints[1].position +
                   Mathf.Pow(t, 2) * controlPoints[2].position;
        }
        else if (controlPoints.Length == 4) // cubic
        {
            return Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                   3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                   3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                   Mathf.Pow(t, 3) * controlPoints[3].position;
        }
        return Vector3.zero;
    }

    private void SampleCurve()
    {
        sampledPoints.Clear();
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            sampledPoints.Add(GetBezierPoint(t));
        }
    }

    private float GetCurveLength(List<Vector3> pts)
    {
        float length = 0f;
        for (int i = 1; i < pts.Count; i++)
        {
            length += Vector3.Distance(pts[i - 1], pts[i]);
        }
        return length;
    }

    private Vector3 GetPointAtDistance(List<Vector3> pts, float targetDist)
    {
        float dist = 0f;
        for (int i = 1; i < pts.Count; i++)
        {
            float seg = Vector3.Distance(pts[i - 1], pts[i]);
            if (dist + seg >= targetDist)
            {
                float t = (targetDist - dist) / seg;
                return Vector3.Lerp(pts[i - 1], pts[i], t);
            }
            dist += seg;
        }
        return pts[pts.Count - 1];
    }

    // ---- Gizmos Debug ----
    private void OnDrawGizmos()
    {
        if (controlPoints == null || controlPoints.Length < 2)
            return;

        // Control points (??)
        Gizmos.color = Color.red;
        foreach (var cp in controlPoints)
        {
            if (cp != null)
                Gizmos.DrawSphere(cp.position, 0.1f);
        }

        // N?i control points (vàng)
        Gizmos.color = Color.yellow;
        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            if (controlPoints[i] != null && controlPoints[i + 1] != null)
                Gizmos.DrawLine(controlPoints[i].position, controlPoints[i + 1].position);
        }

        // ???ng Bezier (xanh lá)
        Gizmos.color = Color.green;
        if (controlPoints.Length >= 2)
        {
            Vector3 prev = GetBezierPoint(0);
            for (int i = 1; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                Vector3 p = GetBezierPoint(t);
                Gizmos.DrawLine(prev, p);
                prev = p;
            }
        }
    }
}