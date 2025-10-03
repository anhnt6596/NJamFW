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
    [SerializeField] private TowerPlacement towerPlacementPrefab;
    [SerializeField] private List<Vector3> towerPlacementPositionConfigs;
    [SerializeField] Transform lightningRod;
    
    public List<LineGroup> LineGroups { get; private set; } = new();
    List<EnemyVisual> enemies = new List<EnemyVisual>();
    List<HealthBar> healthBars = new List<HealthBar>();
    List<TowerPlacement> towerPlacements = new List<TowerPlacement>();
    Game game;
    private void Awake()
    {
        // nen co nut de khoi tao, khong phai chay lai moi khi awake
        LineGroups.AddRange(GetComponentsInChildren<LineGroup>());
    }

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
        Game.StartNewWave += OnNewWaveStarted;

        Game.Lightnings += OnGameLightnings;
        Game.FreezeAllEnemies += OnAllEnemiesFroze;
        Game.BombDrop += OnBombDropped;
        Game.PlaceTower += TryPlaceTower;
    }


    private void OnDisable()
    {
        Game.StartNewWave -= OnNewWaveStarted;

        Game.Lightnings -= OnGameLightnings;
        Game.FreezeAllEnemies -= OnAllEnemiesFroze;
        Game.BombDrop -= OnBombDropped;
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

    private void OnEnemyDeath(Unit enemy)
    {
        DespawnEnemy((EnemyVisual)enemy);
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

    private void SpawnTowerPlacement()
    {
        // spawn tower placement prefab and add to list
        foreach (var positionConfig in towerPlacementPositionConfigs)
        {
            TowerPlacement placement = Instantiate(towerPlacementPrefab, this.transform);
            placement.transform.position = positionConfig;
            
            towerPlacements.Add(placement);
        }
    }

    public bool TryPlaceTower(Vector3 position)
    {
        // Check if all is placed
        
        // Check in range and is not placed
        
        bool allPlaced = true;
        TowerPlacement towerPlacementInRange = null;
        foreach (var towerPlacement in towerPlacements)
        {
            if (!towerPlacement.Placed)
            {
                allPlaced = false;
                if (towerPlacement.InTouchCollision(position))
                {
                    towerPlacementInRange = towerPlacement;
                }
            }
        }

        if (allPlaced)
        {
            return false;
        }

        if (towerPlacementInRange == null)
        {
            return false;
        }
        
        // If selected card is tower
        
        // place tower

        return true;
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

        bool upgradedLightning = game.State.selectedCards.Contains(CardEnum.LightningPower);
        if (enemies.Count == 0)
        {
            App.Get<EffectManager>().SpawnLightning(lightningRod.position, upgradedLightning);
            return;
        }
        var emenyTake = enemies[Random.Range(0, enemies.Count)];

        App.Get<EffectManager>().SpawnLightning(emenyTake.GetFuturePosition(lightningTime),upgradedLightning);

        this.DelayCall(lightningTime, () =>
        {
            if (emenyTake.isDead) return;
            emenyTake.TakeDamage(dmg);
        });
    }

    private void OnAllEnemiesFroze(float duration)
    {
        foreach (var enemy in enemies)
        {
            var status = enemy.statusList.Find(s => s.type == UnitStatusEnum.TimeFrozen);
            if (status != null) status.@params[0] = Mathf.Max(status.@params[0], duration);
            else enemy.statusList.Add(new UnitStatus(UnitStatusEnum.TimeFrozen, duration));
        }
    }

    private void OnBombDropped(Vector3 position, Damage damage, Vector2 radius)
    {
        this.DelayCall(0, () =>
        {
            App.Get<EffectManager>().SpawnBombEffect(position);
            for (int i = enemies.Count; i > 0; i--)
            {
                var enemy = enemies[i - 1];
                var v = GamePlayUtils.CheckElipse(enemy.transform.position, position, radius.x, radius.y);
                if (v < 1) enemy.TakeDamage(damage * GamePlayUtils.GetAoEDamageMultiplier(v, 0.45f));
            }
        });
    }
}
