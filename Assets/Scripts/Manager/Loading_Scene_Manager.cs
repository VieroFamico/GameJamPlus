using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Scene_Manager : MonoBehaviour
{
    public static Loading_Scene_Manager Instance;

    public Animator animator;
    public AudioClip loadInAudioClip;
    public AudioClip loadOutAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadIn()
    {
        if (animator != null)
        {
            animator.SetTrigger("LoadIn");
            Audio_Manager.instance.PlaySFXOneShot(loadInAudioClip);
        }
    }

    public void LoadOut()
    {
        if (animator != null)
        {
            animator.SetTrigger("LoadOut");
            Audio_Manager.instance.PlaySFXOneShot(loadOutAudioClip);
        }
    }
}
