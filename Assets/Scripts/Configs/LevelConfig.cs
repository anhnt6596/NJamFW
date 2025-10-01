using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Level Config", order = 0)]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private int levelIdx;
    [SerializeField] private List<WaveConfig> waveConfigs;
    
    public int LevelIdx => levelIdx;
    public WaveConfig GetWaveConfig(int idx) => waveConfigs[idx];
    public int WaveCount => waveConfigs.Count;

    
}

[Serializable]
public class WaveConfig
{
    [SerializeField] private List<EnemySpawnGroup> enemySpawnGroups;
    [SerializeField] private float waveMaxTime;
    
    public List<EnemySpawnGroup> EnemySpawnGroups => enemySpawnGroups;
    public float WaveMaxTime => waveMaxTime;
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