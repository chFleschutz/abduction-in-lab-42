using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FilterType
{
    None,
    Diamond,
    Emerald,
    Ruby,
    Topas
}

public class ColorChangeLaser : MonoBehaviour
{
    public GameObject laserPrefab;
    public Color startColor;

    public Transform muzzle;
    public int reflectionLimit;
    public bool isAlwaysActive = false;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private Color currentLaserColor;
    private FilterType currentFilterType;

    private void Update()
    {
        if (isAlwaysActive || Input.GetMouseButtonDown(0))
            ShootRay();
    }

    public void ShootRay()
    {
        ResetLaser();

        var colorChangeCount= 0;
        var hitCount = 1;
        var rayOrigin = muzzle.position;
        var rayDirection = muzzle.forward;

        currentLaserColor = startColor;
        currentFilterType = FilterType.None;

        lineRenderers.Add(Instantiate(laserPrefab, rayOrigin, Quaternion.identity).GetComponent<LineRenderer>());
        lineRenderers[colorChangeCount].SetPosition(0, rayOrigin);
        lineRenderers[colorChangeCount].startColor = currentLaserColor;
        lineRenderers[colorChangeCount].endColor = currentLaserColor;

        for (var i = 0; i < reflectionLimit + 1; i++)
        {
            lineRenderers[colorChangeCount].positionCount = hitCount + 1;

            if (!Physics.Raycast(rayOrigin, rayDirection, out var hit, 100))
            {
                lineRenderers[colorChangeCount].SetPosition(hitCount, rayOrigin + rayDirection * 50);
                break;
            }

            // Add hitPoint for the line renderer
            lineRenderers[colorChangeCount].SetPosition(hitCount, hit.point);
           
            // Calculate where to go to next
            rayOrigin = hit.transform.position;
            
            // Check if target was hit
            var target = hit.transform.GetComponent<LaserTarget>();
            if (target)
            {
                if (target.requiredFilterType != currentFilterType) 
                    break;

                target.OnBeingHit();
            }
            // Check if mirror was hit
            else if (hit.transform.gameObject.layer == 6)
            {
                rayDirection = Vector3.Reflect(rayDirection, hit.normal);
                hitCount++;
            }
            // Check if color changer was hit
            else if (hit.transform.gameObject.layer == 7)
            {
                // Start new laser at hit point with same direction
                rayOrigin += rayDirection * Vector3.Distance(hit.point, hit.transform.position);
                colorChangeCount++;
                hitCount = 1;

                var changer = hit.transform.GetComponent<ColorChanger>();
                currentLaserColor = changer.changeColor;
                currentFilterType = changer.filterType;

                lineRenderers.Add(Instantiate(laserPrefab, rayOrigin, Quaternion.identity).GetComponent<LineRenderer>());
                lineRenderers[colorChangeCount].positionCount = hitCount + 1;
                lineRenderers[colorChangeCount].SetPosition(0, rayOrigin);
                lineRenderers[colorChangeCount].transform.GetChild(0).GetComponent<ParticleSystem>().startColor = currentLaserColor;
                lineRenderers[colorChangeCount].startColor = currentLaserColor;
                lineRenderers[colorChangeCount].endColor = currentLaserColor;
            }
            else 
                break;
        }
    }

    private void ResetLaser()
    {
        for (var i = lineRenderers.Count - 1; i >= 0; i--)
        {
            Destroy(lineRenderers[i].gameObject);
        }
        lineRenderers.Clear();
    }
}
