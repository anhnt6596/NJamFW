using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Game/State")]
public class GameState : ScriptableObject
{
    public float energy;
    public List<CardEnum> cards;
    public List<int> lockedCardIdxs;
    public int selectingCardIdx;
    public List<CardEnum> selectedCards;
    public int freeRoll;

    public void Reset()
    {
        energy = 0;
        cards = new List<CardEnum>();
        lockedCardIdxs = new List<int>();
        selectingCardIdx = -1;
        selectedCards = new List<CardEnum>();
        freeRoll = 0;
    }

    public CardEnum selectCard => cards.GetOrDefault(selectingCardIdx);
}
