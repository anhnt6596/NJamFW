using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGroup : MonoBehaviour
{
    public List<CatmullRomSpline> Lines { get; } = new();
    private void Awake()
    {
        Lines.AddRange(GetComponentsInChildren<CatmullRomSpline>());
    }
}
