using Core;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CW.Common.CwInputManager;

public class Level : MonoBehaviour
{
    [SerializeField] float spawnTime = 3f;
    [SerializeField] Transform enemyParent;
    [SerializeField] EnemyVisual enemyPrefab;
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
    }

    private void OnDisable()
    {
        Game.Lightnings -= OnGameLightnings;
    }

    void SpawnEnemy()
    {
        var enemy = LeanPool.Spawn(enemyPrefab, enemyParent);
        enemy.Setup(GetRandomMovingPath(Random.Range(0, LineGroups.Count)));
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

    float _spawnTimer;
    float _lightningTimer;
    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        _lightningTimer += Time.deltaTime;
        if (_spawnTimer >= spawnTime)
        {
            SpawnEnemy();
            _spawnTimer -= spawnTime;
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
