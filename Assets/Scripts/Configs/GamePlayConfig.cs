using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePlayConfig", menuName = "Config/GamePlay")]
public class GamePlayConfig : ScriptableObject
{
    [SerializeField] int baseHealth = 10;
    [SerializeField] float baseEnergyPerSec = 0.1f;
    [SerializeField] float maxEnergy = 10f;
    [SerializeField] float rerollCardCost = 1f;
    [SerializeField] int initEnergy = 2;
    [SerializeField] int initFreeReroll = 1;
    [SerializeField] int selectionCardNumber = 3;
    [SerializeField] int maxLockedCard = 1;
    [SerializeField] float[] damageResistances =
    {
        0, 0.25f, 0.5f, 0.75f, 1f
    };

    public int BaseHealth => baseHealth;
    public float BaseEnergyPerSec => baseEnergyPerSec;
    public float MaxEnergy => maxEnergy;
    public float RerollCardCost => rerollCardCost;
    public int InitEnergy => initEnergy;
    public int InitFreeReroll => initFreeReroll;
    public int SelectionCardNumber => selectionCardNumber;
    public int MaxLockedCard => maxLockedCard;
    public float GetDmgRes(short defPower) => damageResistances[(int)defPower];
}
