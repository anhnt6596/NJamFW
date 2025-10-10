using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class TutPopup : BasePopup
{
    [Serializable]
    public struct TutData
    {
        public string content;
        public Sprite sprite;
    }
    [SerializeField] TextMeshProUGUI contentText;
    [SerializeField] Image image;
    [SerializeField] TutData[] tutDatas;

    private int tutIndex;

    // 0: select card to play
    // 1: first time build tower
    // 2: first time build troop/mine
    // 3: first time cast spell
    public void DisplayTut(int index)
    {
        tutIndex = index;
        var tutData = tutDatas[index];

        contentText.text = tutData.content;
        image.sprite = tutData.sprite;
    }

    public Action OKAction { get; set; }
    public override void Hide()
    {
        base.Hide();
        OKAction?.Invoke();
    }
}
