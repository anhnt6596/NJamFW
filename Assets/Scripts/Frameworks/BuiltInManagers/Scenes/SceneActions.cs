using Core;
using UnityEngine.SceneManagement;

public class ChangeSceneAction : IAction
{
    public string SceneName { get; private set; }
    public string LastSceneName { get; private set; }
    public void SetData(object[] _params)
    {
        SceneName = (string)_params[0];
        LastSceneName = (string)_params[1];
    }
}

public class SceneLoadedAction : IAction
{
    public Scene scene { get; private set; }
    public void SetData(object[] _params)
    {
        scene = (Scene)_params[0];
    }
}