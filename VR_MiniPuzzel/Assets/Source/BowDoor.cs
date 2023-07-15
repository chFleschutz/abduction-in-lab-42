using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowDoor : MonoBehaviour
{
    public bool OpenDoor;
    // Start is called before the first frame update
    void Start()
    {
        OpenDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(OpenDoor)
        {
            transform.position += Vector3.down * Time.deltaTime;

            if(transform.position.y < -0.9f)
            {
                OpenDoor = false;
            }
        }
    }

    public void OpenTheDoor()
    {
        OpenDoor = true;
    }
}
