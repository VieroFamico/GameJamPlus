using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Interact : MonoBehaviour
{
    [Header("Dependencies")]
    private Player_Input playerInputActions;
    public BoxCollider boxCollider;

    [Header("3D Interacting")]
    public Base_Interactable currInteractable;
    public LayerMask interactableLayerMask;

    [Header("2D Interacting")]
    public LayerMask interactable2DLayerMask;
    public float interactionRange2D = 0.5f;

    [Header("HeldObject")]
    public float pushDistance;
    public LayerMask obstacleLayer;

    bool m_HitDetect;
    RaycastHit m_Hit;

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new Player_Input();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += x => Interact();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsObstacleInFront()
    {
        if (!currInteractable)
        {
            return false;
        }
        /*// Get the collider bounds of the held object
        Collider heldCollider = currInteractable.GetComponent<Collider>();
        if (heldCollider == null) return false;

        // Calculate the size and position of the box based on the collider bounds
        Vector3 boxSize = heldCollider.bounds.size;
        Vector3 boxCenter = transform.position + transform.forward * (boxSize.z / 2 + pushDistance);

        // Perform the boxcast
        return Physics.BoxCast(
            boxCenter,
            boxSize / 2,
            transform.forward,
            out RaycastHit hit,
            Quaternion.identity,
            pushDistance,
            obstacleLayer
        );*/

        // Ensure the held object has a collider
        Collider heldCollider = currInteractable.GetComponent<Collider>();
        if (heldCollider == null) return false;

        // Calculate the box size based on the held object's collider, scaled down a bit
        Vector3 boxSize = new Vector3(
            heldCollider.bounds.size.x * 0.5f - pushDistance, // Slightly smaller width to avoid edges
            heldCollider.bounds.size.y * 0.5f, // Slightly smaller
            heldCollider.bounds.size.z * 0.5f - pushDistance // Thin height to only check immediately in the front
        );

        // Calculate the direction from the held object to the player
        Vector3 directionToPlayer = (heldCollider.bounds.center - transform.position).normalized;

        // Determine the maximum distance we want to check, which is just up to the held objectÅfs size
        float maxCheckDistance = heldCollider.bounds.extents.z;

        // Set the number of overlap checks to perform along the direction from the held object to the player
        Vector3 checkPosition = heldCollider.bounds.center + (directionToPlayer * maxCheckDistance / 2);

        // Perform the overlap check at this position, ignoring the heldCollider itself
        Collider[] hits = Physics.OverlapBox(
            checkPosition,
            boxSize,
            Quaternion.identity,
            obstacleLayer
        );

        // If there are any hits, check if any of them are not the heldCollider
        foreach (Collider hit in hits)
        {
            if (hit != heldCollider)
            {
                return true; // An obstacle other than the held object was detected
            }
        }

        // No obstacles detected between the player and held object
        return false;
    }

    public bool IsObstacleBetweenHeldObjectAndPlayer()
    {
        if (!currInteractable)
        {
            return false;
        }

        // Ensure the held object has a collider
        Collider heldCollider = currInteractable.GetComponent<Collider>();
        if (heldCollider == null) return false;

        // Calculate the box size based on the held object's collider, scaled down a bit
        Vector3 boxSize = new Vector3(
            heldCollider.bounds.size.x * 0.5f - pushDistance, // Slightly smaller width to avoid edges
            heldCollider.bounds.size.y * 0.5f, // Slightly smaller
            heldCollider.bounds.size.z * 0.5f - pushDistance // Thin height to only check immediately in the back
        );

        // Calculate the direction from the held object to the player
        Vector3 directionToPlayer = (transform.position - heldCollider.bounds.center).normalized;

        // Determine the maximum distance we want to check, which is just up to the held objectÅfs size
        float maxCheckDistance = heldCollider.bounds.extents.z;

        // Set the number of overlap checks to perform along the direction from the held object to the player
        Vector3 checkPosition = heldCollider.bounds.center + directionToPlayer * maxCheckDistance / 2;

        // Perform the overlap check at this position, ignoring the heldCollider itself
        Collider[] hits = Physics.OverlapBox(
            checkPosition,
            boxSize,
            Quaternion.identity,
            obstacleLayer
        );

        // If there are any hits, check if any of them are not the heldCollider
        foreach (Collider hit in hits)
        {
            if (hit != heldCollider)
            {
                return true; // An obstacle other than the held object was detected
            }
        }

        // No obstacles detected between the player and held object
        return false;

    }


    private void Interact()
    {
        if(Player_Entity.instance.Player_State_Manager.GetState() == Player_State_Manager.PlayerState.ThreeDimension)
        {
            if (currInteractable)
            {
                Player_Entity.instance.Player_Animation_Manager.StopGrabAnimation();

                currInteractable.StopInteract(this);
                currInteractable = null;
                return;
            }

            TryGet3DBoxCast();
        }
        else
        {
            TryGet2DBoxCast();
        }
    }

    private void TryGet3DBoxCast()
    {
        /*m_HitDetect = Physics.BoxCast(boxCollider.transform.position, transform.localScale, transform.forward, out m_Hit, transform.rotation, 0.5f, 
            interactableLayerMask);
        if (m_HitDetect)
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + m_Hit.collider.name);

            Base_Interactable interactable = m_Hit.collider.GetComponent<Base_Interactable>();

            if(interactable != null)
            {
                interactable.Interacted(this);
                currInteractable = interactable;
            }
        }*/

        // Define the box size based on the player's local scale, slightly reduced if needed
        Vector3 boxSize = boxCollider.bounds.extents * 0.9f;

        // Set the center of the overlap box slightly in front of the player
        Vector3 boxCenter = boxCollider.bounds.center + transform.forward * 0.1f;

        // Find all colliders in the interactable layer within the box
        Collider[] colliders = Physics.OverlapBox(
            boxCenter,
            boxSize,
            transform.rotation,
            interactableLayerMask
        );

        // Find the closest interactable object
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        if (closestCollider != null)
        {
            Debug.Log("Hit : " + closestCollider.name);

            Base_Interactable interactable = closestCollider.GetComponent<Base_Interactable>();
            if (interactable != null)
            {
                interactable.Interacted(this);

                if (interactable.GetComponent<DragAndPush_Interactable>())
                {
                    currInteractable = interactable;
                    Player_Entity.instance.Player_Animation_Manager.StartGrabAnimation();
                }
            }
        }
    }

    private void TryGet2DBoxCast()
    {
        Collider[] hits = Physics.OverlapBox(
            boxCollider.transform.position,
            boxCollider.bounds.extents * 0.5f,
            Quaternion.identity,
            interactable2DLayerMask);

        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = hit;
            }
        }

        if (closestCollider != null)
        {
            Debug.Log("Hit: " + closestCollider.name);

            Base_Interactable interactable = closestCollider.GetComponent<Base_Interactable>();
            if (interactable != null)
            {
                interactable.Interacted(this);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * 0.5f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * 0.5f, transform.localScale);
        }
    }

}
