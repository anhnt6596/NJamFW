using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LightningPower", menuName = "Config/Card/Lightning Power")]
public class LightningPowerConfig : CardConfig, ILightningPowerModifier, ICardPlayingInstantly
{
    [SerializeField] float damageMult = 2;
    [SerializeField] float lightningUsedToAppear = 5;

    public override bool CanBeRoll(Game game)
    {
        if (game.State.selectedCards.Count(c => c == CardEnum.Lightning) < lightningUsedToAppear) return false;
        return base.CanBeRoll(game);
    }

    public override string GetDetailInfo(Game game)
    {
        return $"x{damageMult}DMG";
    }

    public float ModifyLightningDamageEnergy(float baseDmg)
    {
        return baseDmg * damageMult;
    }
}
