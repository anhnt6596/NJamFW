using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundResourceSet", menuName = "Resource/Sound")]
public class SoundResourceSet : ResourceSet
{
    public Music music;
    public General general;

    [Serializable]
    public class Music
    {
        public AudioClip backgroundMusic;
        public AudioClip gameMusic;
        public AudioClip climaxMusic;
    }

    [Serializable]
    public class General
    {
        public AudioClip button;
    }
}