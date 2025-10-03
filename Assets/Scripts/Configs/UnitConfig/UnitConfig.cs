using UnityEngine;
public abstract class UnitConfig : ScriptableObject
{
    [SerializeField] private float hp;
    [SerializeField] private float speed = 1;
    [SerializeField] private DeffenseStats def;
    [SerializeField] private Vector2 attackRange = new Vector2(1, 0.7f);
    [SerializeField] private float attackSpeed = 0.6f; // number attack in 1 second
    [SerializeField] private Damage attackDamage = new Damage(20, DamageEnum.Physical);

    public float Hp => hp;
    public float Speed => speed;
    public DeffenseStats Def => def;
    public Vector2 AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public Damage AttackDamage => attackDamage;
}