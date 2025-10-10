using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Config/Card/Heal")]
public class HEalCardConfig : CardConfig
{
    [SerializeField] int value;

    public int Value => value;

    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        SoundManager.Play(ResourceProvider.Sound.combat.gain);

        game.Heal(Value);
        return InputStateEnum.SelectingCard;
    }

    public override bool CanBeRoll(Game game)
    {
        if (game.State.baseHealth + Value > Configs.GamePlay.BaseHealth) return false;
        return base.CanBeRoll(game);
    }
}
