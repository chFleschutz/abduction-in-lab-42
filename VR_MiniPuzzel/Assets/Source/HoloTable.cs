using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloTable : MonoBehaviour
{
    public Collider holoVolume;
    public Transform holoTableOrigin;
    public Transform cloneAreaOrigin;

    public float scaleValue = 10.0f;
    public float gridSize = 0.1f;
    public float snapRotationAngle = 90.0f;

    private void Start()
    {
        holoVolume.isTrigger = true;
    }

    public Vector3 SnapToGridPos(Vector3 worldPos)
    {
        var localPos = holoTableOrigin.InverseTransformPoint(worldPos);
        localPos /= gridSize;
        localPos.x = Mathf.Round(localPos.x);
        localPos.y = Mathf.Round(localPos.y);
        localPos.z = Mathf.Round(localPos.z);
        localPos *= gridSize;
        return holoTableOrigin.TransformPoint(localPos);
    }

    public Vector3 SnapRotation(Vector3 rotation)
    {
        var relativeRotation = rotation - holoTableOrigin.eulerAngles;
        relativeRotation /= snapRotationAngle;
        relativeRotation.x = Mathf.Round(relativeRotation.x);
        relativeRotation.y = Mathf.Round(relativeRotation.y);
        relativeRotation.z = Mathf.Round(relativeRotation.z);
        return relativeRotation * snapRotationAngle + holoTableOrigin.eulerAngles;
    }

    public bool IsInsideVolume(Vector3 position)
    {
        return holoVolume.bounds.Contains(position);
    }
}
