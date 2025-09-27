using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class NotiPopup : BasePopup
{
    [SerializeField] TextMeshProUGUI titleText, contentText;
    public string Tittle
    {
        set => titleText.text = value;
        get => titleText.text;
    }

    public string Content
    {
        set => contentText.text = value;
        get => contentText.text;
    }

    public Action OKAction { get; set; }

    public override void Hide()
    {
        base.Hide();
        OKAction?.Invoke();
    }

}
