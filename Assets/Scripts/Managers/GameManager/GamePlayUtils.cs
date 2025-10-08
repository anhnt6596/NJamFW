using System;
using UnityEngine;

public static class GamePlayUtils
{
    public static Damage CalculateDamage(Damage input, DeffenseStats def)
    {
        switch (input.type)
        {
            case DamageEnum.Physical:
                {
                    float reduced = Configs.GamePlay.GetDmgRes(def.armor);
                    return new Damage(input.amount * (1 - reduced), input.type);
                }
            case DamageEnum.Magic:
                {
                    float reduced = Configs.GamePlay.GetDmgRes(def.magicRes);
                    return new Damage(input.amount * (1 - reduced), input.type);
                }
            case DamageEnum.True: return input;
            default: return input;
        }
    }

    public static float Y2Z(float y, float offset) => y * 0.01f + offset;
    public static Vector3 Y2Z(Vector3 v3, float offset) => new Vector3(v3.x, v3.y, Y2Z(v3.y, offset));
    public static float CheckElipse(Vector2 p1, Vector2 p2, Vector2 range)
    {
        var diff = p1 - p2;
        return (diff.x * diff.x) / (range.x * range.x) + (diff.y * diff.y) / (range.y * range.y);
    }

    public static bool IsInRange(Vector2 p1, Vector2 p2, Vector2 range)
    {
        return CheckElipse(p1, p2, range) <= 1f;
    }

    public static Vector2 GetRandomPointInEllipse(Vector2 center, Vector2 range)
    {
        float t = 2 * Mathf.PI * UnityEngine.Random.value;
        float u = UnityEngine.Random.value + UnityEngine.Random.value;
        float r = (u > 1) ? 2 - u : u;

        float x = r * Mathf.Cos(t) * range.x;
        float y = r * Mathf.Sin(t) * range.y;

        return center + new Vector2(x, y);
    }

    public static float GetAoEDamageMultiplier(float v, float fullRatio)
    {
        v = Mathf.Max(0f, v);
        fullRatio = Mathf.Clamp01(fullRatio);

        if (v >= 1f) return 0f;
        if (fullRatio >= 1f) return 1f;
        if (v <= fullRatio) return 1f;

        float inv = 1f / (1f - fullRatio);
        float t = (v - fullRatio) * inv;
        return Mathf.Clamp01(1f - t);
    }
    /// <summary>
    /// Trả về index hướng (0–7) tương ứng với vector đầu vào.
    /// 0 = Right, 1 = Up-Right, 2 = Up, 3 = Up-Left, 4 = Left, 5 = Down-Left, 6 = Down, 7 = Down-Right
    /// </summary>
    public static int GetDirection8Index(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.0001f)
            return -1; // vector quá nhỏ -> không xác định hướng

        // Lấy góc theo radian -> degree
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Đưa về khoảng [0, 360)
        if (angle < 0)
            angle += 360f;

        // Mỗi hướng chiếm 45°, offset 22.5° để làm tròn chính giữa
        int index = Mathf.FloorToInt((angle + 22.5f) / 45f) % 8;

        return index;
    }

    // 0: right | 1: left
    public static int GetDirection2Index(Vector2 dir, float deadZone = 0.1f)
    {
        dir = dir.normalized;
        if (Mathf.Abs(dir.x) < deadZone)
            return -1;

        return dir.x >= 0 ? 1 : 0;
    }
}