using Unity.Burst.Intrinsics;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerEnum towerType;
    int level = 1;
    TowerConfig config;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    public Damage damage => config.GetAttackByLevel(1);

    private float fireCooldown;

    private void Start()
    {
        config = Configs.GetTowerConfig(towerType);
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        EnemyVisual target = FindTarget();
        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = 1f / config.FireRate;
        }
    }

    EnemyVisual FindTarget()
    {
        EnemyVisual[] enemies = FindObjectsOfType<EnemyVisual>();
        EnemyVisual target = null;
        float nearestDest = Mathf.Infinity;

        foreach (var e in enemies)
        {
            Vector2 diff = e.transform.position - transform.position;
            var remainDest = e.remainingDist;
            var inRange = GamePlayUtils.IsInRange(e.transform.position, transform.position, config.Range.x, config.Range.y);

            if (inRange && remainDest < nearestDest)
            {
                nearestDest = remainDest;
                target = e;
            }
        }

        return target;
    }

    void Shoot(EnemyVisual target)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetDamage(damage);
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