using Lean.Pool;
using System;
using UnityEngine;

public class Bullet : BaseBullet
{
    protected EnemyVisual target;

    public override void SetTarget(EnemyVisual enemy)
    {
        target = enemy;
        SoundManager.Play(ResourceProvider.Sound.general.arrowShot);
    }

    public float rotationOffset = 0f;
    void Update()
    {
        if (target == null || target.isDead)
        {
            LeanPool.Despawn(this);
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);

        if (Vector2.Distance(transform.position, target.transform.position) < 0.2f)
        {
            target.TakeDamage(damage);
            LeanPool.Despawn(this);
        }
    }
}