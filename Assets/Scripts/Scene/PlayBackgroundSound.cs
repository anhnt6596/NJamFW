using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackgroundSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlayMusic(ResourceProvider.Sound.music.menuMusic);
    }

}
