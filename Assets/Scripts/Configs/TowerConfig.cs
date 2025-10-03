using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ATower", menuName = "Tower Config", order = 0)]
public class TowerConfig : ScriptableObject
{
    [SerializeField] private TowerEnum type;
    [SerializeField] private Damage baseAttack;
    [SerializeField] private float speed;

    public TowerEnum Type => type;
    public Damage BaseAttack => baseAttack;
    public float Speed => speed;

}
