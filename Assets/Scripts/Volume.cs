using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        //This is used to connect the volume slider with the audio mixer
        audioMixer.SetFloat("Volume", volume);
    }
}
