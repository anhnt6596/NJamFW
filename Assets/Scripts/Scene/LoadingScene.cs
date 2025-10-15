using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private void Awake() => LoadGame();
    public void LoadGame() => App.Instance.LoadGame();
}
