using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePopup : BaseGUI
{
    [SerializeField] public GUILayer layer;
    [SerializeField] EffectType openFx, closeFx;
    [SerializeField] bool closeWhenClickBackground;
    public override bool IsActive { get; set; }
    public override GUILayer Layer => layer;
    IPopupAppear appearSM;
    IPopupDisappear disappearSM;

    public void Setup()
    {
        SetFXSM();
        if (closeWhenClickBackground) AddBackgroundClick();
    }

    private void SetFXSM()
    {
        appearSM = GetFXSMAppear();
        disappearSM = GetFXSMDisappear();
    }

    private void AddBackgroundClick()
    {
        var btn = gameObject.GetOrAddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        btn.onClick.AddListener(CheckTouchBackground);
    }

    public override void Show()
    {
        Setup();
        base.Show();
        appearSM.DOAnim();
    }

    public virtual void Close() => disappearSM.DOClose(Hide);

    private void CheckTouchBackground()
    {
        var firstChild = transform.GetChild(0);
        if (!firstChild.IsClickOver()) Close();
    }

    private IPopupAppear GetFXSMAppear()
    {
        switch (openFx)
        {
            case EffectType.Fade:
                return new PopupAppear_Fade(this);
            case EffectType.Zoom:
                return new PopupAppear_Zoom(this);
            default:
                return new PopupAppear_None();
        }
    }
    private IPopupDisappear GetFXSMDisappear()
    {
        switch (closeFx)
        {
            case EffectType.Fade:
                return new PopupDisappear_Fade(this);
            case EffectType.Zoom:
                return new PopupDisappear_Zoom(this);
            default:
                return new PopupDisappear_None();
        }
    }
}