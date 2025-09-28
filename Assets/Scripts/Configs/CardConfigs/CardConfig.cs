using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardConfig : ScriptableObject
{
    [SerializeField] CardEnum card;
    [SerializeField] int cost = 1;
    [SerializeField] int escalatingCost = 0;
    [SerializeField] bool isUse = true;

    public CardEnum Card => card;
    public int GetCost(int useTime = 0) => cost + useTime * escalatingCost;
    public bool IsUse => isUse;  // bien tam thoi, neu false thi la config khong duoc su dung
}
