using TMPro;
using UnityEditor;
using UnityEngine;

public static class UIExt
{
    public static void SetAlpha(this TMP_Text text, float alpha)
    {
        var oriColor = text.color;
        oriColor.a = alpha;
        oriColor.r = Mathf.Clamp01(alpha);
    }
}