using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabIndicator : MonoBehaviour
{
    public string objectTag = "Robot";
    private Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != objectTag)
            return;

        anim.Play("GrabBoxIndicatorUp");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != objectTag)
            return;
        if(!anim.isPlaying)
            anim.Play("GrabBoxIndicatorImageSwitch");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != objectTag)
            return;

        anim.Play("GrabBoxIndicatorDown");
    }
}
