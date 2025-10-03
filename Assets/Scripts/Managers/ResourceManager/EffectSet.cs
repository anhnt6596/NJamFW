using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSet", menuName = "Resource/Effect")]
public class EffectSet : ResourceSet
{
    public ParticleSystem lazerBlue;
    public ParticleSystem lazerPurple;
    public ParticleSystem bombExplosion;
    public GameObject smoke;
}