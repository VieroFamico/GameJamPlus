using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody rigidbody;
    private BoxCollider boxCollider;
    private Player_Input playerInputActions;
    public GameObject characterModel;

    [Header("3D Movement Variables")]
    public float moveSpeed3D;
    public float rotateSpeed;
    public float friction;

    [Header("2D Movement Variables")]
    public float moveSpeed2D;
    public float jumpForce;
    public float friction2D;
    public LayerMask obstacleLayer;
    public LayerMask groundLayer;

    private bool lock3DMovement;
    private bool lock3DRotation;
    private bool isGrounded;

    private float movementMomentum;
    private float rotationMomentum;

    private bool movementEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        playerInputActions = new Player_Input();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += x => Jump2D();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnabled)
        {
            if (Player_Entity.instance.Player_State_Manager.GetState() == Player_State_Manager.PlayerState.TwoDimension)
            {
                TwoDimensional_Movement();
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementEnabled)
        {
            if (Player_Entity.instance.Player_State_Manager.GetState() == Player_State_Manager.PlayerState.ThreeDimension)
            {
                ThreeDimensonal_Movement();
            }
        }
        
    }


    #region 3D Movement

    private void ThreeDimensonal_Movement()
    {
        if (!lock3DMovement)
        {
            Movement();
        }
        if (!lock3DRotation)
        {
            Rotation();
        }
    }

    private void Movement()
    {
        if (Mathf.Abs(movementMomentum) < Mathf.Abs(MovementInput().y))
        {
            movementMomentum = MovementInput().y;
        }

        MoveTowards(movementMomentum, moveSpeed3D);

        if (movementMomentum > 0)
        {
            movementMomentum -= movementMomentum > 0.1f ? Time.deltaTime * friction : Mathf.Abs(movementMomentum);
        }
        else if (movementMomentum < 0)
        {
            movementMomentum += movementMomentum < 0.1f ? Time.deltaTime * friction : Mathf.Abs(movementMomentum);
        }
    }

    private Vector3 MovementDirection()
    {
        Camera camera = Camera.main;

        //camera forward and right vectors:
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = right * MovementInput().x + forward * MovementInput().y;

        return moveDir;
    }

    private void MoveTowards(float forwardMovement, float speed)
    {
        Vector3 totalMove = transform.forward * forwardMovement;
        if (Mathf.Abs(forwardMovement) > 0.01f)
        {
            if (forwardMovement > 0 && Player_Entity.instance.Player_Interact.IsObstacleInFront())
            {
                return;
            }
            if (forwardMovement < 0 && Player_Entity.instance.Player_Interact.IsObstacleBetweenHeldObjectAndPlayer())
            {
                return;
            }

            if (!IsObstacleInDirection(totalMove.normalized))
            {
                rigidbody.MovePosition(transform.position + (speed * Time.deltaTime * totalMove));
            }

            //lastMoveDir = moveDir;
            return;
        }
    }

    private void Rotation()
    {
        transform.Rotate(0, MovementInput().x * rotateSpeed, 0);
    }

    public void RotateTowards(Vector3 moveDir, bool absoluteFollow)
    {
        float targetAngle;
        float smoothedAngle;

        targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Mathf.Clamp(Mathf.Abs(targetAngle - transform.eulerAngles.y), 0, 0.2f));

        transform.localEulerAngles = new Vector3(0, smoothedAngle, 0);
    }

    #endregion

    #region 2D Movement
    private void TwoDimensional_Movement()
    {
        // Check for ground below the player
        isGrounded = IsGroundBelow();

        // Handle horizontal movement (z-axis is the 2D vertical axis here)
        Vector3 movement = Vector3.zero;
        if (MovementInput().x < 0)
        {
            movement += Vector3.left; // Move left (relative to 2D)
        }
        else if (MovementInput().x > 0)
        {
            movement += Vector3.right; // Move right (relative to 2D)
        }

        /*if (MovementInput().y > 0)
        {
            movement += Vector3.forward; // Move "up" in the 2D view
        }
        else if (MovementInput().y < 0)
        {
            movement += Vector3.back; // Move "down" in the 2D view
        }*/

        Vector3 gravDir = new Vector3(0, 0, -5f);

        Debug.Log(IsObstacleInDirection(movement.normalized));
        Debug.Log(isGrounded);

        if (!isGrounded)
        {
            rigidbody.AddForce(gravDir);
        }
        else
        {
            rigidbody.velocity = new Vector3 (rigidbody.velocity.x, 0f, 0f);
        }

        // Apply movement only if there is no obstacle in the desired direction
        if (movement != Vector3.zero && !IsObstacleInDirection(movement.normalized) && rigidbody.velocity.sqrMagnitude <= moveSpeed2D)
        {
            Debug.Log(movement);
            rigidbody.AddForce(movement.normalized * moveSpeed2D);
        }
        else if(movement == Vector3.zero && rigidbody.velocity.magnitude > 0.1f)
        { 
            rigidbody.velocity -= new Vector3(rigidbody.velocity.x, 0, 0) * friction2D;
        }

    }

    private void Jump2D()
    {
        if (isGrounded && Player_Entity.instance.Player_State_Manager.GetState() == Player_State_Manager.PlayerState.TwoDimension)
        {
            rigidbody.AddForce(Vector3.forward * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGroundBelow()
    {
        /*Vector3 boxCenter = boxCollider.bounds.center;
        Vector3 boxSize = boxCollider.bounds.size;
        float checkDistance = 0.1f; // Small distance below the collider

        return Physics.BoxCast(
            boxCenter,
            boxSize / 2,
            Vector3.back, // Checking in the (0,0,-1) direction
            Quaternion.identity,
            checkDistance,
            groundLayer
        );*/

        // Calculate the size of the overlap box as a fraction of the player's scale
        Vector3 boxSize = new Vector3(
            boxCollider.bounds.size.x * 0.9f, // Slightly smaller width to avoid edges
            boxCollider.bounds.size.y * 0.01f, // Thin height to only check immediately below
            boxCollider.bounds.size.z * 0.9f  // Slightly smaller depth
        );

        // Position the box slightly below the player's collider
        Vector3 boxCenter = boxCollider.bounds.center + Vector3.back * (boxCollider.bounds.extents.y * transform.localScale.y - 0.1f);

        // Perform the overlap check with the specified ground layer
        Collider[] colliders = Physics.OverlapBox(
            boxCenter,
            boxSize / 2,
            Quaternion.identity,
            groundLayer
        );

        return colliders.Length > 0;
    }

    private bool IsObstacleInDirection(Vector3 direction)
    {
        /*Vector3 boxCenter = boxCollider.bounds.center;
        Vector3 boxSize = boxCollider.bounds.size;
        float checkDistance = 0.1f; // Small distance to check in the given direction

        // Perform boxcast in the specified direction
        return Physics.BoxCast(
            boxCenter,
            boxSize / 2,
            direction,
            Quaternion.identity,
            checkDistance,
            obstacleLayer
        );*/

        // Calculate the box size based on the player's collider dimensions, slightly reduced
        Vector3 boxSize = new Vector3(
            boxCollider.bounds.size.x * 0.9f, // Slightly smaller width
            boxCollider.bounds.size.y * 0.9f, // Slightly smaller height
            boxCollider.bounds.size.z * 0.9f  // Slightly smaller depth
        );

        // Set the box center slightly offset in the specified direction
        Vector3 boxCenter = boxCollider.bounds.center + direction.normalized * 0.1f;

        // Check for any obstacles in the specified layer around the player
        Collider[] colliders = Physics.OverlapBox(
            boxCenter,
            boxSize / 2,
            Quaternion.identity,
            obstacleLayer
        );

        // Return true if any colliders are detected in the obstacle layer
        return colliders.Length > 0;
    }


    #endregion

    #region Changing State

    public void Start3DMovement()
    {
        Player_Entity.instance.Player_State_Manager.ChangePlayerState(Player_State_Manager.PlayerState.ThreeDimension);

        rigidbody.useGravity = true;
        LockRotation(false);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        characterModel.transform.localPosition = new Vector3(0, -0.5f, 0);
        characterModel.transform.localEulerAngles = new Vector3(0, 90, 0);
    }

    public void Start2DMovement()
    {
        Player_Entity.instance.Player_State_Manager.ChangePlayerState(Player_State_Manager.PlayerState.TwoDimension);

        rigidbody.useGravity = false;
        LockRotation(true);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        transform.eulerAngles = new Vector3(0, 0, 0);

        transform.transform.localPosition = transform.transform.localPosition + new Vector3(0, 0.1f, 0);

        characterModel.transform.localPosition = new Vector3(0, 0, -0.5f);
        characterModel.transform.localEulerAngles = new Vector3(90, 0, 0);
    }

    public void LockMovement(bool newState)
    {
        lock3DMovement = newState;
    }

    public void LockRotation(bool newState) 
    { 
        lock3DRotation = newState; 
    }

    public void SetMovement(bool state)
    {
        movementEnabled = state;
    }

    #endregion

    #region GetInput

    public Vector2 MovementInput()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    #endregion
}
