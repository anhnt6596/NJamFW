using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AddXRoll", menuName = "Config/Card/Roll")]
public class AddFreeRollCardConfig : CardConfig, ICardPlayingInstantly
{
    [SerializeField] int value;

    public float Value => value;

    public override void ApplyCardEffect(Game game)
    {
        SoundManager.Play(ResourceProvider.Sound.combat.gain);
        game.State.freeRoll += value;
    }

    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < value) return false;
        return base.CanBeRoll(game);
    }
}
