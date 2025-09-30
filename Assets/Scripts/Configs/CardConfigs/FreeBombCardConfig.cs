using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FreeBomb", menuName = "Config/Card/FreeBomb")]
public class FreeBombCardConfig : CardConfig
{
    public override void ApplySellectedEffect(Game game)
    {
        // change state to build tower
    }
}
