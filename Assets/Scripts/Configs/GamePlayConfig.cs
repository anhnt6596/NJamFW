using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePlayConfig", menuName = "Config/GamePlay")]
public class GamePlayConfig : ScriptableObject
{
    [SerializeField] float baseEnergyPerSec = 0.1f;
    [SerializeField] float maxEnergy = 10f;

    public float BaseEnergyPerSec => baseEnergyPerSec;
    public float MaxEnergy => maxEnergy;
}
