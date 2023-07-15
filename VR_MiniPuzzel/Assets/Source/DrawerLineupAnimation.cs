using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerLineupAnimation : MonoBehaviour
{
    public Animator DrawerAnimator;
    public string objectTag;

    private static float map(float value, float fromLow, float fromHigh, float toLow, float toHigh) 
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
    
    private void OnTriggerStay(Collider other)
    {

        if (other.tag != objectTag)
            return;
        DrawerAnimator.Play(0,0, map(other.gameObject.transform.position.z,-1.2f,1.5f,0,1) );
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != objectTag)
            return;
        DrawerAnimator.Play(0,0,1);
    }
}
