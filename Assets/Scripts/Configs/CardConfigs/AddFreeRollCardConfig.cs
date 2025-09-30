using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AddXRoll", menuName = "Config/Card/Roll")]
public class AddFreeRollCardConfig : CardConfig
{
    [SerializeField] int value;

    public float Value => value;

    public override void ApplySellectedEffect(Game game)
    {
        game.State.freeRoll += value;
    }
}
