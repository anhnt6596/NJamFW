using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Core;
using DG.Tweening;

public class WinPopup : BasePopup
{
    [SerializeField] WinPopup_StarRate starRate;
    private void OnEnable()
    {
        SoundManager.Play(ResourceProvider.Sound.general.win);    
    }

    public void DisplayRate(int rate)
    {
        starRate.HideAllStars();
        starRate.ShowStars(rate);
    }

    public void OnClickContinue()
    {
        var gameMgr = App.Get<GameManager>();
        gameMgr.QuitGame();
    }

    public void OnClickRestart()
    {
        var gameMgr = App.Get<GameManager>();
        gameMgr.Restart();
    }
}
