using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
    // use for store current game state
    [SerializeField] GameState gameState;

    public Game RunningGame { get; private set; } = null;

    public void Init()
    {

    }

    public void StartUp()
    {

    }

    public void Cleanup()
    {

    }

    public void CreateNewGame(int level = 0)
    {
        gameState.Reset();
        RunningGame = new Game(level, gameState);
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
        RunningGame = null;
    }

    private void Update()
    {
        if (RunningGame != null) RunningGame.Update();
    }
}
