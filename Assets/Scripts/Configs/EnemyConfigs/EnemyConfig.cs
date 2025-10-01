using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Config/Enemy")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private EnemyEnum enemyEnum;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damageToTower;
    [SerializeField] private float armor;
    [SerializeField] private float magicRes;

    public float Hp => hp;

    public float Speed => speed;

    public float DamageToTower => damageToTower;

    public float Armor => armor;

    public float MagicRes => magicRes;

    public EnemyEnum EnemyEnum => enemyEnum;
}