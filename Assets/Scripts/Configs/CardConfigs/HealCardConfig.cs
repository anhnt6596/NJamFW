using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Config/Card/Heal")]
public class HealCardConfig : CardConfig, ICardPlayingInstantly
{
    [SerializeField] int value;

    public int Value => value;

    public override void ApplyCardEffect(Game game)
    {
        SoundManager.Play(ResourceProvider.Sound.combat.gain);
        game.Heal(Value);
    }

    public override bool CanBeRoll(Game game)
    {
        if (game.State.baseHealth + Value > Configs.GamePlay.BaseHealth) return false;
        return base.CanBeRoll(game);
    }
}
