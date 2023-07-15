using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level0 : MonoBehaviour
{
    public Animator LiftAnimator;
    private bool lift;

    void Start()
    {
        DeactivateLift();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateLift()
    {
        // Debug.Log(String.Format("{0} starting Up at {1}",
        //                         LiftAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,
        //                         1-Math.Min(LiftAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1) ));
        LiftAnimator.Play("Up",0,1-Math.Min(LiftAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1));
        
    }
    public void DeactivateLift()
    {
        // Debug.Log(String.Format("{0} starting Down at {1}",
        //                         LiftAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,
        //                         1-Math.Min(LiftAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1) ));
        LiftAnimator.Play("Down",0,1-Math.Min(LiftAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1));
    }

}
