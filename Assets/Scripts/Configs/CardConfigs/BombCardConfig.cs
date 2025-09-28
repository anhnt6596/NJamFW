using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Config/Card/Bomb")]
public class BombCardConfig : CardConfig
{
    [SerializeField] float damageValue;
    [SerializeField] DamageEnum damageType;

    public float Damage => damageValue;
    public DamageEnum DamageType => damageType;
}
