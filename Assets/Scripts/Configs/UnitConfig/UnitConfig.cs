using UnityEngine;
public abstract class UnitConfig : ScriptableObject
{
    [SerializeField] private float hp;
    [SerializeField] private float speed = 1;
    [SerializeField] private DeffenseStats def;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float attackSpeed = 0.6f; // number attack in 1 second
    [SerializeField] private Damage attackDamage = new Damage(20, DamageEnum.Physical);

    public float Hp => hp;
    public float Speed => speed;
    public DeffenseStats Def => def;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public Damage AttackDamage => attackDamage;
}