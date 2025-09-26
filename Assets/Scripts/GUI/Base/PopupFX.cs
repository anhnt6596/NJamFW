using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    None,
    Fade,
    Zoom,
    Fall,
}
public interface IPopupAppear
{
    void DOAnim();
}
public interface IPopupDisappear
{
    void DOClose(System.Action close);
}
public class PopupAppear_None : IPopupAppear
{
    public PopupAppear_None() { }
    public void DOAnim() { }
}
public class PopupDisappear_None : IPopupDisappear
{
    public PopupDisappear_None() { }
    public void DOClose(System.Action close) => close();
}
public class PopupAppear_Fade : IPopupAppear
{
    BasePopup popup;
    public PopupAppear_Fade(BasePopup popup) => this.popup = popup;
    public void DOAnim()
    {
        CanvasGroup cg = popup.GetComponent<CanvasGroup>();
        if (!cg) cg = popup.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 0.3f).SetUpdate(true);
    }
}

public class PopupDisappear_Fade : IPopupDisappear
{
    BasePopup popup;
    public PopupDisappear_Fade(BasePopup popup) => this.popup = popup;
    public void DOClose(System.Action closeAction)
    {
        CanvasGroup cg = popup.GetComponent<CanvasGroup>();
        if (!cg) cg = popup.gameObject.AddComponent<CanvasGroup>();
        cg.DOFade(0, 0.2f)
            .SetUpdate(true)
            .OnComplete(() => closeAction());
    }
}

public class PopupAppear_Zoom : IPopupAppear
{
    BasePopup popup;
    Transform firstChild;
    Vector3 firstChildScale;
    public PopupAppear_Zoom(BasePopup popup)
    {
        this.popup = popup;
        firstChild = popup.transform.GetChild(0);
        firstChildScale = popup.transform.GetChild(0).localScale;
    }
    public void DOAnim()
    {
        CanvasGroup cg = popup.GetComponent<CanvasGroup>();
        if (!cg) cg = popup.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 0.3f).SetUpdate(true);
        firstChild.localScale = Vector3.zero;
        firstChild.DOScale(firstChildScale, 0.3f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
}

public class PopupDisappear_Zoom : IPopupDisappear
{
    BasePopup popup;
    Transform firstChild;
    Vector3 firstChildScale;
    public PopupDisappear_Zoom(BasePopup popup)
    {
        this.popup = popup;
        firstChild = popup.transform.GetChild(0);
        firstChildScale = popup.transform.GetChild(0).localScale;
    }
    public void DOClose(System.Action closeAction)
    {
        CanvasGroup cg = popup.GetComponent<CanvasGroup>();
        if (!cg) cg = popup.gameObject.AddComponent<CanvasGroup>();
        cg.DOFade(0, 0.2f).SetUpdate(true).OnComplete(() => closeAction());
        firstChild.DOScale(firstChildScale * 0.3f, 0.2f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            firstChild.localScale = firstChildScale;
        });
    }
}