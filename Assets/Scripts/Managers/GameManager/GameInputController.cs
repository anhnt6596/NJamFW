using Core;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameInputController : MonoBehaviour
{
    Game game;
    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
        ActionService.Sub<TapAction>(OnTap);
        ActionService.Sub<MouseUpdateAction>(OnMouseMoved);
    }

    private void OnDisable()
    {
        ActionService.Unsub<TapAction>(OnTap);
        ActionService.Unsub<MouseUpdateAction>(OnMouseMoved);
    }

    private void OnTap(TapAction obj)
    {
        if (game.InputStateEnum != InputStateEnum.PlayCard) return;
        Vector3 screenPos = obj.Finger.ScreenPosition;
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Vector3 wPos = MathUtils.RaycastToXZPlane(ray.origin, ray.direction);
        if (TryPlayCard(wPos))
        {
            game.ApplyPlayingCard();
        }
        else
        {
            var guiEffectMgr = App.Get<GUIEffectManager>();
            guiEffectMgr.ShowInvalidEffect(wPos, GUILayer.GUI);
        }
    }

    private bool TryPlayCard(Vector3 worldPos)
    {
        var card = Configs.GetCardConfig(game.PlayingCard);
        switch (card)
        {
            case ICardPlayingTilePlace tilePlaceCard:
                return TryFindTile(worldPos, tilePlaceCard);
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

    private bool TryFindTile(Vector3 wPos, ICardPlayingTilePlace tile)
    {
        if (game.GameField.CheckValidWPosOnGrid(wPos, out var gridPos))
        {
            tile.GridPosition = gridPos;
            return true;
        }
        return false;
    }

    private void OnMouseMoved(MouseUpdateAction action)
    {
        if (game.InputStateEnum != InputStateEnum.PlayCard) return;
        var card = Configs.GetCardConfig(game.PlayingCard);
        Vector3 screenPos = action.Finger.ScreenPosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Vector3 wPos = MathUtils.RaycastToXZPlane(ray.origin, ray.direction);
        switch (card)
        {
            case ICardPlayingTilePlace tilePlaceCard:
                game.GameField.ShowGhostObject(wPos);
                break;
        }
    }

    private bool TryFindTowerPlacement(Vector3 wPos, ICardPlayingTowerPlace card)
    {
        Debug.Log($"Check Can Place Tower {game.PlayingCard}");

        int placeIndex = -1;
        var canPlaceTower = game.GameField?.CheckPlaceTowerPosition(wPos, card.Tower, out placeIndex);
        if (canPlaceTower != null && canPlaceTower.Value)
        {
            card.PlacementIndex = placeIndex;
            return true;
        }
        return false;
    }

    private bool TryPlaceRoad(Vector3 wPos, ICardPlayingRoad card)
    {
        if (game.GameField.IsWPosInPolygon(wPos))
        {
            card.WPos = wPos;
            return true;
        }
        return false;
    }
}