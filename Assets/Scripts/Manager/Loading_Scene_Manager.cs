using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Scene_Manager : MonoBehaviour
{
    public static Loading_Scene_Manager Instance;

    public Animator animator;


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
        }
    }

    public void LoadOut()
    {
        if (animator != null)
        {
            animator.SetTrigger("LoadOut");
        }
    }

}
