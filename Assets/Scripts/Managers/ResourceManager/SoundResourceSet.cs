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
        public AudioClip startTurn;
        public AudioClip turnCompleted;
    }

    [Serializable]
    public class Combat
    {
        public Tower tower;
        public AudioClip lightning;
        public AudioClip gain;
        public AudioClip teleport;
        public AudioClip hugeExplode;
        public AudioClip smallExplode;
        public AudioClip damageTaken;
        public AudioClip sword;
        public AudioClip bossCastSpell;
    }

    [Serializable]
    public class Tower
    {
        public AudioClip arrowShot;
        public AudioClip mageShot;
        public AudioClip artilleryShot;

        public AudioClip build;
        public AudioClip upgrade;
        public AudioClip GetShotSound(TowerEnum tower)
        {
            switch (tower)
            {
                case TowerEnum.ArcherTower:
                    return arrowShot;
                case TowerEnum.MageTower:
                    return mageShot;
                case TowerEnum.ArtilleryTower:
                    return artilleryShot;
                default:
                    return null;
            }
        }
    }
}