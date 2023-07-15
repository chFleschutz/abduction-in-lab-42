using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeAnimation : MonoBehaviour
{
    int blendShapeCount;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    float blend = 0f;
    public float blendSpeed = 1f;
    bool blendActive = false;

    public int blendIndex = 0;

    void Awake ()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
        skinnedMesh = GetComponent<SkinnedMeshRenderer> ().sharedMesh;
    }

    public void StartAnimation ()
    {
        blendActive = true; 
    }

    void Update ()
    {
        if (blendActive) {
            if (blend < 100f) {
                skinnedMeshRenderer.SetBlendShapeWeight (blendIndex, blend);
                blend += blendSpeed;
            } else {
                blendActive = false;
            }
        }
    }
}