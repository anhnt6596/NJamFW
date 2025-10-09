using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] Image cardArt;
    [SerializeField] TextMeshProUGUI cardName, energy, detail;
    [SerializeField] GameObject energyBg;
    [SerializeField] Button lockButton;
    [SerializeField] Image lockIcon;
    [SerializeField] Image energyLoadImg;
    [SerializeField] GameObject backFace;
    Game game;

    public int Index { get; private set; }
    public CardEnum Card { get; private set; }
    public CardConfig CardConfig { get; private set; }
    public bool Interactable { get; set; } = false;

    public void SetCard(int index, CardEnum card)
    {
        game = App.Get<GameManager>().RunningGame;
        Index = index;
        Card = card;
        CardConfig = Configs.GetCardConfig(card);
    }

    public void DisplayAll()
    {
        DisplayCardInfo();
        DisplayLock();
    }

    private void DisplayCardInfo()
    {
        cardArt.sprite = ResourceProvider.GetCardArt(Card);
        cardName.text = Card.ToString();
        detail.text = CardConfig.GetDetailInfo(game);
        var cost = game.GetCardCost(Card);
        energy.text = cost.ToString();
        energyBg.SetActive(cost > 0);
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

    private bool isFlipping = false;
    private bool cardDisplayed = true;
    private Tween progressTween;

    public void Flip(Action callback = null)
    {
        if (game.State.lockedCardIdxs.Contains(Index))
        {
            DisplayAll();
            callback?.Invoke();
            return;
        }

        if (isFlipping) return;
        isFlipping = true;
        cardDisplayed = false;
        float progress = 0;

        progressTween = DOTween.To(() => 0f, x => progress = x, 1f, 0.8f)
            .SetEase(Ease.OutQuart)
            .OnUpdate(UpdateFlip)
            .OnComplete(() =>
            {
                transform.localEulerAngles = Vector3.zero;

                isFlipping = false;
                progressTween = null;
                callback?.Invoke();
            });

        void UpdateFlip()
        {
            float p = progress;

            if (!cardDisplayed && p >= 0.5f)
            {
                cardDisplayed = true;
                DisplayAll();
            }

            float y = p <= 0.5f
                ? Mathf.Lerp(0f, 90f, p * 2f)
                : Mathf.Lerp(-90f, 0f, (p - 0.5f) * 2f);
            transform.localEulerAngles = new Vector3(0f, y, 0f);
        }
    }

    public void OnClickLock()
    {
        game.DoLockCard(Index);
    }

    public void OnClickSelectCard()
    {
        if (!Interactable) return;
        Debug.Log($"Card Selected | idx {Index} | type {Card}");
        if (game.State.energy >= CardConfig.GetCost(game)) game.DoSelectCard(Index);
    }

    private void Update()
    {
        if (game == null || !cardDisplayed) return;
        //if (game.) neu trang thai game la dang pick card thi oke
        var fillValue = Mathf.Clamp01(1 - game.State.energy / game.GetCardCost(Card));
        var last = energyLoadImg.fillAmount;
        if (fillValue < last && fillValue != 1)
            energyLoadImg.fillAmount = Mathf.Lerp(last, fillValue, 0.1f);
        else
            energyLoadImg.fillAmount = fillValue;

    }
}
