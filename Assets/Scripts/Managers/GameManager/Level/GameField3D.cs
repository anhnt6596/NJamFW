using Core;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField3D : MonoBehaviour, IGameField
{
    [SerializeField] Transform unitParent;
    [SerializeField] Transform healthBarParent;
    public List<LineGroup> LineGroups { get; private set; } = new();

    private List<HealthBar> healthBars = new List<HealthBar>();
    private void Awake()
    {
        // nen co nut de khoi tao, khong phai chay lai moi khi awake
        LineGroups.AddRange(GetComponentsInChildren<LineGroup>());
    }

    int spawnCount;
    public void StartNewWave(TurnConfig waveConfig)
    {
        SoundManager.Play(ResourceProvider.Sound.general.startTurn);
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

    void SpawnEnemy(EnemyEnum enemyType, int gateIdx)
    {
        var enemy = LeanPool.Spawn(ResourceProvider.GetEnemyVisual(enemyType), unitParent);
        enemy.Setup(GetRandomMovingPath(gateIdx), Configs.GetEnemyConfig(enemyType));
        Enemies.Add(enemy);

        var healthBar = LeanPool.Spawn(ResourceProvider.Component.HealthBar, healthBarParent);
        healthBar.Setup(enemy, Color.green);
        healthBars.Add(healthBar);

        enemy.OnDeath += OnEnemyDeath;
        enemy.OnReachDestination += OnEnemyReachDestination;
    }

    private void DespawnEnemy(Enemy enemy)
    {
        var healthBar = healthBars.First(h => h.Target == enemy);
        healthBars.Remove(healthBar);
        LeanPool.Despawn(healthBar);

        Enemies.Remove(enemy);
        enemy.OnDeath -= OnEnemyDeath;
        enemy.OnReachDestination -= OnEnemyReachDestination;
        LeanPool.Despawn(enemy);
    }

    private void OnEnemyDeath(Unit unit)
    {
        var enemy = (Enemy)unit;
        DespawnEnemy(enemy);
        Game.State.energy += enemy.config.DeathEnergy;
        if (spawnCount == 0
            && Enemies.Count == 0
            && Game.State.baseHealth > 0)
        {
            App.Get<GUIEffectManager>().BannerAnounce($"Turn {Game.CurrentTurn + 1} completed!");
            this.DelayCall(1.5f, () =>
            {
                SoundManager.Play(ResourceProvider.Sound.general.turnCompleted);
                Game.CompleteTurn();
            });
        }
    }

    private void OnEnemyReachDestination(Enemy enemy)
    {
        Game.TakeDamage(enemy.config.DamageToBase);
        App.Get<GUIEffectManager>().FlashScreen(new Color(1, 0, 0, 0.3f));
        OnEnemyDeath(enemy);

        SoundManager.Play(ResourceProvider.Sound.combat.damageTaken);
    }

    private IMovingPath GetRandomMovingPath(int group)
    {
        var lineGroup = LineGroups[group];
        return lineGroup.GetRandomLine();
    }

    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    public List<Tower> Towers => throw new System.NotImplementedException();
    public int TowerPlacementCount => throw new System.NotImplementedException();
    public Game Game { get ; set; }

    public void CastLightning(int times, Damage damage)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckPlaceTowerPosition(Vector3 wPos, TowerEnum tower, out int placeIndex)
    {
        throw new System.NotImplementedException();
    }

    public void DropBomb(Vector3 position, Damage damage, Vector2 radius)
    {
        throw new System.NotImplementedException();
    }

    public void DropMine(Vector3 wPos)
    {
        throw new System.NotImplementedException();
    }

    public void DropNapalm(Vector3 position, int fireNumber, Vector2 radius, Damage instantlyDamage, float interval, float damagePerSec, Vector2 eachRadius)
    {
        throw new System.NotImplementedException();
    }

    public void FreezeEnemies(float duration)
    {
        throw new System.NotImplementedException();
    }

    public bool IsWPosInPolygon(Vector3 wPos)
    {
        throw new System.NotImplementedException();
    }

    public void PlaceTower(int placeIndex, TowerEnum tower)
    {
        throw new System.NotImplementedException();
    }

    public void ReverseEnemies(Vector3 wPos, Vector3 radius, float duration)
    {
        throw new System.NotImplementedException();
    }

    public void SpawnAlly(AllyEnum allyType, Vector3 wPos)
    {
        throw new System.NotImplementedException();
    }
}
