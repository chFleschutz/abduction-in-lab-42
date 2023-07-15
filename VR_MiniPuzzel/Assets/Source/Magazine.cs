using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public int maxCapacity = 3;
    public int currentCapacity;

    private void Start()
    {
        currentCapacity = maxCapacity;
    }
}
