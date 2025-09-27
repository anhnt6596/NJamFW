using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    public void LoadGame()
    {
        App.Instance.LoadGame();
    }
}
