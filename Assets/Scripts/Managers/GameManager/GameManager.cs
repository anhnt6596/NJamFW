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

    public void Restart()
    {
        Time.timeScale = 1;
        RunSceneGame(RunningGame.Level);
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        RunningGame = null;
        App.Get<SceneService>().LoadScene(SceneName.MenuScene);
        //App.Get<ChangeSceneUI>().DoLoadScene(SceneName.MenuScene);
    }

    public void GameWin()
    {
        this.DelayCall(0.5f, () =>
        {
            Time.timeScale = 0;
            App.Get<GUIManager>().ShowGui<WinPopup>();
        });
    }
    
    public void GameLose()
    {
        this.DelayCall(0.5f, () =>
        {
            Time.timeScale = 0;
            App.Get<GUIManager>().ShowGui<LosePopup>();
        });
    }

    public void CheckShowTutPopup(int tutIndex)
    {
        if (PlayerPrefs.GetInt($"tut_{tutIndex}", 0) == 1) return;
        Time.timeScale = 0;
        var popup = App.Get<GUIManager>().ShowGui<TutPopup>();
        popup.DisplayTut(tutIndex);
        popup.OKAction = () =>
        {
            ContinueGame();
            PlayerPrefs.SetInt($"tut_{tutIndex}", 1);
        };
    }

    private void Update()
    {
        if (RunningGame != null) RunningGame.Update();
    }


    private void OnApplicationPause(bool isPause)
    {
        if (isPause && RunningGame != null) PauseGame();
    }
}
