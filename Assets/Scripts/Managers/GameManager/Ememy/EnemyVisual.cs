using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyVisual : Unit
{
    private EnemyConfig config;
    public override float speed => config.Speed;
    public override float maxHP => config.Hp;
    public float movingDist = 0;
    public IMovingPath line;
    public System.Action<EnemyVisual> OnReachDestination;

    public void Setup(IMovingPath line, EnemyConfig config)
    {
        this.line = line;
        this.config = config;
        HP = config.Hp;
        def = config.Def;
        movingDist = 0;
        transform.position = line.GetPointByDistance(0);
        statusList = new();
    }

    private void Update()
    {
        if (line == null) return;
        if (isDead) return;

        ProcessMoving();
        ApplyUpdateStatus();
    }

    private void ProcessMoving()
    {
        if (statusList.Exists(s => s.type == UnitStatusEnum.TimeFrozen)) return;
        // moving
        movingDist += Time.deltaTime * speed;
        if (remainingDist <= 0.1f) ReachDestination();
        else transform.position = line.GetPointByDistance(movingDist);
    }

    private void ApplyUpdateStatus()
    {
        for (int i = statusList.Count; i > 0; i--)
        {
            var status = statusList[i - 1];
            switch (status.type)
            {
                //case UnitStatusEnum.Burning:
                //    if (s.@params.Count < 2) break;
                //    float burnDmg = s.@params[0];
                //    float burnDur = s.@params[1];
                //    TakeDamage(new Damage() { amount = burnDmg * Time.deltaTime, type = DamageType.Fire });
                //    s.@params[1] -= Time.deltaTime;
                //    if (s.@params[1] <= 0) s.@params[1] = 0;
                //    break;
                //case UnitStatusEnum.Poisoned:
                //    if (s.@params.Count < 2) break;
                //    float poisonDmg = s.@params[0];
                //    float poisonDur = s.@params[1];
                //    TakeDamage(new Damage() { amount = poisonDmg * Time.deltaTime, type = DamageType.Poison });
                //    s.@params[1] -= Time.deltaTime;
                //    if (s.@params[1] <= 0) s.@params[1] = 0;
                //    break;
                case UnitStatusEnum.TimeFrozen:
                    float frozenDur = status.@params[0];
                    status.@params[0] -= Time.deltaTime;
                    if (status.@params[0] <= 0) statusList.Remove(status);
                    break;
            }
        }
    }

    public float remainingDist => line.GetTotalLength() - movingDist;

    private void ReachDestination()
    {
        OnReachDestination?.Invoke(this);
        Destroy(gameObject);
    }

    public UnityEngine.Vector3 GetFuturePosition(float v)
    {
        float futureDist = movingDist + speed * v;
        return line.GetPointByDistance(futureDist);
    }
}
