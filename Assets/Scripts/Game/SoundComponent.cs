using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    Action action;
    public AudioSource audioSource;
    public AudioClip clip;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play(AudioClip audioClip)
    {
        clip = audioClip;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            SoundManager.instance.soundObjectPool.ReturnPool(gameObject);
            clip = null;
        }
    }
}
