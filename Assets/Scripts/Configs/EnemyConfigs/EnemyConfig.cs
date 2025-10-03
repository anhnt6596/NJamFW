using UnityEngine;

[CreateAssetMenu(fileName = "AnEnemy", menuName = "Config/Enemy")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private EnemyEnum type;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damageToTower;
    [SerializeField] private DeffenseStats def;

    public EnemyEnum Type => type;
    public float Hp => hp;
    public float Speed => speed;
    public float DamageToTower => damageToTower;
    public DeffenseStats Def => def;
}