using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CW.Common.CwInputManager;

public class Stage : MonoBehaviour
{
    public List<LineGroup> LineGroups { get; private set; } = new();
    private void Awake()
    {
        LineGroups.AddRange(GetComponentsInChildren<LineGroup>());
    }
}
