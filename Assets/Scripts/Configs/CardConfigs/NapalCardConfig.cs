using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Napalm", menuName = "Config/Card/Napalm")]
public class NapalmCardConfig : CardConfig, ICardPlayingAnywhere
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

    public Vector3 WPos { get; set; }
    public override void ApplyCardEffect(Game game)
    {
        game.GamePlay.DropNapalm(WPos, FireNumber, Radius, InstantlyDamage, DamageInterval, DamagePerSec, EachRadius);
    }


    public override string GetPlayDescription(Game game) => Configs.GetCardInfo(Card).PlayDescription;
}
