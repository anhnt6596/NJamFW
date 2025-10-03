using Core;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneUI : MonoBehaviour
{
    [SerializeField] GameObject display;
    [SerializeField] Sprite[] pantsSprites;

    private void Awake()
    {
        Hide();
        foreach (Transform child in display.transform)
        {
            var img = child.GetComponent<Image>();
            if (img) img.sprite = pantsSprites[UnityEngine.Random.Range(0, pantsSprites.Length)];
        }
    }

    public void DoLoadScene(string sceneName)
    {
        //Cover(() => App.Get<SceneService>().LoadScene(sceneName));
    }

    private void Hide()
    {
        display.SetActive(false);
    }

    Sequence seq;

    public Tween Cover()
    {
        Hide();
        display.SetActive(true);
        seq?.Kill();
        seq = DOTween.Sequence();
        foreach (Transform child in display.transform)
        {
            child.localScale = Vector3.zero;
            seq.Join(child.DOScale(1f, 0.6f).SetEase(Ease.OutSine));
        }
        seq.SetUpdate(true);
        return seq;
    }

    public Tween Expose()
    {
        seq?.Kill();
        seq = DOTween.Sequence();
        foreach (Transform child in display.transform)
        {
            child.localScale = Vector3.one * 1f;
            seq.Join(child.DOScale(0, 0.6f).SetEase(Ease.InSine));
        }
        seq.AppendCallback(() => display.SetActive(false));
        seq.SetUpdate(true);
        return seq;
    }

    public IEnumerator CoverScene()
    {
        yield return Cover().WaitForCompletion();
    }

    public IEnumerator ExposeScene()
    {
        yield return Expose().WaitForCompletion();
    }

    public void Init()
    {

    }
}
