using UnityEngine;

[CreateAssetMenu(fileName = "AnAlly", menuName = "Config/Ally")]
public class AllyConfig : UnitConfig
{
    [SerializeField] private AllyEnum type;
    [SerializeField] private float detectionRadius = 2;
    [SerializeField] private float healRegen;
    public AllyEnum AllyType => type;
    public float DetectionRadius => detectionRadius;
    public float HealRegen => healRegen;
}