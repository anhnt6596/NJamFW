using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectingCardBar : MonoBehaviour
{
    [SerializeField] List<CardUI> cardUIs;

    Game game;

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;

        Game.OnCardsRolled += OnCardRolled;
        Game.OnCardLocked += OnCardLocked;
        DisplayCards();
    }

    private void OnDisable()
    {
        Game.OnCardsRolled -= OnCardRolled;
        Game.OnCardLocked -= OnCardLocked;
    }
    private void OnCardRolled() => DisplayCards();

    private void OnCardLocked()
    {
        cardUIs.ForEach(c => c.DisplayLock());
    }


    private void DisplayCards()
    {
        var cards = game.State.cards;
        foreach (var cardUI in cardUIs) cardUI.Interactable = false;

        for (int i = 0; i < cardUIs.Count; i++)
        {
            var cardUI = cardUIs[i];
            if (i < cards.Count)
            {
                var card = cards[i];
                cardUI.SetCard(i, cards[i]);
                cardUI.gameObject.SetActive(true);
                cardUI.Flip(() => cardUI.Interactable = true);
            }
            else
            {
                cardUI.gameObject.SetActive(false);
            }
        }
    }
}
