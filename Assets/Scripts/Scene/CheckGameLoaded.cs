using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckGameLoaded : MonoBehaviour
{
    private void Awake()
    {
        if (App.Instance == null || !App.Instance.IsInitialized) SceneManager.LoadScene(SceneName.LoadingScene);
    }
}
