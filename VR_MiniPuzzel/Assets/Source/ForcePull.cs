using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ForcePull : MonoBehaviour
{
    public Hand hand;
    public SteamVR_Action_Boolean preparePullAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "PrepareForcePull");
    public float pullTriggerSpeed = 1.0f;

    public float flyDuration = 1.0f;
    public float maxPullDistance = 10.0f;
    public float pullCooldown = 1.0f;
    public float sphereCastRadius = 0.1f;

    private Vector3 lastPosition;
    private GameObject targetObject;
    private float timeSincePull;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForPullObject();
            PullTargetObject();
        }

        if (!preparePullAction.state) 
            OnPreparePullAction();
        if (preparePullAction.lastStateUp)
            ChangeTarget(null);

        lastPosition = hand.transform.position;
        timeSincePull += Time.deltaTime;
    }

    private void OnPreparePullAction()
    {
        CheckForPullObject();

        if ((hand.transform.position - lastPosition).magnitude / Time.deltaTime < pullTriggerSpeed) 
            return;
        if (timeSincePull < pullCooldown)
            return;
        
        PullTargetObject();
    }

    private void CheckForPullObject()
    {
        Debug.DrawRay(transform.position + (transform.right * sphereCastRadius), transform.forward * maxPullDistance, Color.white, 0.1f);
        Debug.DrawRay(transform.position + (transform.right * sphereCastRadius * -1.0f), transform.forward * maxPullDistance, Color.white, 0.1f);
        Debug.DrawRay(transform.position + (transform.up * sphereCastRadius), transform.forward * maxPullDistance, Color.white, 0.1f);
        Debug.DrawRay(transform.position + (transform.up * sphereCastRadius * -1.0f), transform.forward * maxPullDistance, Color.white, 0.1f);

        if (!Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out var hit, maxPullDistance))
        {
            ChangeTarget(null);
            return;
        }
        
        if (!hit.transform.gameObject.GetComponent<Throwable>()) 
            return;

        ChangeTarget(hit.transform.gameObject);
    }

    private void PullTargetObject()
    {
        if (!targetObject)
            return;

        var targetRb = targetObject.GetComponent<Rigidbody>();
        if (!targetRb)
            return;
        
        var velocityChange = CalcPullVelocity(targetObject.transform.position) - targetRb.velocity; // Takes current velocity of rb into account
        targetRb.AddForce(velocityChange, ForceMode.VelocityChange);

        ChangeTarget(null);
        timeSincePull = 0.0f;
    }

    private Vector3 CalcPullVelocity(Vector3 startPosition)
    {
        // Calculate start velocity in parabolic arc at startPosition to end up at this transforms position
        return (transform.position - startPosition - (0.5f * Physics.gravity * flyDuration * flyDuration)) / flyDuration;
    }

    private void ChangeTarget(GameObject newTarget)
    {
        // Update highlight
        if (targetObject)
            targetObject.SendMessage("OnHandHoverEnd", hand);
        
        if (newTarget)
            newTarget.SendMessage("OnHandHoverBegin", hand);
    
        targetObject = newTarget;
        Debug.Log("Force-Pull Target Object: " + targetObject);
    }
}
