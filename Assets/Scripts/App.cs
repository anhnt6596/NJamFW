using UnityEngine;
using Core;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        ResourceProvider.LoadAllResourceSets();
        AddManager(gameObject.GetComponentInChildren<SceneService>());
        AddManager(gameObject.AddComponent<TouchService>());
        AddManager(gameObject.GetComponentInChildren<GUIManager>());
        AddManager(gameObject.GetComponentInChildren<GUIEffectManager>());
        AddManager(gameObject.GetComponentInChildren<EffectManager>());
        AddManager(gameObject.GetComponentInChildren<GameManager>());
    }

    protected override void StartApp()
    {
        SceneManager.LoadSceneAsync(SceneName.MenuScene);
    }

    private void OnApplicationQuit()
    {

    }

    private void OnApplicationPause(bool isPause)
    {

    }
}
