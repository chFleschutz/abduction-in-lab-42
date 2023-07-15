using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Valve.Newtonsoft.Json.Bson;
using Vector3 = UnityEngine.Vector3;

public class PositionManipulator : MonoBehaviour
{
    public Transform targetTransform;
    public float duration;

    private bool isEnabled = false;
    private Vector3 startPosition;
    private float elapsedTime;

    // Update is called once per frame
    private void Update()
    {
        if (!isEnabled)
            return;

        if (elapsedTime > duration)
        {
            isEnabled = false;
            return;
        }

        var t = Mathf.SmoothStep(0.0f, 1.0f, elapsedTime / duration); // Ease in and out
        transform.position = Vector3.Lerp(startPosition, targetTransform.position, t);
        elapsedTime += Time.deltaTime;
    }

    public void StartManipulator()
    {
        if (isEnabled)
            return;

        isEnabled = true;
        startPosition = transform.position;
        elapsedTime = 0.0f;
    }
}
