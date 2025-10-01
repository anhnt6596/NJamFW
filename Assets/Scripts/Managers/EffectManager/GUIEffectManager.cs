using Core;
using Lean.Pool;
using UnityEngine;

public class GUIEffectManager : MonoBehaviour, IManager
{
    [SerializeField] ScreenText screenTextPrefab;
    GUIManager _guiMgr;
    public void Init() { }

    public void StartUp()
    {
        _guiMgr = App.Get<GUIManager>();
    }

    public void Cleanup() { }

    public ScreenText ShowScreenTextWP(string content, Vector3 wPos, Color color, GUILayer layer = GUILayer.GUI)
    {
        var screenPos = Camera.main.WorldToScreenPoint(wPos);
        return ShowScreenText(content, screenPos, color, layer);
    }

    public ScreenText ShowScreenText(string content, Vector3 screenPos, Color color, GUILayer layer = GUILayer.GUI)
    {
        var text = LeanPool.Spawn(screenTextPrefab);
        var guiLayer = _guiMgr.GetLayer(layer);
        text.transform.parent = guiLayer;

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(guiLayer, screenPos, Camera.main, out uiPos);
        text.transform.position = screenPos;

        text.Show(content, color, () => LeanPool.Despawn(text));
        return text;
    }
}
