using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FreeBomb", menuName = "Config/Card/FreeBomb")]
public class FreeBombCardConfig : CardConfig
{
    [SerializeField] int bombUsed = 2;
    public override bool CanBeRoll(Game game)
    {
        if (game.State.selectedCards.Count(c => c == CardEnum.Bomb) < bombUsed) return false;
        return base.CanBeRoll(game);
    }
}
