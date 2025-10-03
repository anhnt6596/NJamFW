using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIBar : MonoBehaviour
{
    [SerializeField] CardBar cardBar;
    [SerializeField] BombBar bombBar;
    [SerializeField] GameObject towerBar;
    Game game;

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
        Game.OnInputStateChanged += DisplayGameBars;
        DisplayGameBars(game.InputStateEnum);
    }

    private void OnDisable()
    {
        Game.OnInputStateChanged -= DisplayGameBars;
    }

    private void DisplayGameBars(InputStateEnum state)
    {
        cardBar?.gameObject.SetActive(state == InputStateEnum.SelectingCard);
        bombBar?.gameObject.SetActive(state == InputStateEnum.PlacingBomb);
        //towerBar?.gameObject.SetActive(state == InputStateEnum.BuildingTower);
    }
}
