using System;
using UnityEngine;

[Serializable]
public class PostData
{
    public Transform Transform;
    [Range(0f, 100f)]
    public int Probability = 100;
}
