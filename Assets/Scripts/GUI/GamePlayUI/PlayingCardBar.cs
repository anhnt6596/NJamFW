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
    }

    private void OnTouchDown(TouchDownAction obj)
    {
        Vector3 screenPos = obj.Finger.ScreenPosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));

        worldPos.z = 0;
        if (TryPlayCard(worldPos))
        {
            game.CardActionDone();
        }
        else
        {
            var guiEffectMgr = App.Get<GUIEffectManager>();
            guiEffectMgr.ShowInvalidEffect(worldPos, GUILayer.GUI);
        }
    }

    private bool TryPlayCard(Vector3 worldPos)
    {
        switch (game.PlayingCard)
        {
            case CardEnum.Bomb:
                {
                    game.DropBomb(worldPos);
                    return true;
                }
            case CardEnum.ArcherTower:
            case CardEnum.MageTower:
            case CardEnum.ArtilleryTower:
                {
                    return TryPlaceTower(worldPos);
                }
            case CardEnum.TimeReverse:
                {
                    game.ReverseEnemies(worldPos);
                    return true;
                }
            case CardEnum.Troop:
                {
                    return TryPlaceTroop(worldPos);
                }
            case CardEnum.Napalm:
                {
                    game.DropNapalm(worldPos);
                    return true;
                }
        }
        return true;
    }

    private bool TryPlaceTower(Vector3 wPos)
    {
        Debug.Log($"Check Can Place Tower {game.PlayingCard}");
        var towerConfig = (TowerCardConfig)Configs.GetCardConfig(game.PlayingCard);

        int placeIndex = -1;
        var canPlaceTower = game.GamePlay?.CheckPlaceTowerPosition(wPos, towerConfig.Tower, out placeIndex);
        if (canPlaceTower != null && canPlaceTower.Value)
        {
            game.PlaceTower(placeIndex, towerConfig.Tower);
            return true;
        }
        return false;
    }

    private bool TryPlaceTroop(Vector3 wPos)
    {
        if (game.GamePlay.IsWPosInPolygon(wPos))
        {
            var allyConfig = (AllyCardConfig)Configs.GetCardConfig(game.PlayingCard);
            game.GamePlay.SpawnAlly(allyConfig.Ally, wPos);
            return true;
        }
        return false;
    }

    public void OnClickCancel()
    {
        game.CancelPlayingCard();
    }
}