using Unity.Burst.Intrinsics;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float attackRange = 3f;       // dùng khi hình tròn
    public float rangeX = 3f;            // ellipse bán trục X
    public float rangeY = 1.5f;          // ellipse bán trục Y
    public float fireRate = 1f;          // số phát/giây
    public GameObject bulletPrefab;      // prefab đạn
    public Transform firePoint;          // chỗ bắn đạn
    public Damage damage;              // sát thương đạn

    private float fireCooldown;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        EnemyVisual target = FindTarget();
        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = 1f / fireRate;
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
            var inRange = GamePlayUtils.IsInRange(e.transform.position, transform.position, rangeX, rangeY);

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
        Gizmos.color = Color.yellow;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(rangeX * 2, rangeY * 2, 1));
        Gizmos.matrix = matrix;
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;
    }
}