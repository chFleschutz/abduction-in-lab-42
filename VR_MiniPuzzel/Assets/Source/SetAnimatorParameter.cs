using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetAnimatorParameter : MonoBehaviour
{
    public Animator anim;
    public string parameter;

    public void SetBool(bool value)
    {
        anim.SetBool(parameter,value);
    }
}
