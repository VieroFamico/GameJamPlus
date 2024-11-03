using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation_Manager : MonoBehaviour
{
    private Animator animator;
    private Player_Movement player_Movement;

    [Header("Dependencies")]
    public ParticleSystem runParticleSystem;

    private float lastDir;
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
            Debug.Log("A");
        }
        else
        {
            Animation2DMovement();
            Debug.Log("B");
        }
    }

    private void Animation3DMovement()
    {
        if(Mathf.Abs(player_Movement.MovementInput().y) >= 0.1f)
        {
            animator.SetBool("Walking", true);

            if(runParticleSystem.isPlaying)
            {
                return;
            }

            runParticleSystem.Play();
        }
        else
        {
            animator.SetBool("Walking", false);
            runParticleSystem.Stop();
        }

        if (player_Movement.MovementInput().y >= 0.1f)
        {
            runParticleSystem.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }
        else if(player_Movement.MovementInput().y <= -0.1f)
        {
            runParticleSystem.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
    }

    private void Animation2DMovement()
    {
        if (Mathf.Abs(player_Movement.MovementInput().x) >= 0.1f)
        {
            animator.SetFloat("2DMove", player_Movement.MovementInput().x);

            lastDir = 1 * player_Movement.MovementInput().x;

            if (runParticleSystem.isPlaying)
            {
                return;
            }

            runParticleSystem.Play();
        }
        else
        {
            animator.SetFloat("2DMove", lastDir);
            runParticleSystem.Stop();
        }

        if (player_Movement.MovementInput().x >= 0.1f)
        {
            runParticleSystem.transform.localEulerAngles = new Vector3(-90, 0, 90);
        }
        else if (player_Movement.MovementInput().x <= -0.1f)
        {
            runParticleSystem.transform.localEulerAngles = new Vector3(-90, 0, -90);
        }
    }

    public void GoTo3D()
    {
        animator.SetFloat("2DMove", 0f);
    }

    public void GoTo2D()
    {
        animator.SetBool("Walking", false);

        runParticleSystem.transform.localEulerAngles = new Vector3(-90, 0, 90);
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
