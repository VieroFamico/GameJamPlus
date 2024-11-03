using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DragAndPush_Interactable : Base_Interactable
{
    [Serializable]
    public class HoldPosition
    {
        public Transform holdTransformPosition;
        public Vector3 holdRotation;
    }

    public List<HoldPosition> holdPositions;

    public GameObject destroyedParticleSystem;
    public AudioClip destroyAudioClip;

    public bool isHeld = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destroyed()
    {
        StopInteract(Player_Entity.instance.Player_Interact);

        Audio_Manager.instance.PlaySFXOneShot(destroyAudioClip);

        GameObject spawnedParticleSystem = Instantiate(destroyedParticleSystem, transform.position, Quaternion.identity);

        Destroy(spawnedParticleSystem, 0.5f);
        Destroy(gameObject);
    }

    public override void Interacted(Player_Interact player_Interact)
    {
        base.Interacted(player_Interact);

        Held(!isHeld);

        player_Interact.currInteractable = this;

        float minDistance = Mathf.Infinity;
        HoldPosition closestHoldPosition = null;

        foreach(HoldPosition holdPosition in holdPositions)
        {
            float currDistance = Vector3.Distance(player_Interact.transform.position, holdPosition.holdTransformPosition.position);

            if (minDistance > currDistance)
            {
                minDistance = currDistance;
                closestHoldPosition = holdPosition;
            }
        }

        if(closestHoldPosition != null)
        {
            player_Interact.transform.position = closestHoldPosition.holdTransformPosition.position;
            player_Interact.transform.eulerAngles = closestHoldPosition.holdRotation;

            transform.parent = player_Interact.transform;

            Player_Entity.instance.player_Movement.LockRotation(true);
        }
        else
        {
            Debug.Log("There is no holdPosition on this GameObject");
        }
        
    }

    public override void StopInteract(Player_Interact player_Interact)
    {
        base.StopInteract(player_Interact);

        Held(!isHeld);

        Player_Entity.instance.player_Movement.LockRotation(false);

        transform.parent = null;
    }

    public bool IsHeld()
    {
        return isHeld;
    }

    public virtual void Held(bool changeHeldState)
    {
        isHeld = changeHeldState;
    }
}
