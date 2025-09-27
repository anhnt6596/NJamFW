using Core;
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

    public GameObject FlyText()
    {
        return default;
    }
}
