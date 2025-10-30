using System;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform allyParent;
    [SerializeField] private EllipseClick ellipseClick;
    public OnTower Ally { get; private set; }

    public void PlaceAlly(OnTower ally)
    {
        Ally = ally;
        ally.transform.SetParent(allyParent);
        ally.transform.localPosition = Vector3.zero;
        ally.transform.rotation = allyParent.rotation;
    }

    public bool CheckPlaceAlly(Vector3 screenPos)
    {
        if (Ally != null) return false;
        return ellipseClick.CheckClick(screenPos);
    }
}