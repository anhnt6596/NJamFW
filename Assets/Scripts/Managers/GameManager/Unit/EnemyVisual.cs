using System;
using System.Linq;
using UnityEngine;

public class EnemyVisual : Unit
{
    [SerializeField] private CharacterAnimator characterAnimator;
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


    public Ally CurrentTarget { get; private set; }
    private int currentDir;
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
            characterAnimator.UpdateState(0);
            characterAnimator.UpdateDir(currentDir);
            return;
        }
        // moving
        movingDist += Time.deltaTime * speed;
       
        if (remainingDist <= 0.1f) ReachDestination();
        else
        {
            var last = transform.position;
            transform.position = line.GetPointByDistance(movingDist);
            currentDir = GamePlayUtils.GetDirection8Index(transform.position - last);
            characterAnimator.UpdateState(1);
            characterAnimator.UpdateDir(currentDir);
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
        if (dist > 1) return;
        else
        {
            if (Time.time - lastAttackTime >= 1f / attackSpeed)
            {
                lastAttackTime = Time.time;
                CurrentTarget.TakeDamage(config.AttackDamage);
                
                characterAnimator.UpdateState(2);
                characterAnimator.UpdateDir(currentDir);
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
