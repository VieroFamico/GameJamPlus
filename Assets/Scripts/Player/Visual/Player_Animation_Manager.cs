using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation_Manager : MonoBehaviour
{
    private Animator animator;

    private Player_Movement player_Movement;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        player_Movement = Player_Entity.instance.player_Movement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
