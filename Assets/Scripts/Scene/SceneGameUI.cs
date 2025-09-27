using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameUI : MonoBehaviour
{
    public void OnClickPause()
    {
        Time.timeScale = 0;
        App.Get<GUIManager>().ShowGui<PausePopup>();
    }
}
