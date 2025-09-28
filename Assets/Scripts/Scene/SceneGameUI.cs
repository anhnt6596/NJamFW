using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameUI : MonoBehaviour
{
    public void OnClickPause()
    {
        App.Get<GameManager>().PauseGame();
    }
}
