using UnityEngine;

[CreateAssetMenu(fileName = "AnEnemy", menuName = "Config/Enemy")]
public class EnemyConfig : UnitConfig
{
    [SerializeField] private EnemyEnum type;
    [SerializeField] private int damageToBase = 1;
    [SerializeField] private float deathEnergy = 0;

    public EnemyEnum EnemyType => type;
    public int DamageToBase => damageToBase;
    public float DeathEnergy => deathEnergy;
}