using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    void Start()
    {
        SoundManager.PlayMusic(ResourceProvider.Sound.music.menuMusic);
    }

}
