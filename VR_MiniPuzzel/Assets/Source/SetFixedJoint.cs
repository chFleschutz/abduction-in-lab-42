using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFixedJoint : MonoBehaviour
{
    private FixedJoint joint;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
