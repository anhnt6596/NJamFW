using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public enum SceneLoadState
    {
        None,
        FadeIn,
        Loading,
        FadeOut,
        Loaded
    }

    public static class SceneName
    {
        public const string LoadingScene = "LoadingScene";
        public const string MenuScene = "MenuScene";
        public const string GameScene = "GameScene";
    } 

    public class SceneService : MonoBehaviour, IManager
    {
        [SerializeField] ChangeSceneUI changeSceneUI;
        private SceneLoadState currentState = SceneLoadState.None;
        private string targetScene;
        private AsyncOperation loadOp;
        public string LastSceneName { get; private set; }
        public Scene ActiveScene => SceneManager.GetActiveScene();
        public void Init() { }
        public void StartUp() { }
        public void Cleanup() { }
        public void LoadScene(string sceneName)
        {
            if (currentState != SceneLoadState.None)
            {
                Debug.LogWarning("Scene is already loading.");
                return;
            }

            StartCoroutine(StateMachine());
            targetScene = sceneName;
            LastSceneName = SceneManager.GetActiveScene().name;
        }

        private IEnumerator StateMachine()
        {
            // Step 1: Fade In
            currentState = SceneLoadState.FadeIn;
            yield return changeSceneUI.CoverScene();

            // Step 2: Load Scene Async 
            currentState = SceneLoadState.Loading;
            loadOp = SceneManager.LoadSceneAsync(targetScene);
            loadOp.allowSceneActivation = false;

            while (loadOp.progress < 0.9f)
            {
                yield return null;
            }
            loadOp.allowSceneActivation = true;
            while (!loadOp.isDone)
            {
                yield return null;
            }
            ActionService.Dispatch<ChangeSceneAction>(targetScene, LastSceneName);

            // Step 3: Fade Out
            currentState = SceneLoadState.FadeOut;
            yield return changeSceneUI.ExposeScene();

            // Step 4: Loaded
            currentState = SceneLoadState.Loaded;
            currentState = SceneLoadState.None;
        }
    }
}