using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBar : MonoBehaviour
{
    [SerializeField] List<CardUI> cardUIs;
    [SerializeField] Button rerollButton;

    Game game;

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;

        game.OnCardRolled += OnCardRolled;
        DisplayCards();
    }

    private void OnDisable()
    {
        game.OnCardRolled -= OnCardRolled;
    }
    private void OnCardRolled() => DisplayCards();

    private void DisplayCards()
    {
        var cards = game.SelectionCards.Cards;
        for (int i = 0; i < cardUIs.Count; i++)
        {
            if (i < cards.Count)
            {
                cardUIs[i].SetCard(cards[i]);
                cardUIs[i].gameObject.SetActive(true);
            }
            else
            {
                cardUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisplaySelectableCards()
    {

    }

    private void Update()
    {
        DisplayRerollButtonStatus();
    }

    private void DisplayRerollButtonStatus()
    {
        rerollButton.interactable =
            game.IsRunning &&
            game.State.energy >= Configs.GamePlay.RerollCardCost;
    }

    public void OnClickReroll()
    {
        game.PayToRerollCards();
    }
}
