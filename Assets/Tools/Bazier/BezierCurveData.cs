using UnityEngine;
using System.Collections.Generic;

public class BezierCurveData : MonoBehaviour
{
    [Tooltip("Danh sách các điểm đã sinh ra trên curve (theo thứ tự)")]
    public List<Transform> points = new List<Transform>();

    /// <summary>
    /// Lấy vị trí theo quãng đường đã đi được tính từ điểm đầu tiên.
    /// Ví dụ: nếu khoảng cách giữa các point ~1, đầu vào 4.5 thì sẽ lấy điểm giữa point[4] và point[5].
    /// </summary>
    public Vector3 GetPositionAtDistance(float distance)
    {
        if (points == null || points.Count == 0)
            return Vector3.zero;

        if (points.Count == 1)
            return points[0].position;

        float traveled = 0f;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 a = points[i].position;
            Vector3 b = points[i + 1].position;
            float segLength = Vector3.Distance(a, b);

            if (traveled + segLength >= distance)
            {
                float t = (distance - traveled) / segLength;
                return Vector3.Lerp(a, b, t);
            }

            traveled += segLength;
        }

        // Nếu distance vượt quá tổng chiều dài, trả về điểm cuối
        return points[points.Count - 1].position;
    }

    public float GetTotalLength()
    {
        if (points == null || points.Count < 2) return 0f;

        float length = 0f;
        for (int i = 1; i < points.Count; i++)
        {
            if (points[i - 1] != null && points[i] != null)
                length += Vector3.Distance(points[i - 1].position, points[i].position);
        }
        return length;
    }

    private void OnDrawGizmos()
    {
        if (points == null || points.Count < 2) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }

        Gizmos.color = Color.magenta;
        foreach (var p in points)
        {
            if (p != null)
                Gizmos.DrawSphere(p.position, 0.05f);
        }
    }
}