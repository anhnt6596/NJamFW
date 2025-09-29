using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePlayConfig", menuName = "Config/GamePlay")]
public class GamePlayConfig : ScriptableObject
{
    [SerializeField] float baseEnergyPerSec = 0.1f;
    [SerializeField] float maxEnergy = 10f;
    [SerializeField] float rerollCardCost = 1f;
    [SerializeField] int selectionCardNumber = 3;

    public float BaseEnergyPerSec => baseEnergyPerSec;
    public float MaxEnergy => maxEnergy;
    public float RerollCardCost => rerollCardCost;
    public int SelectionCardNumber => selectionCardNumber;
}
