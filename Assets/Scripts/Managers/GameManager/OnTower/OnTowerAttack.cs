using Lean.Pool;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OnTowerAttack : OnTower
{
    [SerializeField] Transform firePoint;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image fireIndicator;
    [SerializeField] Transform scaleNode;
    [SerializeField] Animator animator;
    [SerializeField] BaseBullet bulletPrefab;
    [SerializeField] OnTowerAttackConfig config;

    public int Level { get; private set; } = 1;
    public Damage damage => config.GetAttackByLevel(Level);
    public TowerEnum TowerType { get; private set; }

    private Vector3 oriScale;
    private float fireCooldown;
    private Enemy currentTarget;
    IGameField gameField;

    private void Awake()
    {
        oriScale = scaleNode.localScale;
    }

    public override void Setup(Tower tower)
    {
        gameField = App.Get<GameManager>().RunningGame.GameField;
        base.Setup(tower);
        Level = 1;
        DisplayLevel();
        fireCooldown = 1f / config.FireRate;
    }

    public void LevelUp()
    {
        Level++;
        DisplayLevel();
    }

    private void DisplayLevel() => levelText.text = $"Level {Level}";

    void Update()
    {
        if (!Ready) return;
        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
        fireIndicator.fillAmount = 1 - fireCooldown * config.FireRate;

        if (fireCooldown <= 0f)
        {
            FindTarget();
            if (currentTarget != null)
            {
                Shoot(currentTarget);
                fireCooldown = 1f / config.FireRate;
            }
        }
    }

    private void FindTarget()
    {
        if (currentTarget != null && !currentTarget.isDead)
        {
            // if last target in range, do not change
            if (GamePlayUtils.IsInRange(currentTarget.transform.position, transform.position, config.Range)) return;
        }

        var enemies = gameField.Enemies;
        currentTarget = null;
        float nearestDest = Mathf.Infinity;

        foreach (var e in enemies)
        {
            Vector2 diff = e.transform.position - transform.position;
            var remainDest = e.remainingDist;
            var inRange = GamePlayUtils.IsInRange(e.transform.position, transform.position, config.Range);

            if (inRange && remainDest < nearestDest)
            {
                nearestDest = remainDest;
                currentTarget = e;
            }
        }
    }

    void Shoot(Enemy target)
    {
        if (scaleNode != null)
        {
            var a = MovingUtils.GetDirection2Index(target.transform.position - scaleNode.transform.position, Camera.main.transform);
            scaleNode.localScale = new Vector3(a == Dir2.Right ? oriScale.x : -oriScale.x, oriScale.y, oriScale.z);
        }

        if (bulletPrefab == null || firePoint == null) return;

        if (animator) animator.SetTrigger("Shoot");

        //SoundManager.Play(ResourceProvider.Sound.combat.tower.GetShotSound(config.Type));

        BaseBullet bullet = LeanPool.Spawn(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.SetDamage(damage);
        bullet.Display();
        if (bullet != null)
        {
            bullet.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (config == null) return;
        Vector3 center = new Vector3(transform.position.x, 0f, transform.position.z);

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(center, Vector3.up, config.Range);
    }
}