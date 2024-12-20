using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Entity : MonoBehaviour
{
    public static Player_Entity instance;
    public Player_Movement player_Movement;
    public Player_Interact Player_Interact;
    public Player_State_Manager Player_State_Manager;
    public Player_Animation_Manager Player_Animation_Manager;

    public AudioClip playerDeathAudioClip;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player_Movement = GetComponent<Player_Movement>();
        Player_Interact = GetComponent<Player_Interact>();
        Player_State_Manager = GetComponent<Player_State_Manager>();
        Player_Animation_Manager = GetComponentInChildren<Player_Animation_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        Audio_Manager.instance.PlaySFXOneShot(playerDeathAudioClip);

        Game_Manager.instance.loading_Scene_Manager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
