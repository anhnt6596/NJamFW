using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public Game(int level, GameState state)
    {
        Level = level;
        this.State = state;
        gameConfig = Configs.GamePlay;
    }
    public GameState State { get; private set; }
    private GamePlayConfig gameConfig;
    public int Level { get; }
    public bool IsRunning { get; private set; } = false;

    public void StartGame()
    {
        IsRunning = true;
    }

    public void Update()
    {
        if (IsRunning) UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        IncreaseEnergy(gameConfig.BaseEnergyPerSec * Time.deltaTime);
    }

    private void IncreaseEnergy(float value)
    {
        State.energy = Mathf.Clamp(State.energy + value, 0, gameConfig.MaxEnergy);
    }
}
