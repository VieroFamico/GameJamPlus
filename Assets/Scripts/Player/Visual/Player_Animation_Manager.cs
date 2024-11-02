using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation_Manager : MonoBehaviour
{
    private Animator animator;
    private Player_Movement player_Movement;

    [Header("Dependencies")]
    public ParticleSystem runParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        player_Movement = Player_Entity.instance.player_Movement;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player_Entity.instance.Player_State_Manager.GetState() == Player_State_Manager.PlayerState.ThreeDimension)
        {
            Animation3DMovement();
        }
        else
        {
            Animation2DMovement();
        }
    }

    private void Animation3DMovement()
    {
        if(Mathf.Abs(player_Movement.MovementInput().y) >= 0.1f)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    private void Animation2DMovement()
    {

    }

    public void StartGrabAnimation()
    {
        animator.SetTrigger("StartPullPush");
    }

    public void StopGrabAnimation()
    {
        animator.SetTrigger("StopPullPush");
    }
}
