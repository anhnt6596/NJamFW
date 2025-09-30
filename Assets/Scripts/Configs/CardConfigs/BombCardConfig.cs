using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Config/Card/Bomb")]
public class BombCardConfig : CardConfig
{
    [SerializeField] float damageValue;
    [SerializeField] DamageEnum damageType;

    public float Damage => damageValue;
    public DamageEnum DamageType => damageType;

    public override int GetCost(Game game)
    {
        if (game.State.selectedCards.Contains(CardEnum.FreeBomb)) return 0;
        return base.GetCost(game);
    }

    public override void ApplySellectedEffect(Game game)
    {
        // change state to bomb
    }
}
