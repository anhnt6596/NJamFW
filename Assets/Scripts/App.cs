using UnityEngine;
using Core;
using System.Collections.Generic;

public class App : BaseApp
{
    public new static T Get<T>() where T : IManager => Instance.Get<T>(); 
    protected override void ConfigApp()
    {
        Input.multiTouchEnabled = false;
    }

    protected override void SetAllManagers()
    {
        Configs.Load();
        AddManager<SceneService>();
        AddManager(gameObject.GetComponentInChildren<GUIManager>());
    }

    protected override void StartApp()
    {
        Get<SceneService>().LoadScene(SceneName.MenuScene);
    }

    private void OnApplicationQuit()
    {

    }

    private void OnApplicationPause(bool isPause)
    {

    }
}
