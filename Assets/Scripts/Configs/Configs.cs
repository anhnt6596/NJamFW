using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Configs
{
    public static GamePlayConfig GamePlay;
    public static Dictionary<CardEnum, CardConfig> CardConfigs { get; } = new();
    public static Dictionary<EnemyEnum, EnemyConfig> EnemyConfigs { get; } = new();
    public static Dictionary<int, LevelConfig> LevelConfigs { get; } = new();
    public static CardConfig GetCardConfig(CardEnum card) => CardConfigs[card];
    public static EnemyConfig GetEnemyConfig(EnemyEnum enemy) => EnemyConfigs[enemy];
    public static LevelConfig GetLevelConfig(int level) => LevelConfigs[level];
    public static void Load()
    {
        GamePlay = (GamePlayConfig)Resources.LoadAll("", typeof(GamePlayConfig))[0];
        LoadCardConfigs();
        LoadEnemyConfigs();
        LoadLevelConfigs();
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

    private static void LoadEnemyConfigs()
    {
        EnemyConfigs.Clear();
        var allConfigs = Resources.LoadAll("", typeof(EnemyConfig));
        foreach (var configObj in allConfigs)
        {
            var config = (EnemyConfig) configObj;
            if (!EnemyConfigs.ContainsKey(config.Type)) EnemyConfigs.Add(config.Type, config);
        }
    }

    private static void LoadLevelConfigs()
    {
        LevelConfigs.Clear();
        var allConfigs = Resources.LoadAll("", typeof(LevelConfig));
        foreach (var configObj in allConfigs)
        {
            var config = (LevelConfig) configObj;
            if (!LevelConfigs.ContainsKey(config.LevelIdx)) LevelConfigs.Add(config.LevelIdx, config);
        }
    }
}
