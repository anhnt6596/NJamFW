using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIBar : MonoBehaviour
{
    [SerializeField] SelectingCardBar selectingCardBar;
    [SerializeField] PlayingCardBar playingCardBar;
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
        selectingCardBar.gameObject.SetActive(state == InputStateEnum.SelectingCard);
        playingCardBar.gameObject.SetActive(state == InputStateEnum.PlayCard);
        if (state == InputStateEnum.PlayCard)
        {
            playingCardBar.Display(game);
        }
    }
}
