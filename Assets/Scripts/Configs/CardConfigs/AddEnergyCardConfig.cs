using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AddXEnergy", menuName = "Config/Card/AddEnergy")]
public class AddEnergyCardConfig : CardConfig
{
    [SerializeField] float value;
    [SerializeField] int appearAtTotalRoll;

    public float Value => value;

    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        game.IncreaseEnergy(value);
        return InputStateEnum.None;
    }

    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < appearAtTotalRoll) return false;
        if (game.State.energyFloor > GetCost(game)) return false;
        return base.CanBeRoll(game);
    }
}
