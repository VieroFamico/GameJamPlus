using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDimensionLever : Base_Interactable
{
    public AudioClip interactedSound;
    public float interactDelay = 0.5f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public override void Interacted(Player_Interact player_Interact)
    {
        if (timer < interactDelay)
        {
            return;
        }

        base.Interacted(player_Interact);

        Audio_Manager.instance.PlaySFXOneShot(interactedSound);

        if(Player_Entity.instance.Player_State_Manager.GetState() == Player_State_Manager.PlayerState.ThreeDimension)
        {
            Player_Entity.instance.player_Movement.Start2DMovement();
        }
        else
        {
            Player_Entity.instance.player_Movement.Start3DMovement();
        }

        timer = 0f;
    }
}
