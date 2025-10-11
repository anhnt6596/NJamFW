using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup_StarRate : MonoBehaviour
{
    [SerializeField] Transform[] stars;

    public void HideAllStars()
    {
        foreach (var star in stars) star.gameObject.SetActive(false); 
    }

    public void ShowStars(int starCount)
    {
        for (int i = 0; i < starCount; i++)
        {
            var star = stars[i];
            stars[i].gameObject.SetActive(true);
            star.localScale = Vector3.zero;
            star.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f + i * 0.2f).SetUpdate(true);
        }
    }
}
