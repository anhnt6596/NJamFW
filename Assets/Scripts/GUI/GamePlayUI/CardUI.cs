using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] Image cardArt;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI energy;
    [SerializeField] Button lockButton;
    [SerializeField] Image lockIcon;
    Game game;

    public SelectionCard Card { get; private set; }
    public CardConfig CardConfig { get; private set; }

    public void SetCard(SelectionCard card)
    {
        game = App.Get<GameManager>().RunningGame;
        Card = card;
        CardConfig = Configs.GetCardConfig(card.cardType);
        DisplayCardInfo();
        DisplayLockInfo();
    }

    private void OnEnable()
    {
        Game.OnCardLocked += OnCardLocked;
    }

    private void OnDisable()
    {
        Game.OnCardLocked -= OnCardLocked;
    }

    private void OnCardLocked(SelectionCard card)
    {
        DisplayLockInfo();
    }

    private void DisplayCardInfo()
    {
        cardArt.sprite = ResourceProvider.GetCardArt(Card.cardType);
        cardName.text = Card.cardType.ToString();
        energy.text = CardConfig.GetCost().ToString();
    }

    public void DisplayLockInfo()
    {
        bool hasLockedCard = game.GetLockedCard() != null;
        if (hasLockedCard)
        {
            if (Card.isLocked)
            {
                lockButton.gameObject.SetActive(true);
                lockIcon.sprite = ResourceProvider.Icon.locked;
            }
            else
            {
                lockButton.gameObject.SetActive(false);
            }
        }
        else
        {
            lockButton.gameObject.SetActive(true);
            lockIcon.sprite = ResourceProvider.Icon.unlocked;
        }
    }

    public void OnClickLock()
    {
        game.DoLockCard(Card);
    }
}
