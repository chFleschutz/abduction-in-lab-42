using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonPingPongAnimator : MonoBehaviour
{
    public Animator PingPongAnimator;
    public String ForwardAnimationName;
    public String BackwardAnimationName;

    void Start()
    {
        BackwardAnimation();
    }

    public void ForwardAnimation()
    {
        // Debug.Log(String.Format("{0} starting ForwardAnimation at {1}",
        //                         PingPongAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,
        //                         1-Math.Min(PingPongAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1) ));
        PingPongAnimator.Play(ForwardAnimationName,0,1-Math.Min(PingPongAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1));
        
    }
    public void BackwardAnimation()
    {
        // Debug.Log(String.Format("{0} starting BackwardAnimation at {1}",
        //                         PingPongAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,
        //                         1-Math.Min(PingPongAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1) ));
        PingPongAnimator.Play(BackwardAnimationName,0,1-Math.Min(PingPongAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime,1));
    }

}
