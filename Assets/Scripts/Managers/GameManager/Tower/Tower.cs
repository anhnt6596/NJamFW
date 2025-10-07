using Lean.Pool;
using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerEnum towerType;
    public int Level { get; private set; } = 1;
    TowerConfig config;
    [SerializeField] Transform firePoint;
    public Damage damage => config.GetAttackByLevel(Level);
    public TowerEnum TowerType => towerType;
    private float fireCooldown;
    private BaseBullet bulletPrefab;

    private EnemyVisual currentTarget;
    IGamePlay gamePlay;
    public void Setup(IGamePlay gamePlay)
    {
        this.gamePlay = gamePlay;
    }

    private void Start()
    {
        config = Configs.GetTowerConfig(towerType);
        bulletPrefab = ResourceProvider.GetBullet(towerType); 
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        FindTarget();
        if (currentTarget != null && fireCooldown <= 0f)
        {
            Shoot(currentTarget);
            fireCooldown = 1f / config.FireRate;
        }
    }

    public void LevelUp()
    {
        Level++;
    }

    private void FindTarget()
    {
        if (currentTarget != null && !currentTarget.isDead)
        {
            // if last target in range, do not change
            if (GamePlayUtils.IsInRange(currentTarget.transform.position, transform.position, config.Range)) return;
        }

        var enemies = gamePlay.Enemies;
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

    void Shoot(EnemyVisual target)
    {
        if (bulletPrefab == null || firePoint == null) return;

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
        Gizmos.color = Color.yellow;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(config.Range.x * 2, config.Range.y * 2, 1));
        Gizmos.matrix = matrix;
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;
    }

}