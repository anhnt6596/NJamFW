using UnityEngine;

[CreateAssetMenu(fileName = "AnAlly", menuName = "Config/Ally")]
public class AllyConfig : UnitConfig
{
    [SerializeField] private AllyEnum type;
    [SerializeField] private Vector2 detectionRadius;
    public AllyEnum AllyType => type;
    public Vector2 DetectionRadius => detectionRadius;
}