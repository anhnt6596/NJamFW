using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum GUILayer
{
    GUI = 0,
    POPUP = 1,
    LOADING = 2,
    TOP = 3
}

public class GUIManager : MonoBehaviour, IManager
{
    protected List<BaseGUI> PopupPrefabs { get; } = new();

    protected List<RectTransform> layers = new List<RectTransform>();
    protected Dictionary<string, BaseGUI> guis = new Dictionary<string, BaseGUI>();
    public void Init()
    {
        LoadAllPopupAssets();
        foreach (GUILayer val in Enum.GetValues(typeof(GUILayer)))
        {
            CreateLayer(val);
        }
    }
    public void StartUp()
    {
        ActionService.Sub<ChangeSceneAction>(OnSceneChanged);
    }
    public void Cleanup()
    {
        ActionService.Unsub<ChangeSceneAction>(OnSceneChanged);
    }

    private void OnSceneChanged(ChangeSceneAction action)
    {
        HideAllGUIs();
    }

    private void CreateLayer(GUILayer layerId)
    {
        GameObject panel = new GameObject("Layer-System: " + layerId);
        panel.transform.SetParent(transform);
        panel.transform.localPosition = Vector3.zero;
        panel.AddComponent<CanvasRenderer>();

        RectTransform parentRectTransform = GetComponent<RectTransform>();

        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.sizeDelta = parentRectTransform.sizeDelta;
        rectTransform.localScale = Vector3.one;

        layers.Add(rectTransform);
    }

    public RectTransform GetLayer(GUILayer id) => layers[((int)id)];

    public T ShowGui<T>() where T : BaseGUI
    {
        string guiId = typeof(T).FullName;
        if (guis.ContainsKey(guiId))
        {
            if (guis[guiId].IsActive) Debug.LogWarning("GUI shown" + guiId);
            else guis[guiId].Show();
        }
        else
        {
            T prefab = (T)PopupPrefabs.First(p => p is T);
            Transform layer = GetLayer(prefab.Layer);
            T gui = Instantiate(prefab, layer);
            guis[guiId] = gui;
            gui.Show();
        }
        guis[guiId].transform.SetAsLastSibling();
        return (T)guis[guiId];
    }
    public T GetGUIIfActive<T>() where T : BaseGUI
    {
        string guiId = typeof(T).FullName;
        if (guis.ContainsKey(guiId) && guis[guiId].IsActive) return (T)guis[guiId];
        return default;
    }
    public T CloseGUI<T>() where T : BaseGUI
    {
        string guiId = typeof(T).FullName;
        if (guis.ContainsKey(guiId) && guis[guiId].IsActive) guis[guiId].Hide();
        return default;
    }

    public void HideAllGUIs()
    {
        foreach (var gui in guis.Values) gui.Hide();
    }

    public void LoadAllPopupAssets()
    {
        PopupPrefabs.Clear();
        var ps = Resources.LoadAll("", typeof(BasePopup));
        foreach (var p in ps) PopupPrefabs.Add((BaseGUI)p);
        //EditorUtility.SetDirty(this);
    }
}