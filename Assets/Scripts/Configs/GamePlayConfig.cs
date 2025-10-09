using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePlayConfig", menuName = "Config/GamePlay")]
public class GamePlayConfig : ScriptableObject
{
    [SerializeField] int baseHealth = 10;
    [SerializeField] double baseEnergyPerSec = 0.25f;
    [SerializeField] double maxEnergy = 10f;
    [SerializeField] double rerollCardCost = 1f;
    [SerializeField] int selectionCardNumber = 3;
    [SerializeField] int maxLockedCard = 1;
    [SerializeField] float[] damageResistances =
    {
        0, 0.25f, 0.5f, 0.75f, 1f
    };

    public int BaseHealth => baseHealth;
    public double BaseEnergyPerSec => baseEnergyPerSec;
    public double MaxEnergy => maxEnergy;
    public double RerollCardCost => rerollCardCost;
    public int SelectionCardNumber => selectionCardNumber;
    public int MaxLockedCard => maxLockedCard;
    public float GetDmgRes(short defPower) => damageResistances[(int)defPower];
}
