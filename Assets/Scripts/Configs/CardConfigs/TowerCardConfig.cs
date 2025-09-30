using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerCardConfig", menuName = "Config/Card/Tower")]
public class TowerCardConfig : CardConfig
{
    [SerializeField] TowerEnum tower;

    public TowerEnum Tower => tower;

    public override void ApplySellectedEffect(Game game)
    {
        // change state to build tower
    }
}
