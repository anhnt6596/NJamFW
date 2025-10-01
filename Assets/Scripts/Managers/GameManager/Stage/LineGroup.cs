using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGroup : MonoBehaviour
{
    public List<CatmullRomSpline2D> Lines { get; } = new();

    public IMovingPath GetRandomLine()
    {
        return Lines[UnityEngine.Random.Range(0, Lines.Count)];
    }

    private void Awake()
    {
        Lines.AddRange(GetComponentsInChildren<CatmullRomSpline2D>());
    }
}
