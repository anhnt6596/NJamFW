using Core;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CW.Common.CwInputManager;

public class Level : MonoBehaviour, IGamePlay
{
    [SerializeField] Transform unitParent;
    [SerializeField] Transform healthBarParent;
    [SerializeField] private List<TowerPlacement> towerPlacements;
    [SerializeField] Transform lightningRod;
    [SerializeField] PolygonDrawer poly;
    
    public List<LineGroup> LineGroups { get; private set; } = new();
    public List<EnemyVisual> Enemies { get; private set; } = new List<EnemyVisual>();
    public List<Ally> Allies { get; private set; } = new List<Ally>();
    List<HealthBar> healthBars = new List<HealthBar>();
    List<Tower> spawnedTowers = new List<Tower>();
    public Game Game { get; set; }
    private void Awake()
    {
        // nen co nut de khoi tao, khong phai chay lai moi khi awake
        LineGroups.AddRange(GetComponentsInChildren<LineGroup>());
    }

    private void OnEnable()
    {

    }


    private void OnDisable()
    {

    }

    void SpawnEnemy(EnemyEnum enemyType, int gateIdx)
    {
        var enemy = LeanPool.Spawn(ResourceProvider.GetEnemyVisual(enemyType), unitParent);
        enemy.Setup(GetRandomMovingPath(gateIdx), Configs.GetEnemyConfig(enemyType));
        Enemies.Add(enemy);

        var healthBar = LeanPool.Spawn(ResourceProvider.Component.HealthBar, healthBarParent);
        healthBar.Setup(enemy);
        healthBars.Add(healthBar);

        enemy.OnDeath += OnEnemyDeath;
        enemy.OnReachDestination += OnEnemyReachDestination;
    }

    private void DespawnEnemy(EnemyVisual enemy)
    {
        LeanPool.Despawn(healthBars.First(h => h.Target == enemy));
        Enemies.Remove(enemy);
        enemy.OnDeath -= OnEnemyDeath;
        enemy.OnReachDestination -= OnEnemyReachDestination;
        LeanPool.Despawn(enemy);
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

    public bool CheckPlaceTowerPosition(Vector3 wPos, TowerEnum towerEnum, out int placeIndex)
    {
        placeIndex = -1;
        for (int i = 0; i < towerPlacements.Count; i++)
        {
            var placement = towerPlacements[i];
            if (placement.CheckPostion(wPos, towerEnum))
            {
                placeIndex = i;
                return true;
            }
        }
        return false;
    }

    public void PlaceTower(int placeIndex, TowerEnum tower)
    {
        var placement = towerPlacements[placeIndex];
        placement.BuildTower(tower);
    }

    public bool IsWPosInPolygon(Vector3 wPos)
    {
        var pts = poly.GetPolygon2D();
        return GeometryUtils.IsPointInPolygon(wPos, pts);
    }

    public void SpawnAlly(AllyEnum allyType, Vector3 wPos)
    {
        var ally = LeanPool.Spawn(ResourceProvider.GetAlly(allyType), unitParent);
        ally.transform.position = wPos;
        ally.Setup(this, Configs.GetAllyConfig(allyType));
        Allies.Add(ally);

        var healthBar = LeanPool.Spawn(ResourceProvider.Component.HealthBar, healthBarParent);
        healthBar.Setup(ally);
        healthBars.Add(healthBar);

        ally.OnDeath += OnAllyDeath;
    }

    private void OnAllyDeath(Unit unit)
    {
        DespawnAlly((Ally)unit);
    }

    private void DespawnAlly(Ally ally)
    {
        LeanPool.Despawn(healthBars.First(h => h.Target == ally));
        Allies.Remove(ally);
        ally.OnDeath -= OnAllyDeath;
        LeanPool.Despawn(ally);
    }

    private void Update()
    {

    }

    public void OnNewWaveStarted(WaveConfig waveConfig)
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

    public void CastGameLightnings(int times, Damage damage)
    {
        for (int i = 0; i < times; i++)
        {
            this.DelayCall(i * delayEachLightning, () => Lightning(damage));
        }
    }

    float delayEachLightning = 0.5f;
    float lightningTime = 0.25f;
    public void Lightning(Damage dmg)
    {
        if (!Game.IsRunning) return;

        bool upgradedLightning = Game.State.selectedCards.Contains(CardEnum.LightningPower);
        if (Enemies.Count == 0)
        {
            App.Get<EffectManager>().SpawnLightning(lightningRod.position, upgradedLightning);
            return;
        }
        var emenyTake = Enemies[Random.Range(0, Enemies.Count)];

        App.Get<EffectManager>().SpawnLightning(emenyTake.GetFuturePosition(lightningTime),upgradedLightning);

        this.DelayCall(lightningTime, () =>
        {
            if (emenyTake.isDead) return;
            emenyTake.TakeDamage(dmg);
        });
    }

    public void OnAllEnemiesFroze(float duration)
    {
        foreach (var enemy in Enemies)
        {
            var status = enemy.statusList.Find(s => s.type == UnitStatusEnum.TimeFrozen);
            if (status != null) status.@params[0] = Mathf.Max(status.@params[0], duration);
            else enemy.statusList.Add(new UnitStatus(UnitStatusEnum.TimeFrozen, duration));
        }
    }

    public void OnAllEnemiesReversed(Vector3 wPos, Vector3 radius, float duration)
    {
        foreach (var enemy in Enemies)
        {
            if (!enemy.isDead
                && GamePlayUtils.IsInRange(wPos, enemy.transform.position, radius)
                ) enemy.Reverse(duration);
        }
    }

    public void OnBombDropped(Vector3 position, Damage damage, Vector2 radius)
    {
        this.DelayCall(0, () =>
        {
            App.Get<EffectManager>().SpawnBombEffect(position);
            for (int i = Enemies.Count; i > 0; i--)
            {
                var enemy = Enemies[i - 1];
                var v = GamePlayUtils.CheckElipse(enemy.transform.position, position, radius);
                if (v < 1) enemy.TakeDamage(damage * GamePlayUtils.GetAoEDamageMultiplier(v, 0.45f));
            }
        });
    }
}
