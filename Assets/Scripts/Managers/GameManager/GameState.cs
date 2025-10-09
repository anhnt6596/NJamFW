using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Game/State")]
public class GameState : ScriptableObject
{
    public int baseHealth;
    public double energy;
    public List<CardEnum> cards;
    public List<int> lockedCardIdxs;
    public int selectingCardIdx;
    public List<CardEnum> selectedCards;
    public int freeRoll;
    public int autoRolled;
    public int proactiveRolled;

    public void InitialState()
    {

    }

    public void Reset()
    {
        baseHealth = Configs.GamePlay.BaseHealth;
        energy = 0;
        cards = new List<CardEnum>();
        lockedCardIdxs = new List<int>();
        selectingCardIdx = -1;
        selectedCards = new List<CardEnum>();
        freeRoll = 0;
        energy = 0;
        autoRolled = 0;
        proactiveRolled = 0;
    }
    public int energyFloor => Mathf.FloorToInt((float)energy);
    public int totalRolled => autoRolled + proactiveRolled;

}
