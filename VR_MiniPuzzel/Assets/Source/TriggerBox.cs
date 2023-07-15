using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(BoxCollider))]
public class TriggerBox : MonoBehaviour
{
    [Tooltip("Only objects with this tag starts the trigger")]
    public string objectTag;

    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != objectTag)
            return;

        Debug.Log("Trigger " + gameObject.name + " was entered by " + other.name);
        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != objectTag)
            return;

        Debug.Log("Trigger " + gameObject.name + " was exited by " + other.name);
        onTriggerExit.Invoke();
    }
}
