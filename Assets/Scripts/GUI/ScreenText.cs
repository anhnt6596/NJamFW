using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class ScreenText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Show(string content, Color color, Action completeCallback)
    {
        text.text = content;
        text.color = color;
        DoEffect(completeCallback);
    }

    Sequence _seq;

    private void DoEffect(Action completeCallback)
    {
        _seq?.Kill();
        _seq = DOTween.Sequence();
        text.transform.localPosition = Vector3.zero;
        text.SetAlpha(0);
        _seq.Append(text.transform.DOLocalJump(Vector3.down * 25, 50, 1, 2).SetEase(Ease.OutSine));
        _seq.Join(text.DOFade(1, 0.1f));
        _seq.Insert(1.5f, text.DOFade(0, 0.5f));
        _seq.AppendCallback(() => completeCallback());
    }
}
