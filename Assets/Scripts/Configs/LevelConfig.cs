using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ALevel", menuName = "Config/Level", order = 0)]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private int levelIdx;
    [SerializeField] private List<TurnConfig> waveConfigs;
    
    public int LevelIdx => levelIdx;
    public TurnConfig GetTurnConfig(int idx) => waveConfigs[idx];
    public int TurnCount => waveConfigs.Count;

    
}

[Serializable]
public class TurnConfig
{
    [SerializeField] private List<EnemySpawnGroup> enemySpawnGroups;
    [SerializeField] private double turnEnergyGain = 2;
    [SerializeField] private int freeRollGain = 1;

    public List<EnemySpawnGroup> EnemySpawnGroups => enemySpawnGroups;
    public double TurnEnergyGain => turnEnergyGain;
    public int TurnFreeRollGain => freeRollGain;
}

[Serializable]
public class EnemySpawnGroup
{
    [SerializeField] private EnemyEnum enemy;
    [SerializeField] private int quantity = 0;
    [SerializeField] private int gateIdx;
    [SerializeField] private float delay;
    [SerializeField] private float spawnTime;
    public EnemyEnum Enemy => enemy;
    public int Quantity => quantity;
    public int GateIdx => gateIdx;
    public float Delay => delay;
    public float SpawnTime => spawnTime;
}