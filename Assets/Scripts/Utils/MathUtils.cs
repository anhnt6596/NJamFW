using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// Trả về điểm giao giữa tia (origin, direction) với mặt phẳng XZ (y = 0).
    /// Trả về null nếu tia song song với mặt phẳng hoặc không cắt.
    /// </summary>
    public static Vector3 RaycastToXZPlane(Vector3 origin, Vector3 direction)
    {
        // Nếu hướng gần song song với mặt phẳng (hướng y ≈ 0), thì không có giao điểm
        if (Mathf.Abs(direction.y) < 1e-6f)
            return Vector3.zero;

        // Tính t bằng phương trình: origin.y + t * direction.y = 0  =>  t = -origin.y / direction.y
        float t = -origin.y / direction.y;

        // Nếu t < 0, nghĩa là tia hướng ngược lên, không cắt mặt phẳng phía trước
        if (t < 0)
            return Vector3.zero;

        // Điểm giao
        return origin + direction * t;
    }

    public static Vector3 GetOffsetXZ(GridSize size, float cellSize)
    {
        return new Vector3((size.w - 1) / 2f * cellSize, 0, (size.h - 1) / 2f * cellSize);
    }
}
