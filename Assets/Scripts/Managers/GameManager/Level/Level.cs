using Core;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public Game Game { get; set; }
    public List<Tower> Towers => towerPlacements.Where(tp => tp.Tower != null).Select(tp => tp.Tower).ToList();
    public int TowerPlacementCount => towerPlacements.Count;
    private void Awake()
    {
        // nen co nut de khoi tao, khong phai chay lai moi khi awake
        LineGroups.AddRange(GetComponentsInChildren<LineGroup>());
    }

    private void OnEnable()
    {
        Game.OnInputStateChanged += OnInputStateChanged;
    }


    private void OnDisable()
    {
        Game.OnInputStateChanged -= OnInputStateChanged;
    }

    void SpawnEnemy(EnemyEnum enemyType, int gateIdx)
    {
        var enemy = LeanPool.Spawn(ResourceProvider.GetEnemyVisual(enemyType), unitParent);
        enemy.Setup(GetRandomMovingPath(gateIdx), Configs.GetEnemyConfig(enemyType));
        Enemies.Add(enemy);

        var healthBar = LeanPool.Spawn(ResourceProvider.Component.HealthBar, healthBarParent);
        healthBar.Setup(enemy, Color.red);
        healthBars.Add(healthBar);

        enemy.OnDeath += OnEnemyDeath;
        enemy.OnReachDestination += OnEnemyReachDestination;
    }

    private void DespawnEnemy(EnemyVisual enemy)
    {
        var healthBar = healthBars.First(h => h.Target == enemy);
        healthBars.Remove(healthBar);
        LeanPool.Despawn(healthBar);

        Enemies.Remove(enemy);
        enemy.OnDeath -= OnEnemyDeath;
        enemy.OnReachDestination -= OnEnemyReachDestination;
        LeanPool.Despawn(enemy);
    }

    private void OnEnemyDeath(Unit enemy)
    {
        DespawnEnemy((EnemyVisual)enemy);

        if (spawnCount == 0
            && Enemies.Count == 0
            && Game.IsLastWave
            && Game.State.baseHealth > 0) Game.Win();
    }

    private void OnEnemyReachDestination(EnemyVisual enemy)
    {
        Game.TakeDamage(enemy.config.DamageToBase);
        App.Get<GUIEffectManager>().FlashScreen(new Color(1, 0, 0, 0.3f));
        SoundManager.Play(ResourceProvider.Sound.general.damageSound);
        OnEnemyDeath(enemy);
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
        placement.BuildTower(tower, this);
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
        healthBar.Setup(ally, Color.green);
        healthBars.Add(healthBar);

        ally.OnDeath += OnAllyDeath;
    }

    private void OnAllyDeath(Unit unit)
    {
        DespawnAlly((Ally)unit);
    }

    private void DespawnAlly(Ally ally)
    {
        var healthBar = healthBars.First(h => h.Target == ally);
        healthBars.Remove(healthBar);
        LeanPool.Despawn(healthBar);

        Allies.Remove(ally);
        ally.OnDeath -= OnAllyDeath;
        LeanPool.Despawn(ally);
    }

    private void Update()
    {

    }

    int spawnCount; // > 0 : spawning, = 0 : spawnCompleted

    public void StartNewWave(WaveConfig waveConfig)
    {
        spawnCount = 0;
        for (int i = 0; i < waveConfig.EnemySpawnGroups.Count; i++)
        {
            var enemyGroup = waveConfig.EnemySpawnGroups[i];
            spawnCount++;
            this.DelayCall(enemyGroup.Delay, () =>
            {
                SpawnGroup(enemyGroup);
                spawnCount--;
            });
        }
        this.DelayCall(waveConfig.WaveMaxTime, () => Game.CheckRunNextWave());
    }

    private void SpawnGroup(EnemySpawnGroup group)
    {
        var space = group.Quantity == 0 ? 0 : group.SpawnTime / (group.Quantity);
        for (int i = 0; i < group.Quantity; i++)
        {
            spawnCount++;
            this.DelayCall(i * space, () =>
            {
                SpawnEnemy(group.Enemy, group.GateIdx);
                spawnCount--;
            });
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
    float lightningTime = 0.2f;
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

        App.Get<EffectManager>().SpawnLightning(emenyTake.GetFuturePosition(lightningTime), upgradedLightning);

        this.DelayCall(lightningTime, () =>
        {
            CameraShake.Shake(0.2f, 0.03f);
            if (emenyTake.isDead) return;
            emenyTake.TakeDamage(dmg);
        });
    }

    public void FreezeEnemies(float duration)
    {
        foreach (var enemy in Enemies)
        {
            var status = enemy.statusList.Find(s => s.type == UnitStatusEnum.TimeFrozen);
            if (status != null) status.@params[0] = Mathf.Max(status.@params[0], duration);
            else enemy.statusList.Add(new UnitStatus(UnitStatusEnum.TimeFrozen, duration));
        }
    }

    public void ReverseEnemies(Vector3 wPos, Vector3 radius, float duration)
    {
        SoundManager.Play(ResourceProvider.Sound.general.teleport);
        foreach (var enemy in Enemies)
        {
            if (!enemy.isDead
                && GamePlayUtils.IsInRange(wPos, enemy.transform.position, radius)
                ) enemy.Reverse(duration);
        }
    }

    public void DropBomb(Vector3 position, Damage damage, Vector2 radius)
    {
        this.DelayCall(0, () =>
        {
            CameraShake.Shake(0.3f, 0.1f);
            App.Get<EffectManager>().SpawnBombEffect(position);
            for (int i = Enemies.Count; i > 0; i--)
            {
                var enemy = Enemies[i - 1];
                var v = GamePlayUtils.CheckElipse(enemy.transform.position, position, radius);
                if (v < 1)
                {
                    enemy.TakeDamage(damage * GamePlayUtils.GetAoEDamageMultiplier(v, 0.45f));
                }
            }
        });
    }

    public void DropNapalm(Vector3 position, int fireNumber, Vector2 radius, Damage instantlyDamage, float interval, float dps, Vector2 eachRadius)
    {
        for (int i = 0; i < fireNumber; i++)
        {
            this.DelayCall(i * 0.1f, () => {
                var aFirePos = GamePlayUtils.GetRandomPointInEllipse(position, radius);
                var dur = App.Get<EffectManager>().NapalmDrop(aFirePos);
                this.DelayCall(dur, () =>
                {
                    CameraShake.Shake(0.3f, 0.1f);
                    for (int i = Enemies.Count; i > 0; i--)
                    {
                        var enemy = Enemies[i - 1];
                        var v = GamePlayUtils.CheckElipse(enemy.transform.position, aFirePos, eachRadius);
                        if (v < 1)
                        {
                            enemy.AddStatus(new UnitStatus(UnitStatusEnum.Burning, interval, dps));
                            enemy.TakeDamage(instantlyDamage);
                        }
                    }
                });
            });
        }
    }

    private void OnInputStateChanged(InputStateEnum state)
    {
        if (
            state == InputStateEnum.PlayCard
            && Configs.GetCardConfig(Game.PlayingCard) is TowerCardConfig towerConfig
            )
            towerPlacements.ForEach(tp =>
            {
                if (!tp.Tower || tp.Tower.TowerType == towerConfig.Tower) tp.Focus(true);
            });
        else
            towerPlacements.ForEach(tp => tp.Focus(false));
    }
}
