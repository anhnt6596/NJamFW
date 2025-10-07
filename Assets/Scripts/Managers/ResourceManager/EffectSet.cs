using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EffectSet", menuName = "Resource/Effect")]
public class EffectSet : ResourceSet
{
    public ParticleSystem lazerBlue;
    public ParticleSystem lazerPurple;
    public ParticleSystem bombExplosion;
    public GameObject smoke;
    public Image invalid;
    public GameObject burning;
    public ArrowFocus arrowFocus;
}