using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject target;
    public float freq = 3;
    private float time = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( time > freq)
        {
            Vector3 pos = new Vector3(Random.Range(-5,5), Random.Range(0,10), Random.Range(-5,5));
            Instantiate(target, pos, new Quaternion());
            time = 0;
        }
        time += Time.deltaTime;

    }
}
