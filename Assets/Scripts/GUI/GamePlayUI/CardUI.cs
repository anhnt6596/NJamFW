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

    public int Index { get; private set; }
    public CardEnum Card { get; private set; }
    public CardConfig CardConfig { get; private set; }

    public void SetCard(int index, CardEnum card)
    {
        game = App.Get<GameManager>().RunningGame;
        Index = index;
        Card = card;
        CardConfig = Configs.GetCardConfig(card);
        DisplayCardInfo();
        DisplayLock();
    }

    private void DisplayCardInfo()
    {
        cardArt.sprite = ResourceProvider.GetCardArt(Card);
        cardName.text = Card.ToString();
        energy.text = CardConfig.GetCost().ToString();
    }

    public void DisplayLock()
    {
        bool isMaxLocked = game.IsMaxLockedCard;
        if (isMaxLocked)
        {
            if (game.State.lockedCardIdxs.Contains(Index))
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
        game.DoLockCard(Index);
    }
}
