using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private EnemyInfoUI enemyInfoUI;
    [SerializeField] private Image arrowFocus;

    private void Awake()
    {
        display.SetActive(false);
        arrowFocus.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Game.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        Game.OnPhaseChanged -= OnPhaseChanged;
    }

    private void OnPhaseChanged(int turnIdx, TurnPhaseEnum phase)
    {
        if (phase != TurnPhaseEnum.Prepare)
        {
            display.SetActive(false);
            return;
        }
        else
        {
            display.SetActive(true);
            turnText.text = $"Turn {turnIdx + 1}";
            DisplayEnemyInfo();
        }
    }

    private void DisplayEnemyInfo()
    {
        Game runningGame = App.Get<GameManager>().RunningGame;
        var config = Configs.GetLevelConfig(runningGame.Level).GetTurnConfig(runningGame.CurrentTurn);
        var enemyInfo = GamePlayUtils.GetEnemyInfoFromConfig(config);
        enemyInfoUI.Display(enemyInfo);
    }

    public void OnClickStart()
    {
        App.Get<GameManager>().RunningGame.ReadyForTurn();
        HideArrow();
    }

    private float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            if (CheckShowArrow()) ShowArrow();
        }
    }

    [Header("Arrow config")]
    [SerializeField] private float moveDistance = 30f;
    [SerializeField] private float moveDuration = 0.5f;

    private bool CheckShowArrow()
    {
        if (arrowFocus.gameObject.activeSelf) return false;
        var game = App.Get<GameManager>().RunningGame;
        if (game.TurnPhase != TurnPhaseEnum.Prepare) return false;
        if (game.InputStateEnum != InputStateEnum.SelectingCard) return false;
        else
        {
            var cards = game.State.cards;
            foreach (var card in cards)
            {
                if (game.State.energy >= game.GetCardCost(card)) return false;
            }
        }
        return true;
    }

    private Tweener moveTween;
    private Tweener fadeTween;

    private void ShowArrow()
    {
        arrowFocus.gameObject.SetActive(true);

        moveTween?.Kill();
        fadeTween?.Kill();

        arrowFocus.SetAlpha(0);
        fadeTween = arrowFocus.DOFade(1f, 0.3f);

        moveTween = arrowFocus.transform
            .DOLocalMoveY(arrowFocus.transform.localPosition.y - moveDistance, moveDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void HideArrow()
    {
        fadeTween?.Kill();

        fadeTween = arrowFocus.DOFade(0f, 0.1f).OnComplete(() =>
        {
            moveTween?.Kill();
            arrowFocus.gameObject.SetActive(false);
        });
    }
}
