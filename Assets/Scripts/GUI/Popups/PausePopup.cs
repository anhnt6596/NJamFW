using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Core;

public class PausePopup : BasePopup
{
    public void OnClickContinue()
    {
        Time.timeScale = 1;
        Close();
    }

    public void OnClickQuit()
    {
        Time.timeScale = 1;
        App.Get<ChangeSceneUI>().DoLoadScene(SceneName.MenuScene);
    }
}
