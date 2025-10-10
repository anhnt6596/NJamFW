using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundResourceSet", menuName = "Resource/Sound")]
public class SoundResourceSet : ResourceSet
{
    public Music music;
    public General general;
    public Combat combat;

    [Serializable]
    public class Music
    {
        public AudioClip menuMusic;
        public AudioClip combatMusic;
    }

    [Serializable]
    public class General
    {
        public AudioClip button;
        public AudioClip win;
        public AudioClip lose;
    }

    [Serializable]
    public class Combat
    {
        public AudioClip button;
        public AudioClip arrowShot;
        public AudioClip lightning;
        public AudioClip manaBuff;
        public AudioClip teleport;
        public AudioClip spellAttack;
        public AudioClip cannonFire;
        public AudioClip explode;
        public AudioClip damageTaken;
        public AudioClip sword;
    }
}