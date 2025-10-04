using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Config/Card/Bomb")]
public class BombCardConfig : CardConfig
{
    [SerializeField] Damage damage;
    [SerializeField] Vector2 radius;

    public Damage Damage => damage;
    public Vector2 Radius => radius;

    public override int GetCost(Game game)
    {
        if (game.State.selectedCards.Contains(CardEnum.FreeBomb)) return 0;
        return base.GetCost(game);
    }

    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.PlayCard;
    }

    public override string GetPlayDescription(Game game) =>  "Tap to drop Bomb";
}
