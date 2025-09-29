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

    public SelectionCard Card { get; private set; }
    public CardConfig CardConfig { get; private set; }

    public void SetCard(SelectionCard card)
    {
        Card = card;
        CardConfig = Configs.GetCardConfig(card.cardType);
        DisplayCardInfo();
    }

    private void DisplayCardInfo()
    {
        cardArt.sprite = ResourceProvider.GetCardArt(Card.cardType);
        cardName.text = Card.cardType.ToString();
        energy.text = CardConfig.GetCost().ToString();
    }
}
