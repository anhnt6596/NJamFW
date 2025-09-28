using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IManager
{
    private Game RunningGame { get; set; }

    public void Init()
    {

    }

    public void StartUp()
    {

    }

    public void Cleanup()
    {

    }

    public void StartNewGame(int level = 0)
    {
        var game = new Game(level);
        App.Get<ChangeSceneUI>().DoLoadScene(SceneName.GameScene);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        App.Get<GUIManager>().ShowGui<PausePopup>();
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        App.Get<ChangeSceneUI>().DoLoadScene(SceneName.MenuScene);
    }
}
