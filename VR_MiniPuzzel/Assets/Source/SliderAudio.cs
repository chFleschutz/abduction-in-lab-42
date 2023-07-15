using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SliderAudio : MonoBehaviour
{
    public LinearMapping linearMapping;
    public AudioSource audioSource;
    public float pitchFactor = 10f;
    public float minDistance = 0.01f;

    private float playbackPosition()
    {
        return (audioSource.time / audioSource.clip.length);
    }
    void Update()
    {
        if( Mathf.Abs(playbackPosition() - linearMapping.value) > minDistance )
        {
            audioSource.pitch = (linearMapping.value - playbackPosition())  *pitchFactor;
            if( !audioSource.isPlaying)
            {
                audioSource.Play();
            }  
        }
        else
        {
            audioSource.Pause();
        }
    }

}
