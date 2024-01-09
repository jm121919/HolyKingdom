using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public SoundObjectPool soundObjectPool;
    //사운드 클립들
    public AudioClip potionSound;
    public AudioClip coindropSound;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip bgmSound;
    public AudioClip uiPopSound;
    public AudioClip equipSound;
}
