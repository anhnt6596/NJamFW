using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Config/Card/Bomb")]
public class BombCardConfig : CardConfig, ICardPlayingAnywhere
{
    [SerializeField] Damage damage;
    [SerializeField] Vector2 radius;

    public Damage Damage => damage;
    public Vector2 Radius => radius;


    public Vector3 WPos { get; set; }

    public override int GetCost(Game game)
    {
        if (game.State.selectedCards.Contains(CardEnum.FreeBomb)) return 0;
        return base.GetCost(game);
    }

    public override void ApplyCardEffect(Game game)
    {
        game.GamePlay.DropBomb(WPos, Damage, Radius);
    }

    public override string GetPlayDescription(Game game)
    {
        var cardInfo = Configs.GetCardInfo(Card);
        return cardInfo.PlayDescription.Replace("@name#", cardInfo.DisplayName);
    }
}
