using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransform : MonoBehaviour
{
    public Transform resetTransform;

    public bool resetPosition = true;
    public bool resetRotation = true;
    public bool resetScale = false;

    public void ResetGameObject()
    {
        if (resetPosition)
            transform.position = resetTransform.position;
        
        if (resetRotation)
            transform.rotation = resetTransform.rotation;
        
        if (resetScale)
            transform.localScale = resetTransform.localScale;
    }
}
