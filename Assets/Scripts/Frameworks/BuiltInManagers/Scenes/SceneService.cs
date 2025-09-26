using UnityEngine.SceneManagement;

namespace Core
{
    public static class SceneName
    {
        public const string LoadingScene = "LoadingScene";
        public const string MenuScene = "MenuScene";
        public const string GameScene = "GameScene";
    } 

    public class SceneService : IManager
    {
        public string LastSceneName { get; private set; }
        public Scene ActiveScene => SceneManager.GetActiveScene();
        public void Init() { }
        public void StartUp() { }
        public void Cleanup() { }
        public void LoadScene(string sceneName)
        {
            LastSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
            ActionService.Dispatch<ChangeSceneAction>(sceneName, LastSceneName);
        }
    }
}