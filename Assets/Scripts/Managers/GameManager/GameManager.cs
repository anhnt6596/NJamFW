using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IManager
{
    public int LevelToPrepare { get; set; } // in the future, level become string, play in any branch, any mode
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

    public void RunSceneGame(int level = 0)
    {
        LevelToPrepare = level;
        App.Get<SceneService>().LoadScene(SceneName.GameScene);
    }

    public Game CreateGame()
    {
        gameState.Reset();
        RunningGame = new Game(LevelToPrepare, gameState);
        return RunningGame;
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
        App.Get<SceneService>().LoadScene(SceneName.MenuScene);
        //App.Get<ChangeSceneUI>().DoLoadScene(SceneName.MenuScene);
        RunningGame = null;
    }

    public void GameLose()
    {
        Time.timeScale = 0;
        App.Get<GUIManager>().ShowGui<LosePopup>();
    }

    private void Update()
    {
        if (RunningGame != null) RunningGame.Update();
    }
}
