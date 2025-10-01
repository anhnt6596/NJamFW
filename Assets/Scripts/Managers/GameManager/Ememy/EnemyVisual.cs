using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyVisual : MonoBehaviour
{
    public IMovingPath line;
    public float speed = 0.5f;
    public float maxHP = 100f;
    public float HP;
    public bool isDead => HP <= 0;
    public float movingDist = 0;
    public Transform healthNode;

    public System.Action<EnemyVisual> OnDeath;
    public System.Action<EnemyVisual> OnReachDestination;

    public void Setup(IMovingPath line)
    {
        this.line = line;
        HP = maxHP;
        movingDist = 0;
        transform.position = line.GetPointByDistance(0);
    }

    private void Update()
    {
        if (line == null) return;
        if (isDead) return;

        // moving
        movingDist += Time.deltaTime * speed;

        // check reach destination
        float totalLen = line.GetTotalLength();
        if (movingDist >= totalLen)
        {
            ReachDestination();
        }
        else
        {
            transform.position = line.GetPointByDistance(movingDist);
        }
    }

    private void ReachDestination()
    {
        OnReachDestination?.Invoke(this);
        Destroy(gameObject);
    }


    public void TakeDamage(Damage dmg)
    {
        if (HP <= 0) return;
        HP -= dmg.amount;
        if (HP <= 0) Die();
    }

    void Die()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
        {
            var text = dieText[UnityEngine.Random.Range(0, dieText.Count)];
            App.Get<GUIEffectManager>().ShowScreenTextWP(text, healthNode.position, Color.white);
        }
        OnDeath?.Invoke(this);
    }

    public UnityEngine.Vector3 GetFuturePosition(float v)
    {
        float futureDist = movingDist + speed * v;
        return line.GetPointByDistance(futureDist);
    }

    List<string> dieText = new List<string>()
    {
        "!",
        "?",
        "A",
        "No Way",
        "Avenge me!",
        "Good Bye",
    };
}
