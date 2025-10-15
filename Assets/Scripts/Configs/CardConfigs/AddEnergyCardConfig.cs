using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AddXEnergy", menuName = "Config/Card/AddEnergy")]
public class AddEnergyCardConfig : CardConfig, ICardPlayingInstantly
{
    [SerializeField] double value;
    [SerializeField] int appearAtTotalRoll;

    public double Value => value;

    public override void ApplyCardEffect(Game game)
    {
        SoundManager.Play(ResourceProvider.Sound.combat.gain);
        game.IncreaseEnergy(value);
    }

    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < appearAtTotalRoll) return false;
        //if (game.State.energyFloor > GetCost(game)) return false;
        return base.CanBeRoll(game);
    }
}
