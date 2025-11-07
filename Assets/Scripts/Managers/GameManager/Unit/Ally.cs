using UnityEngine;

public class Ally : Unit
{
    private AllyConfig config;
    public override float speed => config.Speed;
    public override float maxHP => config.Hp;
    public override float attackRange => config.AttackRange;
    public override float attackSpeed => config.AttackSpeed;

    public Vector3 AnchorPos { get; private set;}
    public Enemy CurrentTarget { get; set; }
    private float lastAttackTime;
    private float lastTakeDamageTime;
    private float outOfCombatRegenTime = 2f;

    private IGameField gameField;
    private enum State { Search, Combat }
    private State state = State.Search;

    public void Setup(IGameField level, AllyConfig config, Vector3 anchorPos)
    {
        this.gameField = level;
        this.config = config;
        HP = config.Hp;
        def = config.Def;
        statusList = new();
        CurrentTarget = null;
        AnchorPos = anchorPos;

        transform.position = AnchorPos;
        unitAnimator.UpdateDir(1);
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

        CheckRegen();
    }

    void SearchForTarget()
    {
        unitAnimator.UpdateState(0);
        Enemy nearestEnemy = gameField.EnemySpatialHash.GetNearest<Enemy>(transform.position, config.DetectionRadius);

        if (nearestEnemy != null)
        {
            CurrentTarget = nearestEnemy;
            nearestEnemy.SetTarget(this);
            state = State.Combat;
            lastAttackTime = Time.time;
        }
        else if (Vector3.Magnitude(transform.position - AnchorPos) > 0.1f)
        {
            MoveTo(Vector3.MoveTowards(
                transform.position,
                AnchorPos,
                config.Speed * Time.deltaTime
            ));
        }
    }

    bool hasAttackThisCycle = false;

    void CombatBehavior()
    {
        if (CurrentTarget == null || CurrentTarget.isDead)
        {
            CurrentTarget = null;
            state = State.Search;
            return;
        }

        float totalAttackRange = attackRange + CurrentTarget.attackRange;
        bool inRange = GamePlayUtils.IsInRange(transform.position, CurrentTarget.transform.position, totalAttackRange);

        if (inRange)
        {
            if (unitAnimator.State == 1) unitAnimator.UpdateState(0);
            var remainAttackTime = Time.time - lastAttackTime;
            var shootCycle = 1f / attackSpeed;

            if (remainAttackTime > shootCycle * 0.85f && !hasAttackThisCycle)
            {
                SoundManager.Play(ResourceProvider.Sound.combat.sword);
                unitAnimator.TriggerAttack();
                hasAttackThisCycle = true;
            }

            if (remainAttackTime >= shootCycle)
            {
                lastAttackTime = Time.time;
                CurrentTarget.TakeDamage(config.AttackDamage);
                hasAttackThisCycle = false;
            }
            var dir = MovingUtils.GetDirection2Index(CurrentTarget.transform.position - transform.position, Camera.main.transform);
            if (dir != Dir2.Unknown) unitAnimator.UpdateDir((int)dir);
        }
        else
        {
            MoveTo(Vector3.MoveTowards(
                transform.position,
                CurrentTarget.transform.position,
                config.Speed * Time.deltaTime
            ));
            lastAttackTime = Time.time;
        }
    }

    private void MoveTo(Vector3 nextPos)
    {
        var dir = MovingUtils.GetDirection2Index(nextPos - transform.position, Camera.main.transform);
        if (dir != Dir2.Unknown) unitAnimator.UpdateDir((int)dir);
        unitAnimator.UpdateState(1);
        transform.position = nextPos;
    }

    public override void TakeDamage(Damage dmgInput)
    {
        lastTakeDamageTime = Time.time;
        base.TakeDamage(dmgInput);
    }

    private void CheckRegen()
    {
        if (Time.time - lastTakeDamageTime > outOfCombatRegenTime)
        {
            Heal(Time.deltaTime * config.HealRegen);
        }
    }

    private void OnEnable()
    {
        Game.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        Game.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnPhaseChanged(int turn, TurnPhaseEnum phase)
    {
        if (phase == TurnPhaseEnum.Prepare) Heal(config.Hp);
    }

    void OnDrawGizmosSelected()
    {
        if (!config) return;
        Gizmos.color = Color.yellow;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(config.AttackRange * 2, 1, config.AttackRange * 2));
        Gizmos.matrix = matrix;
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        Matrix4x4 matrix2 = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(config.DetectionRadius * 2, 1, config.DetectionRadius * 2));
        Gizmos.matrix = matrix2;
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
