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
        switch (game.PlayingCard)
        {
            case CardEnum.Bomb:
                {
                    game.DropBomb(worldPos);
                    break;
                }
            case CardEnum.ArcherTower:
            case CardEnum.MageTower:
            case CardEnum.ArtilleryTower:
                {
                    CheckCanPlaceTower(worldPos);
                    break;
                }
            case CardEnum.TimeReverse:
                {
                    game.DoReverseAllEnemies(worldPos);
                    break;
                }
        }
    }

    private void CheckCanPlaceTower(Vector3 wPos)
    {
        Debug.Log($"Check Can Place Tower {game.PlayingCard}");
        var towerConfig = (TowerCardConfig)Configs.GetCardConfig(game.PlayingCard);

        int placeIndex = -1;
        var canPlaceTower = game.GamePlay?.CheckPlaceTowerPosition(wPos, towerConfig.Tower, out placeIndex);
        if (canPlaceTower != null && canPlaceTower.Value)
        {
            game.PlaceTower(placeIndex, towerConfig.Tower);
        }
    }
}