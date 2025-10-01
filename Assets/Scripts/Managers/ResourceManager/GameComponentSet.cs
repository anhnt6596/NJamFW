using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameComponentSet", menuName = "Resource/GameComponent")]
public class GameComponentSet : ResourceSet
{
    [SerializeField] HealthBar healthBar;
    public HealthBar HealthBar => healthBar;
}