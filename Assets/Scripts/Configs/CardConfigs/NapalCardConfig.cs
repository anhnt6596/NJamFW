using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Napalm", menuName = "Config/Card/Napalm")]
public class NapalmCardConfig : CardConfig
{
    [SerializeField] float damagePerSec = 20;
    [SerializeField] float damageInterval = 6;
    [SerializeField] int fireNumber = 10;
    [SerializeField] Vector2 radius;
    [SerializeField] Vector2 eachRadius;
    [SerializeField] Damage instantlyDamage;

    public float DamagePerSec => damagePerSec;
    public float DamageInterval => damageInterval;
    public int FireNumber => fireNumber;
    public Vector2 Radius => radius;
    public Vector2 EachRadius => eachRadius;
    public Damage InstantlyDamage => instantlyDamage;

    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.PlayCard;
    }

    public override string GetPlayDescription(Game game) => "Tap to drop Fire Rain";
}
