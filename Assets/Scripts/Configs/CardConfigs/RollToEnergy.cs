using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RollToEnergy", menuName = "Config/Card/RollToEnergy")]
public class RollToEnergyCardConfig : CardConfig, IRollEnergyModifier
{
    [SerializeField] int requiredRoll = 5;
    [SerializeField] float energyEachRoll = 0.5f;

    public override string GetDetailInfo(Game game)
    {
        return $"+{energyEachRoll}";
    }

    public float ModifyRollEnergy(float baseRollEnergy)
    {
        return baseRollEnergy + energyEachRoll;
    }

    public override bool CanBeRoll(Game game)
    {
        if (game.State.proactiveRolled < requiredRoll) return false;
        return base.CanBeRoll(game);
    }
}
