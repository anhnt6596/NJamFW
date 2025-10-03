using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 6f;
    public Damage damage;

    private EnemyVisual target;
    public float rotationOffset = 0f;

    public void SetDamage(Damage dmg) => damage = dmg;

    public void SetTarget(EnemyVisual enemy)
    {
        target = enemy;
    }

    public void Display()
    {
        var particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null) particleSystem.Play();
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);

        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}