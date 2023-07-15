using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveWithPlatform : MonoBehaviour
{
    public string robotTag = "Robot";
    Transform prevParent;

    private FixedJoint joint;
    private Rigidbody rigidbodyRobot;

    private void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    private void  OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == robotTag)
        {
            if (collider.gameObject.transform.parent == prevParent)
                return;

            //prevParent = collision.gameObject.transform.parent;
            //collision.gameObject.transform.parent = transform;

            rigidbodyRobot = collider.gameObject.GetComponent<Rigidbody>();
        }
    } 

    private void  OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == robotTag)
        {
            //collider.gameObject.transform.parent = prevParent;

            rigidbodyRobot = null;
        }
    }    

    public void SetFixedJoint()
    {
        Debug.Log("Connected Fixed Joint");
        joint.connectedBody = rigidbodyRobot;
    }

    public void UnsetFixedJoint()
    {
        Debug.Log("Disconnected Fixed Joint");
        rigidbodyRobot = null;
        joint.connectedBody = null;
    }
}

    // private void Start()
    // {
    //     // Avoid Robo getting moved into the ground
    //     previousPosition = transform.position;
    // }

    // private void  OnTriggerStay(Collider collision){
    //     if(collision.gameObject.tag == robotTag)
    //     {
    //         collision.gameObject.transform.position += new Vector3(0,0, (transform.position - previousPosition).z);
    //         previousPosition = transform.position;
    //     }
    // } 

    // private void  OnTriggerEnter(Collider collision){
    //     if(collision.gameObject.tag == robotTag)
    //     {
    //         previousPosition = transform.position;
    //     }
    // }    
// }
