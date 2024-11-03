using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    public AudioSource sfxAudioSource;
    public AudioSource bgmAudioSource;

    public List<AudioSource> continousAudioSourceList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    public void PlaySFXOneShot(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public AudioSource PlaySFX(AudioClip clip)
    {
        AudioSource newAudioSource = new AudioSource();

        newAudioSource.clip = clip;
        newAudioSource.Play();

        continousAudioSourceList.Add(newAudioSource);

        Destroy(newAudioSource, clip.length);

        return newAudioSource;
    }

    public void StopSFX(AudioSource audioSourceToStop)
    {
        if (continousAudioSourceList.Contains(audioSourceToStop))
        {
            AudioSource audioSource = continousAudioSourceList.Find((x) => x = audioSourceToStop);

            audioSource.Stop();

            Destroy(audioSource);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }
}
