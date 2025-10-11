using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEffect : MonoBehaviour
{
    [SerializeField] Image center, left, right;


    Vector3 centerOriPos;
    Vector3 leftOriPos;
    Vector3 rightOriPos;
    private void Awake()
    {
        centerOriPos = center.rectTransform.localPosition;
        leftOriPos = left.rectTransform.localPosition;
        rightOriPos = right.rectTransform.localPosition;
    }

    private void OnEnable()
    {
        DoAnimShow();
    }

    private void DoAnimShow()
    {
        center.SetAlpha(0);
        left.SetAlpha(0);
        right.SetAlpha(0);

        center.rectTransform.localPosition = centerOriPos + Vector3.down * 400;
        left.rectTransform.localPosition = leftOriPos + Vector3.left * 250;
        right.rectTransform.localPosition = leftOriPos + Vector3.right * 250;

        float sideDelay = 0.3f;

        center.DOFade(1, 1.2f).SetEase(Ease.OutSine).SetDelay(0f);
        center.transform.DOLocalMove(centerOriPos, 1.5f).SetEase(Ease.OutSine).SetDelay(0f);

        left.DOFade(1, 0.8f).SetEase(Ease.OutSine).SetDelay(sideDelay);
        left.transform.DOLocalMove(leftOriPos, 1.2f).SetEase(Ease.OutSine).SetDelay(sideDelay);

        right.DOFade(1, 0.8f).SetEase(Ease.OutSine).SetDelay(sideDelay);
        right.transform.DOLocalMove(rightOriPos, 1.2f).SetEase(Ease.OutSine).SetDelay(sideDelay);
    }
}
