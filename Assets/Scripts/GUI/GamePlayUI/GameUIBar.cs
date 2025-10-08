using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIBar : MonoBehaviour
{
    [SerializeField] GameObject waiting;
    [SerializeField] GameObject playing;

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
        waiting.SetActive(state == InputStateEnum.None);
        playing.SetActive(state != InputStateEnum.None);

        if (state != InputStateEnum.None)
        {
            selectingCardBar.gameObject.SetActive(state == InputStateEnum.SelectingCard);
            playingCardBar.gameObject.SetActive(state == InputStateEnum.PlayCard);
            if (state == InputStateEnum.PlayCard)
            {
                playingCardBar.Display(game);
            }
        }
    }
}
