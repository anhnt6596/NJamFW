using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageCardInfo : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private CardInfoIcon cardPrefab;

    private readonly List<CardInfoIcon> spawnedCards = new();

    public void ShowCards(CardInfoPopup popup, List<CardEnum> cardList)
    {
        foreach (var card in spawnedCards) Destroy(card.gameObject);
        spawnedCards.Clear();

        foreach (var data in cardList)
        {
            var card = Instantiate(cardPrefab, contentParent);
            card.SetData(data, type => popup.DisplayCardInfo(type));
            spawnedCards.Add(card);
        }
    }
}