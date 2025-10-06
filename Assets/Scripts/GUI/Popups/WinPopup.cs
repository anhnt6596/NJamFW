using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Core;

public class WinPopup : BasePopup
{
    public void OnClickContinue()
    {
        Time.timeScale = 1;
        App.Get<SceneService>().LoadScene(SceneName.MenuScene);
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        var gameMgr = App.Get<GameManager>();
        gameMgr.RunSceneGame(gameMgr.RunningGame.Level);
    }
}
