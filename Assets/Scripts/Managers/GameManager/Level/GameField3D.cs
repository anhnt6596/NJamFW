using Core;
using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField3D : MonoBehaviour, IGameField
{
    [SerializeField] Transform unitParent;
    [SerializeField] Transform healthBarParent;
    [SerializeField] GameObject grid;
    [SerializeField] PlacingSquare placingSquare;
    public List<LineGroup> LineGroups { get; private set; } = new();

    private List<HealthBar> healthBars = new List<HealthBar>();
    protected IGrid Grid { get; set; }
    private void Awake()
    {
        // nen co nut de khoi tao, khong phai chay lai moi khi awake
        LineGroups.AddRange(GetComponentsInChildren<LineGroup>());

        Grid = grid.GetComponent<IGrid>();
        placingSquare.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Game.OnInputStateChanged += OnInputStateChanged;
    }
    
    private void OnDisable()
    {
        Game.OnInputStateChanged -= OnInputStateChanged;
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

    public bool CheckValidWPosOnGrid(Vector3 wPos, out GridPos pos)
    {
        var config = (ObjectCardConfig) Configs.GetCardConfig(Game.PlayingCard);
        var size = config.Size;
        var offset = MathUtils.GetOffsetXZ(size, Grid.CellSize);
        Grid.WorldToCell(wPos - offset, out int x, out int y);
        pos = new GridPos(x, y);
        if (Grid.IsAreaFreeRect(x, y, size.w, size.h))
        {
            return true;
        }
        return false;
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

    PlaceObject ghostObject;
    public void ShowGhostObject(Vector3 wPos)
    {
        var avaiablePos = CheckValidWPosOnGrid(wPos, out GridPos gPos);
        var config = (ObjectCardConfig)Configs.GetCardConfig(Game.PlayingCard);
        if (!ghostObject)
        {
            ghostObject = LeanPool.Spawn(ResourceProvider.GetPlaceObject(config.ObjectType));
            placingSquare.gameObject.SetActive(true);
            placingSquare.Display(config.Size);
        }

        var offset = MathUtils.GetOffsetXZ(config.Size, Grid.CellSize);
        ghostObject.transform.position = Grid.CellToWorld(gPos.x, gPos.y) + offset;
        placingSquare.transform.position = ghostObject.transform.position;
        placingSquare.ShowAvaiable(avaiablePos);
    }

    private void OnInputStateChanged(InputStateEnum state)
    {
        LeanPool.Despawn(ghostObject);
        placingSquare.gameObject.SetActive(false);
        ghostObject = null;
    }

    public void PlaceObject(PlaceObjectEnum objectType, GridPos gPos)
    {
        var prefab = ResourceProvider.GetPlaceObject(objectType);
        var obj = LeanPool.Spawn(prefab, transform);
        var offset = MathUtils.GetOffsetXZ(prefab.Size, Grid.CellSize);
        obj.transform.position = Grid.CellToWorld(gPos.x, gPos.y) + offset;
        Grid.OccupyRect(gPos.x, gPos.y, prefab.Size.w, prefab.Size.h);
    }
}
