using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource source;

    private float volume = 1.0f;

    void Update()
    {
        source.volume = volume;
    }

    public void UpdateAudio(float volume)
    {
        this.volume = volume;
    }

}
