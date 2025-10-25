using Core;
using System;
using TMPro;
using UnityEngine;

public class PlayingCardBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDescription;

    Game game;

    public void Display(Game game)
    {
        this.game = game;
        textDescription.text = Configs.GetCardConfig(game.PlayingCard).GetPlayDescription(game);

        CheckShowTut();
    }

    public void OnClickCancel()
    {
        game.CancelPlayingCard();
    }

    // quick quick check show tut
    private void CheckShowTut()
    {
        return;
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