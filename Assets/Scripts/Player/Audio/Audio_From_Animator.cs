using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_From_Animator : MonoBehaviour
{
    public AudioClip walkingAudioClip;
    public AudioClip walkingWithBoxAudioClip;
    

    public void PlayWalkSound()
    {
        Audio_Manager.instance.PlaySFXOneShot(walkingAudioClip);
    }

    public void PlayWalkWithBoxSound()
    {
        Audio_Manager.instance.PlaySFXOneShot(walkingWithBoxAudioClip);
    }
}
