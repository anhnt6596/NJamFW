using UnityEngine;
using Core;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class App : BaseApp
{
    public new static T Get<T>() where T : IManager => Instance.Get<T>();
    [SerializeField] ChangeSceneUI changeSceneUI;
    protected override void ConfigApp()
    {
        Input.multiTouchEnabled = false;
    }

    protected override void SetAllManagers()
    {
        Configs.Load();
        ResourceProvider.LoadAllResourceSets();
        AddManager<SceneService>();
        AddManager(gameObject.GetComponentInChildren<GUIManager>());
        AddManager(gameObject.GetComponentInChildren<ChangeSceneUI>());
        AddManager(gameObject.GetComponentInChildren<GUIEffectManager>());
    }

    protected override void StartApp()
    {
        Get<SceneService>().LoadScene(SceneName.MenuScene);
    }

    public void LoadSceneGame()
    {
        changeSceneUI.Cover(() =>
        {
            Get<SceneService>().LoadScene(SceneName.GameScene);
        });
    }

    private void OnApplicationQuit()
    {

    }

    private void OnApplicationPause(bool isPause)
    {

    }
}
