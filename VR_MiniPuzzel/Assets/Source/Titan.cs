using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titan : MonoBehaviour
{
    [SerializeField] private float risingSpeed;
    private bool startRising;

    // Start is called before the first frame update
    void Start()
    {
        startRising = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(startRising)
        {
            if(transform.position.y < 1.5f)
            {
                transform.position += Vector3.up * Time.deltaTime * risingSpeed;
            }
        }
    }

    public void StartRiseOfTheTitan()
    {
        startRising = true;
    }
}
