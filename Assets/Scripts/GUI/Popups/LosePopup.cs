using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Core;

public class LosePopup : BasePopup
{
    private void OnEnable()
    {
        SoundManager.Play(ResourceProvider.Sound.general.lose);
    }

    public void OnClickQuit()
    {
        var gameMgr = App.Get<GameManager>();
        gameMgr.QuitGame();
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        var gameMgr = App.Get<GameManager>();
        gameMgr.RunSceneGame(gameMgr.RunningGame.Level);
    }
}
