using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    private AllyConfig config;
    public override float speed => config.Speed;
    public override float maxHP => config.Hp;
    public override Vector2 attackRange => config.AttackRange;
    public override float attackSpeed => config.AttackSpeed;


    public EnemyVisual CurrentTarget { get; set; }
    private float lastAttackTime;

    private IGamePlay level;
    private enum State { Search, Combat }
    private State state = State.Search;

    public void Setup(IGamePlay level, AllyConfig config)
    {
        this.level = level;
        this.config = config;
        HP = config.Hp;
        def = config.Def;
        statusList = new();
        CurrentTarget = null;
    }

    void Update()
    {
        if (isDead) return;
        switch (state)
        {
            case State.Search:
                SearchForTarget();
                break;

            case State.Combat:
                CombatBehavior();
                break;
        }
    }

    void SearchForTarget()
    {
        var enemies = level.Enemies;

        EnemyVisual nearestEnemy = null;
        float smallestV = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            if (enemy != null && !enemy.isDead && enemy.CurrentTarget == null)
            {
                var v = GamePlayUtils.CheckElipse(transform.position, enemy.transform.position, config.DetectionRadius);
                if (v < 1 && v < smallestV)
                {
                    smallestV = v;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null)
        {
            CurrentTarget = nearestEnemy;
            nearestEnemy.SetTarget(this);
            state = State.Combat;
            lastAttackTime = Time.time;
        }
    }

    void CombatBehavior()
    {
        if (CurrentTarget == null || CurrentTarget.isDead)
        {
            CurrentTarget = null;
            state = State.Search;
            return;
        }

        Vector2 totalAttackRange = attackRange + CurrentTarget.attackRange;
        float dist = GamePlayUtils.CheckElipse(transform.position, CurrentTarget.transform.position, totalAttackRange);

        if (dist <= 1)
        {
            if (Time.time - lastAttackTime >= 1f / attackSpeed)
            {
                lastAttackTime = Time.time;
                CurrentTarget.TakeDamage(config.AttackDamage);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                CurrentTarget.transform.position,
                config.Speed * Time.deltaTime
            );
            lastAttackTime = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!config) return;
        Gizmos.color = Color.yellow;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(config.AttackRange.x * 2, config.AttackRange.y * 2, 1));
        Gizmos.matrix = matrix;
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        Matrix4x4 matrix2 = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(config.DetectionRadius.x * 2, config.DetectionRadius.y * 2, 1));
        Gizmos.matrix = matrix2;
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
