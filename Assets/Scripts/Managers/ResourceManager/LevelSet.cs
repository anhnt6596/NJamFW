using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSet", menuName = "Resource/Level")]
public class LevelSet : ResourceSet
{
    [SerializeField] public List<Level> levels
        ;
}