using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class Robot : MonoBehaviour
{
    [Header("Character Settings")]
    public float animationSpeed;
    public float headHeight = 0.2f;
    public float footHeight = 0.1f;
    public float footRadius = 0.03f;
    public float maxSlopeAngle = 30.0f;
    public LayerMask footRayIgnore;

    [Header("Movement Settings")]
    public float movingTurnSpeed = 360;
    public float stationaryTurnSpeed = 180;
    public float jumpVelocity;
    public float airControl;
    [Tooltip("The time it takes after landing a jump to slow down")]
    public float frictionTime = 0.2f;

    [Header("Grab Settings")]
    [Range(0.0f, 1.0f)]
    public float holdingSpeedPenalty = 0.5f;
    [Range(0.0f, 1.0f)]
    public float holdingTurnRatePenalty = 0.3f;
    public float pickupRange = 0.3f;
    public float grabCooldown = 0.1f;

    [Header("Gem Settings")] 
    public Transform carryPoint;

    private RaycastHit footHit;
    private bool isGrounded;
    private float turnAmount;
    private float forwardAmount;
    private float groundedTime;
    private Vector3 input;
    private bool isHeld;
    private float jumpTimer;

    private Rigidbody heldObjectRigidbody = null;
    private FixedJoint objectJoint = null;
    private bool isHoldingObject = false;
    private float grabTimer;

    // Components
    private new Rigidbody rigidbody;
    private Interactable interactable;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        animator.speed = animationSpeed;
        interactable.onAttachedToHand += OnAttachedToHand;
    }

    private void Update()
    {
        jumpTimer -= Time.deltaTime;
        grabTimer -= Time.deltaTime;

        isHeld = interactable.attachedToHand != null;
        CheckGrounded();
        
        rigidbody.freezeRotation = !isHeld;
        if (!isHeld)
            FixRotation();
    }
    
    private void FixedUpdate()
    {
        groundedTime += Time.fixedDeltaTime;
        if (!isGrounded)
            groundedTime = 0; // reset timer

        if (!isGrounded || isHeld)
            return;

        // Lift character up 
        rigidbody.position = new Vector3(rigidbody.position.x, footHit.point.y, rigidbody.position.z);
    }

    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (!(Time.deltaTime > 0)) 
            return;

        var animationDelta = (animator.deltaPosition) / Time.deltaTime;
        animationDelta = Vector3.ProjectOnPlane(animationDelta, footHit.normal);

        if (isGrounded && jumpTimer < 0)
        {
            if (groundedTime < frictionTime) //slow down when first hitting the floor after a jump
            {
                var moveFac = Mathf.InverseLerp(0, frictionTime, groundedTime);
                var lerpV = Vector3.Lerp(rigidbody.velocity, animationDelta, moveFac * Time.deltaTime * 30);
                animationDelta.x = lerpV.x;
                animationDelta.z = lerpV.z;
            }

            if (isHoldingObject)
                animationDelta *= holdingSpeedPenalty;

            // adding a little downward force to keep him on the floor
            animationDelta.y += -0.2f;// rb.velocity.y;
            rigidbody.velocity = animationDelta;
        }
        else // in air
        {
            rigidbody.velocity += input * Time.deltaTime * airControl;
        }
    }

    public void OnAttachedToHand(Hand hand)
    {
        if (isHoldingObject)
           DropObject();
    }

    public void AddMovementInput(Vector3 move)
    {
        input = move;
        if (move.magnitude > 1.0f)
            move.Normalize();
        // To local space
        move = transform.InverseTransformDirection(move);

        if (isHoldingObject)
        {   // Don't rotate when object is grabbed
            forwardAmount = move.z;
            turnAmount = move.x;
        }
        else
        {   // Transform into forward and turn 
            forwardAmount = move.z;
            turnAmount = Mathf.Atan2(move.x, move.z);
            // Actually turn character
            ApplyExtraTurnRotation();
        }

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }

    public void Jump()
    {
        if (!isGrounded || isHoldingObject)
            return;

        isGrounded = false;
        jumpTimer = 0.1f;
        animator.applyRootMotion = false;
        rigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
    }

    private void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("Holding", isHeld);
        animator.SetBool("GrabBox", isHoldingObject);

        if (isGrounded) 
            return;

        animator.SetFloat("FallSpeed", Mathf.Abs(rigidbody.velocity.y));
        animator.SetFloat("Jump", rigidbody.velocity.y);
    }

    private void FixRotation()
    {
        // Remove Rotation around X- and Z-Axis to stand up right
        var rotation = transform.eulerAngles;
        rotation.x = 0.0f;
        rotation.z = 0.0f;
        var targetRotation = Quaternion.Euler(rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * (isGrounded ? 20 : 3));
    }

    private void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        if (isHoldingObject)
            turnSpeed *= holdingTurnRatePenalty;

        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    private void CheckGrounded()
    {
        isGrounded = false;
        if (isHeld)
            return;

        var hipToFeetRay = new Ray(transform.position + (Vector3.up * footHeight), Vector3.down);
        if (!Physics.SphereCast(hipToFeetRay, footRadius, out footHit, footHeight - footRadius, ~footRayIgnore))
            return; // hit nothing -> in air

        // Check for slope angle
        if (Vector3.Angle(Vector3.up, footHit.normal) <= maxSlopeAngle)
            isGrounded = true;
    }

    public void GrabObject()
    {
        if (isHeld || !isGrounded) // Don't even try 
            return;

        // Ray-cast in front
        var startPoint = transform.position + (Vector3.up * headHeight);
        if (!Physics.Raycast(startPoint, transform.forward, out var hit, pickupRange)) 
            return;
        
        if (hit.transform.tag != "InteractableBox")
            return;

        var obj = hit.transform.gameObject;
        if (!obj.GetComponent<Rigidbody>())
            return;

        isHoldingObject = true;
        heldObjectRigidbody = obj.GetComponent<Rigidbody>();

        objectJoint = gameObject.AddComponent<FixedJoint>();
        objectJoint.connectedBody = heldObjectRigidbody;
        grabTimer = grabCooldown;
    }

    public void DropObject()
    {
        isHoldingObject = false;
        heldObjectRigidbody = null;

        objectJoint.connectedBody = null;
        Destroy(objectJoint);
        grabTimer = grabCooldown;
    }

    public void ToggleGrab()
    {
        if (grabTimer > 0.0f)
            return;

        if (isHoldingObject)
        {
            DropObject();
        }
        else
        {
            GrabObject();
        }
    }
}
