using UnityEngine;

public static class Configs
{
    public static GamePlayConfig GamePlay;
    public static void Load()
    {
        GamePlay = (GamePlayConfig)Resources.LoadAll("", typeof(GamePlayConfig))[0];
    }
}
