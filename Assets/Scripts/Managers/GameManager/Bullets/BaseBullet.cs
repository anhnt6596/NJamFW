using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class BaseBullet : MonoBehaviour
{
    public float speed = 6f;
    public Damage damage;
    public abstract void SetTarget(EnemyVisual enemy);
    public void SetDamage(Damage dmg) => damage = dmg;

    public void Display()
    {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps) ps.Play();
    }
}