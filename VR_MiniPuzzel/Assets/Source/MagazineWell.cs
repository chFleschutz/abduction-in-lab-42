using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Socket))]
public class MagazineWell : MonoBehaviour
{
    // Note: Requires a collider with IsTrigger set to true

    public Magazine magazine { get; private set; }
    public float attachCooldown = 0.5f;

    private Socket socket;
    private float lastDetachTime;

    private void Start()
    {
        socket = GetComponent<Socket>();
        lastDetachTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (magazine) // Already Loaded
            return;

        magazine = other.gameObject.GetComponent<Magazine>();
        if (!magazine) // Not a magazine
            return;

        if (Time.time - lastDetachTime < attachCooldown)
            return;

        // Snap magazine into place
        Debug.Log("Mag attached");
        socket.Attach(magazine.gameObject);
    }

    public void DetachMagazine()
    {
        socket.Detach();
        magazine = null;
        lastDetachTime = Time.time;

        Debug.Log("Mag detached");
    }
}
