using UnityEngine;

[CreateAssetMenu(fileName = "AnEnemy", menuName = "Config/Enemy")]
public class EnemyConfig : UnitConfig
{
    [SerializeField] private EnemyEnum type;
    [SerializeField] private float damageToTower;

    public EnemyEnum EnemyType => type;
    public float DamageToTower => damageToTower;
}