using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DarkHole", menuName = "Config/Card/DarkHole")]
public class DarkHoleConfig : CardConfig
{
    [SerializeField] float damage = 500;
    [SerializeField] DamageEnum damageType = DamageEnum.True;
    [SerializeField] int appearAtTotalRoll = 15;

    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < appearAtTotalRoll) return false;
        return base.CanBeRoll(game);
    }
}
