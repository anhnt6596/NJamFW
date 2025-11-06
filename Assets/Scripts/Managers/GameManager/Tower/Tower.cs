using System;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform allyParent;
    [SerializeField] private EllipseClick ellipseClick;
    [SerializeField] private GameObject highlightPlacement;
    public OnTower Ally { get; private set; }
    public Transform AllyParent => allyParent;

    public void PlaceAlly(OnTower ally)
    {
        Ally = ally;
        Ally.Setup(this);
        ally.transform.SetParent(allyParent);
        ally.transform.localPosition = Vector3.zero;
        ally.transform.rotation = allyParent.rotation;
    }

    public bool CheckPlaceAlly(Vector3 screenPos)
    {
        if (Ally != null) return false;
        return ellipseClick.CheckClick(screenPos);
    }

    public void ShowHighlightPlacement(bool show)
    {
        highlightPlacement.SetActive(show);
    }
}