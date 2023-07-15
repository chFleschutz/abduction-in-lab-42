using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class KeyPickup : MonoBehaviour
{
    private Rigidbody rb;
    private Transform target;
    private bool followTarget = false;


    public float forceStrength = 20;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Throwable>().onPickUp.AddListener(Release);
    }

    //Add Trigger to our GameObject when Script is added
    private void Reset()
    {
        BoxCollider bc = gameObject.AddComponent( typeof(BoxCollider) ) as BoxCollider;
        bc.isTrigger = true;
        bc.size = new Vector3(3,3,3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!followTarget)
            return;

        rb.AddForce( forceStrength * (target.position - transform.position) );
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Robot")
            return;

        GameObject go = collision.gameObject;
        target = go.GetComponent<Robot>().carryPoint;

        followTarget = true;
    }

    private void Release()
    {
        followTarget = false;
    }

    public void InsertedKey()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject, 2);
    }
}
