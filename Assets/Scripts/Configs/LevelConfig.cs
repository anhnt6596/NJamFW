using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Level Config", order = 0)]
public class LevelConfig : ScriptableObject
{
    [SerializeField]
    private List<WaveConfig> waveConfigs;
    
    public List<WaveConfig> WaveConfigs => waveConfigs;
}

[Serializable]
public class WaveConfig
{
    [SerializeField] private List<EnemySpawnGroup> enemySpawnGroups;
    [SerializeField] private float spawnInterval = 1f;
    
    public float SpawnInterval => spawnInterval;
    public List<EnemySpawnGroup> EnemySpawnGroups => enemySpawnGroups;
}

[Serializable]
public class EnemySpawnGroup
{
    [SerializeField] private EnemyEnum enemyEnum;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private int gateIdx;
    public int EnemyCount => enemyCount;
    public int GateIdx => gateIdx;
    public EnemyEnum EnemyEnum => enemyEnum;
}