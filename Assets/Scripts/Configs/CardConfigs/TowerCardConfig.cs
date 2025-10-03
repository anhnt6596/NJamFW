using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerCardConfig", menuName = "Config/Card/Tower")]
public class TowerCardConfig : CardConfig
{
    [SerializeField] TowerEnum tower;
    public TowerEnum Tower => tower;
    
    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.BuildingTower;
    }
}
