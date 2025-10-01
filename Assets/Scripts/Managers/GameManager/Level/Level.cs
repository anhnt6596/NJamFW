using Core;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CW.Common.CwInputManager;

public class Level : MonoBehaviour
{
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform healthBarParent;
    
    public List<LineGroup> LineGroups { get; private set; } = new();
    List<EnemyVisual> enemies = new List<EnemyVisual>();
    List<HealthBar> healthBars = new List<HealthBar>();
    Game game;
    private void Awake()
    {
        // nen co nut de khoi tao, khong phai chay lai moi khi awake
        LineGroups.AddRange(GetComponentsInChildren<LineGroup>());
    }

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
        Game.Lightnings += OnGameLightnings;
        Game.StartNewWave += OnNewWaveStarted;
    }

    private void OnDisable()
    {
        Game.Lightnings -= OnGameLightnings;
        Game.StartNewWave -= OnNewWaveStarted;
    }

    void SpawnEnemy(EnemyEnum enemyType, int gateIdx)
    {
        var enemy = LeanPool.Spawn(ResourceProvider.GetEnemyVisual(enemyType), enemyParent);
        enemy.Setup(GetRandomMovingPath(gateIdx), Configs.GetEnemyConfig(enemyType));
        enemies.Add(enemy);

        var healthBar = LeanPool.Spawn(ResourceProvider.Component.HealthBar, healthBarParent);
        healthBar.Setup(enemy);
        healthBars.Add(healthBar);

        enemy.OnDeath += OnEnemyDeath;
        enemy.OnReachDestination += OnEnemyReachDestination;
    }

    private void DespawnEnemy(EnemyVisual enemy)
    {
        enemies.Remove(enemy);
        enemy.OnDeath -= OnEnemyDeath;
        enemy.OnReachDestination -= OnEnemyDeath;
        LeanPool.Despawn(enemy);
        LeanPool.Despawn(healthBars.First(h => h.Target == enemy));
    }

    private void OnEnemyDeath(EnemyVisual emeny)
    {
        DespawnEnemy(emeny);
    }

    private void OnEnemyReachDestination(EnemyVisual emeny)
    {
        DespawnEnemy(emeny);
    }

    private IMovingPath GetRandomMovingPath(int group)
    {
        var lineGroup = LineGroups[group];
        return lineGroup.GetRandomLine();
    }

    private void Update()
    {

    }

    private void OnNewWaveStarted(WaveConfig waveConfig)
    {
        foreach (var enemyGroup in waveConfig.EnemySpawnGroups)
        {
            this.DelayCall(enemyGroup.Delay, () => SpawnSimulated(enemyGroup));
        }
    }

    private void SpawnSimulated(EnemySpawnGroup group)
    {
        var space = group.Quantity == 0 ? 0 : group.SpawnTime / (group.Quantity);
        for (int i = 0; i < group.Quantity; i++)
        {
            this.DelayCall(i * space, () => SpawnEnemy(group.Enemy, group.GateIdx));
        }
    }

    private void OnGameLightnings(int times, Damage damage)
    {
        for (int i = 0; i < times; i++)
        {
            this.DelayCall(i * delayEachLightning, () => Lightning(damage));
        }
    }

    float delayEachLightning = 0.5f;
    float lightningTime = 0.25f;
    private void Lightning(Damage dmg)
    {
        if (!game.IsRunning) return;
        if (enemies.Count == 0) return;
        var emenyTake = enemies[Random.Range(0, enemies.Count)];
        App.Get<EffectManager>().SpawnLightning(emenyTake.GetFuturePosition(lightningTime));
        this.DelayCall(lightningTime, () =>
        {
            if (emenyTake.isDead) return;
            emenyTake.TakeDamage(dmg);
        });
    }
}
