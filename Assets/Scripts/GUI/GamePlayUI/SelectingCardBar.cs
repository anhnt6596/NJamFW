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
        for (int i = 0; i < cardUIs.Count; i++)
        {
            if (i < cards.Count)
            {
                cardUIs[i].SetCard(i, cards[i]);
                cardUIs[i].gameObject.SetActive(true);
            }
            else
            {
                cardUIs[i].gameObject.SetActive(false);
            }
        }
    }
}
