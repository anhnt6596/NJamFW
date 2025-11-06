using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "On Tower Config", menuName = "Config/OnTower")]
public class OnTowerAttackConfig : ScriptableObject
{
    [SerializeField] private OnTowerEnum type;
    [SerializeField] private Damage baseAttack;
    [SerializeField] private float fireRate;
    [SerializeField] private float range;

    public OnTowerEnum Type => type;
    public Damage BaseAttack => baseAttack;
    public float FireRate => fireRate;
    public float Range => range;
    public Damage GetAttackByLevel(int level) => baseAttack + baseAttack * (level - 1) * 1.1f;
}
