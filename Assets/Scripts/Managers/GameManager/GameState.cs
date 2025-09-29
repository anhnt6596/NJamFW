using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Game/State")]
public class GameState : ScriptableObject
{
    public float energy;
    public List<CardEnum> cards;
    public List<int> lockedCardIdxs;
    public int selectedCardIdx;

    public void Reset()
    {
        energy = 0;
        cards = new List<CardEnum>();
        lockedCardIdxs = new List<int>();
        selectedCardIdx = -1;
    }
}
