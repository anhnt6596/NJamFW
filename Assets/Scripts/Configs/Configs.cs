using System;
using System.Collections.Generic;
using UnityEngine;

public static class Configs
{
    public static GamePlayConfig GamePlay;
    public static Dictionary<CardEnum, CardConfig> CardConfigs { get; } = new();
    public static void Load()
    {
        GamePlay = (GamePlayConfig)Resources.LoadAll("", typeof(GamePlayConfig))[0];
        LoadCardConfigs();
    }


    private static void LoadCardConfigs()
    {
        CardConfigs.Clear();
        var allConfigs = Resources.LoadAll("", typeof(CardConfig));
        foreach (var configObj in allConfigs)
        {
            var config = (CardConfig) configObj;
            if (!CardConfigs.ContainsKey(config.Card)) CardConfigs.Add(config.Card, config);
        }
    }
}
