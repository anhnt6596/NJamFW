using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIExt
{
    public static void SetAlpha(this TMP_Text text, float alpha)
    {
        var color = text.color;
        color.a = alpha;
        color.a = Mathf.Clamp01(alpha);
        text.color = color;
    }

    public static void SetAlpha(this Image img, float alpha)
    {
        var color = img.color;
        color.a = alpha;
        color.a = Mathf.Clamp01(alpha);
        img.color = color;
    }

    public static void SetAlpha(this SpriteRenderer img, float alpha)
    {
        var color = img.color;
        color.a = alpha;
        color.a = Mathf.Clamp01(alpha);
        img.color = color;
    }
}