using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGripper : MonoBehaviour
{
    public float pushPullForce = 10.0f;
    public Transform holdPoint;
    public float pickupRange = 2.0f;
    public float maxHoldPointDeviation = 5.0f;

    private GameObject heldObject;
    private Rigidbody heldObjectRigidbody = null;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObjectRigidbody == null)
            {
                // Grab Object
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out var hit, pickupRange))
                {
                    if (hit.transform.tag == "InteractableBox")
                        GrabObject(hit.transform.gameObject);
                }
            }
            else
            {
                // Drop object
                DropObject();
            }
        }

        if (heldObject)
        {
            MoveObject();
        }
    }


    private void MoveObject()
    {
        var moveDirection = holdPoint.position - heldObject.transform.position;
        if (moveDirection.magnitude > maxHoldPointDeviation)
        {
            DropObject();
            return;
        }
        heldObjectRigidbody.AddForce(moveDirection * pushPullForce * moveDirection.magnitude);
    }

    private void GrabObject(GameObject obj)
    {
        if (!obj.GetComponent<Rigidbody>())
            return;

        heldObject = obj;
        heldObjectRigidbody = obj.GetComponent<Rigidbody>();
        heldObjectRigidbody.transform.parent = transform;
        heldObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        Debug.Log("Grabbed Object " + obj.name);
    }

    private void DropObject()
    {
        heldObjectRigidbody.transform.parent = null;
        heldObjectRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        heldObjectRigidbody = null;
        heldObject = null;

        Debug.Log("Dropped Object");
    }
}

