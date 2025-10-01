using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSet", menuName = "Resource/Stage")]
public class StageSet : ResourceSet
{
    [SerializeField] public List<Stage> stages;
}