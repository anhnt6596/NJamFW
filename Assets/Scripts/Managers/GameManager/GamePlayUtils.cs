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
    public static float CheckElipse(Vector2 p1, Vector2 p2, float rangeX, float rangeY)
    {
        var diff = p1 - p2;
        return (diff.x * diff.x) / (rangeX * rangeX) + (diff.y * diff.y) / (rangeY * rangeY);
    }

    public static bool IsInRange(Vector2 p1, Vector2 p2, float rangeX, float rangeY)
    {
        return CheckElipse(p1, p2, rangeX, rangeY) <= 1f;
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
}