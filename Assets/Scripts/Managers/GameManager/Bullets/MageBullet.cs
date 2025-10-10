using Lean.Pool;
using UnityEngine;

public class MageBullet : BaseBullet
{
    protected EnemyVisual target;

    public override void SetTarget(EnemyVisual enemy)
    {
        target = enemy;
    }

    public float rotationOffset = 0f;
    void Update()
    {
        if (target == null || target.isDead)
        {
            target = FindAnotherTarget();
            if (target == null || target.isDead)
            {
                LeanPool.Despawn(this);
                return;
            }
        }

        var des = target.transform.position + Vector3.up * 0.25f;
        Vector3 dir = (des - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);

        if (Vector2.Distance(transform.position, des) < 0.2f)
        {
            target.AddStatus(new UnitStatus(UnitStatusEnum.Slow, 3, 0.5f));
            target.TakeDamage(damage);
            LeanPool.Despawn(this);
        }
    }

    private EnemyVisual FindAnotherTarget()
    {
        EnemyVisual target = null;
        var enemies = App.Get<GameManager>().RunningGame.GamePlay.Enemies;
        if (enemies.Count == 0) return null;

        float smallestMag = Mathf.Infinity;

        foreach (var e in enemies)
        {
            var mag = (e.transform.position - transform.position).magnitude;
            if (mag < smallestMag)
            {
                smallestMag = mag;
                target = e;
            }
        }
        return target;
    }
}