using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public FixedJoint joint;

    public GameObject storedObject { get; private set; }

    private void Start()
    {
        if (!joint)
            joint = GetComponent<FixedJoint>();
    }

    public void Attach(GameObject newObject)
    {
        if (storedObject)
            return;

        var objBody = newObject.GetComponent<Rigidbody>();
        if (!objBody) 
            return;

        storedObject = newObject;
        storedObject.transform.position = transform.position;
        storedObject.transform.rotation = transform.rotation;
        joint.connectedBody = objBody;
    }

    public void Detach()
    {
        joint.connectedBody = null;
        storedObject = null;
    }
}
