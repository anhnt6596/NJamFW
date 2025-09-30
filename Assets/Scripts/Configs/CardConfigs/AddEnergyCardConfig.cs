using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AddXEnergy", menuName = "Config/Card/AddEnergy")]
public class AddEnergyCardConfig : CardConfig
{
    [SerializeField] float value;

    public float Value => value;

    public override void ApplySellectedEffect(Game game)
    {
        game.IncreaseEnergy(value);
    }
}
