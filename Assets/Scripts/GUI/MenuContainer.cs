using Core;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MenuContainer : MonoBehaviour
{
    [SerializeField] Transform menuArea;
    [SerializeField] RectTransform gameTittle;
    private void OnEnable()
    {
        DoAnimShow();
    }

    private void DoAnimShow()
    {
        // game tittle
        {
            var oriPos = gameTittle.anchoredPosition;
            gameTittle.anchoredPosition = oriPos + Vector2.up * 400;
            var cg = gameTittle.gameObject.GetOrAddComponent<CanvasGroup>();
            cg.alpha = 0;

            this.DelayCall(0.5f, () =>
            {
                cg.DOFade(1, 0.5f);
                gameTittle.DOAnchorPos(oriPos, 0.5f).SetEase(Ease.OutSine);
            });
        }

        // fade button seq
        var allButtons = menuArea.DirectChildGameObjects().ToList();
        for (int i = 0; i < allButtons.Count; i++)
        {
            var button = allButtons[i];
            var cg = button.GetOrAddComponent<CanvasGroup>();
            cg.alpha = 0;

            var oriScale = button.transform.localScale;
            button.transform.localScale = oriScale * 0.3f;

            this.DelayCall(1f + i * 0.2f, () =>
            {
                cg.DOFade(1, 0.5f).SetEase(Ease.OutSine);
                button.transform.DOScale(oriScale, 0.5f).SetEase(Ease.OutBack);
            });
        }
    }

    public void StartGame() => App.Get<GameManager>().RunSceneGame();
    public void OpenAboutUsPopup() => App.Get<GUIManager>().ShowGui<AboutUsPopup>();
    public void OpenHelpPopup()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        App.Get<GUIManager>().ShowGui<HelpPopup>();
    }

    public void OpenSettings()
    {
        var notiPopup = App.Get<GUIManager>().ShowGui<NotiPopup>();
        notiPopup.Tittle = "Opp!!";
        notiPopup.Content = "Feature not ready!!!!";
        notiPopup.OKAction = () =>
        {
            Debug.Log("Popup Setting Close!");
        };
    }
}
