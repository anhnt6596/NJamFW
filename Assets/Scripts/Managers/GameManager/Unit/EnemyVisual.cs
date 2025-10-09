using System;
using System.Linq;
using UnityEngine;

public class EnemyVisual : Unit
{
    public EnemyConfig config { get; private set; }
    public override float speed => GetSpeed();

    private float GetSpeed()
    {
        var mult = 1f;
        var slow = statusList.FirstOrDefault(s => s.type == UnitStatusEnum.Slow);
        if (slow != null) mult = slow.@params[1];
        return config.Speed * mult;
    }

    public override float maxHP => config.Hp;
    public override Vector2 attackRange => config.AttackRange;
    public override float attackSpeed => config.AttackSpeed;
    private enum State { Moving, Combat }
    private State state = State.Moving;
    public float movingDist = 0;
    public IMovingPath line;
    public System.Action<EnemyVisual> OnReachDestination;

    private CharacterAnimator unitAnimator;

    private void Awake()
    {
        unitAnimator = GetComponentInChildren<CharacterAnimator>();
    }

    public Ally CurrentTarget { get; private set; }
    public void Setup(IMovingPath line, EnemyConfig config)
    {
        this.line = line;
        this.config = config;
        HP = config.Hp;
        def = config.Def;
        movingDist = 0;
        transform.position = line.GetPointByDistance(0);
        statusList = new();
        CurrentTarget = null;
    }

    private void Update()
    {
        if (line == null) return;
        if (isDead) return;
        switch (state)
        {
            case State.Moving:
                ProcessMoving();
                break;

            case State.Combat:
                CombatBehavior();
                break;
        }
        ApplyUpdateStatus();
    }

    private void ProcessMoving()
    {
        if (statusList.Exists(s => s.type == UnitStatusEnum.TimeFrozen))
        {
            unitAnimator.UpdateState(0);;
            return;
        }
        // moving
        movingDist += Time.deltaTime * speed;
       
        if (remainingDist <= 0.1f) ReachDestination();
        else
        {
            var last = transform.position;
            transform.position = line.GetPointByDistance(movingDist);
            var dir = GamePlayUtils.GetDirection2Index(transform.position - last);
            unitAnimator.UpdateState(1);
            if (dir != -1) unitAnimator.UpdateDir(dir);
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
        // neu co hieu ung khong che di chuyen cung anh huong dau ra cua ham nay, se bo sung sau
        if (CurrentTarget) return transform.position;

        float futureDist = movingDist + speed * v;
        return line.GetPointByDistance(futureDist);
    }

    private float lastAttackTime;

    bool hasAttackThisCycle = false;
    private void CombatBehavior()
    {
        if (CurrentTarget == null || CurrentTarget.isDead)
        {
            state = State.Moving;
            CurrentTarget = null;
            return;
        }

        if (statusList.Exists(s => s.type == UnitStatusEnum.TimeFrozen)) return;

        Vector2 totalAttackRange = attackRange + CurrentTarget.attackRange;
        float dist = GamePlayUtils.CheckElipse(transform.position, CurrentTarget.transform.position, totalAttackRange);
        if (dist > 1)
        {
            unitAnimator.UpdateState(0);
            unitAnimator.UpdateDir(GamePlayUtils.GetDirection2Index(CurrentTarget.transform.position - transform.position));
        }
        else
        {
            var remainAttackTime = Time.time - lastAttackTime;
            var shootCycle = 1f / attackSpeed;

            if (remainAttackTime > shootCycle * 0.85f && !hasAttackThisCycle)
            {
                unitAnimator.TriggerAttack();
                hasAttackThisCycle = true;
            }

            if (remainAttackTime >= shootCycle)
            {
                lastAttackTime = Time.time;
                CurrentTarget.TakeDamage(config.AttackDamage);
                hasAttackThisCycle = false;
            }
        }
    }

    public void SetTarget(Ally ally)
    {
        CurrentTarget = ally;
        state = State.Combat;
        lastAttackTime = Time.time;
    }

    public void Reverse(float duration)
    {
        movingDist = movingDist + speed * -duration;
        App.Get<EffectManager>().SpawnSmokeEffect(transform.position);

        transform.position = line.GetPointByDistance(movingDist);

        if (CurrentTarget != null) CurrentTarget.CurrentTarget = null;
        CurrentTarget = null;
    }
}
