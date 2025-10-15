using Core;
using System;
using TMPro;
using UnityEngine;

public class PlayingCardBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDescription;

    Game game;
    private void OnEnable()
    {
        ActionService.Sub<TouchDownAction>(OnTouchDown);
    }

    private void OnDisable()
    {
        ActionService.Unsub<TouchDownAction>(OnTouchDown);
    }

    public void Display(Game game)
    {
        this.game = game;
        textDescription.text = Configs.GetCardConfig(game.PlayingCard).GetPlayDescription(game);

        CheckShowTut();
    }

    private void OnTouchDown(TouchDownAction obj)
    {
        Vector3 screenPos = obj.Finger.ScreenPosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));

        worldPos.z = 0;
        if (TryPlayCard(worldPos))
        {
            game.ApplyPlayingCard();
        }
        else
        {
            var guiEffectMgr = App.Get<GUIEffectManager>();
            guiEffectMgr.ShowInvalidEffect(worldPos, GUILayer.GUI);
        }
    }

    private bool TryPlayCard(Vector3 worldPos)
    {
        var card = Configs.GetCardConfig(game.PlayingCard);
        switch (card)
        {
            case ICardPlayingTowerPlace towerPlaceCard:
                return TryFindTowerPlacement(worldPos, towerPlaceCard);
            case ICardPlayingRoad roadPlaceCard:
                return TryPlaceRoad(worldPos, roadPlaceCard);
            case ICardPlayingAnywhere playAnywhereCard:
            {
                playAnywhereCard.WPos = worldPos;
                return true;
            } 
        }
        return true;
    }

    private bool TryFindTowerPlacement(Vector3 wPos, ICardPlayingTowerPlace card)
    {
        Debug.Log($"Check Can Place Tower {game.PlayingCard}");

        int placeIndex = -1;
        var canPlaceTower = game.GamePlay?.CheckPlaceTowerPosition(wPos, card.Tower, out placeIndex);
        if (canPlaceTower != null && canPlaceTower.Value)
        {
            card.PlacementIndex = placeIndex;
            return true;
        }
        return false;
    }

    private bool TryPlaceRoad(Vector3 wPos, ICardPlayingRoad card)
    {
        if (game.GamePlay.IsWPosInPolygon(wPos))
        {
            card.WPos = wPos;
            game.ApplyPlayingCard();
            return true;
        }
        return false;
    }

    public void OnClickCancel()
    {
        game.CancelPlayingCard();
    }

    // quick quick check show tut
    private void CheckShowTut()
    {
        var gameMgr = App.Get<GameManager>();
        switch (game.PlayingCard)
        {
            case CardEnum.ArcherTower:
            case CardEnum.MageTower:
            case CardEnum.ArtilleryTower:
                {
                    gameMgr.CheckShowTutPopup(1);
                    break;
                }
            case CardEnum.Troop:
            case CardEnum.TroopMed:
            case CardEnum.Mine:
                {
                    gameMgr.CheckShowTutPopup(2);
                    break;
                }
            case CardEnum.Lightning:
            case CardEnum.Bomb:
            case CardEnum.Napalm:
            case CardEnum.TimeReverse:
                {
                    gameMgr.CheckShowTutPopup(3);
                    break;
                }
        }
    }
}