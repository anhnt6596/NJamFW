using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameComponentSet", menuName = "Resource/GameComponent")]
public class GameComponentSet : ResourceSet
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] Mine mine;
    public HealthBar HealthBar => healthBar;
    public Mine Mine => mine;
}