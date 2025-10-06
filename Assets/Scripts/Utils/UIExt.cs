using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIExt
{
    public static void SetAlpha(this TMP_Text text, float alpha)
    {
        var oriColor = text.color;
        oriColor.a = alpha;
        oriColor.r = Mathf.Clamp01(alpha);
    }
    public static void SetAlpha(this Image text, float alpha)
    {
        var oriColor = text.color;
        oriColor.a = alpha;
        oriColor.r = Mathf.Clamp01(alpha);
    }
}