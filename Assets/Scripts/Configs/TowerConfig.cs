using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ATower", menuName = "Config/Tower", order = 0)]
public class TowerConfig : ScriptableObject
{
    [SerializeField] private TowerEnum type;
    [SerializeField] private Damage baseAttack;
    [SerializeField] private float fireRate;
    [SerializeField] private Vector2 range;

    public TowerEnum Type => type;
    public Damage BaseAttack => baseAttack;
    public float FireRate => fireRate;
    public Vector2 Range => range;
    public Damage GetAttackByLevel(int level) => baseAttack + baseAttack * (level - 1) * 1.1f;

}
