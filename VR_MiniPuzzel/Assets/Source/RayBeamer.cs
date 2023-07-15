using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayBeamer : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private int reflectionLimit;

    [SerializeField] private LineRenderer lineRenderer;
    private List<Vector3> hitPoints = new List<Vector3>();

    private float maxShootDuration = 0.1f;
    private float shootDuration = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pow");
            ShootRay();
        }
    }

    public void ShootRay()
    {
        if (shootDuration>maxShootDuration)
        {
            lineRenderer.positionCount = 0;
            return;
        }
        shootDuration += Time.deltaTime;

        RaycastHit raycastHit;
        Vector3 rayOrigin = shootingPoint.position;
        Vector3 rayDirection = shootingPoint.forward;

        hitPoints.Clear();
        hitPoints.Add(shootingPoint.position);// + shootingPoint.right * 0.5f + shootingPoint.up * -0.3f);

        for(int i = 0; i < reflectionLimit + 1; i++)
        {
            if (!Physics.Raycast(rayOrigin, rayDirection, out raycastHit, 100))
            {
                hitPoints.Add(rayOrigin + rayDirection * 50);
                //Debug.DrawRay(rayOrigin, rayDirection * 1000, Color.white, 5.0f);
                break;
            }

            //Debug.DrawRay(rayOrigin, rayDirection * raycastHit.distance, Color.red, 5.0f);

            // Add hitPoint for the line renderer
            hitPoints.Add(raycastHit.point);

            // Signal the target it has been hit
            LaserInteractable targetScript = raycastHit.transform.GetComponent<LaserInteractable>();

            if(targetScript != null)
            {
                targetScript.OnBeingHit();
            }

            // Check if mirror was hit
            if (!(raycastHit.transform.gameObject.layer == 6))
            {
                break;
            }


            // Set new origin and direction
            rayOrigin = raycastHit.point;
            rayDirection = Vector3.Reflect(rayDirection, raycastHit.normal);
            
        }

        lineRenderer.positionCount = hitPoints.Count; 
        for(int i = 0; i < hitPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, hitPoints[i]);
        }

    }

    public void RechargeRay()
    {
        lineRenderer.positionCount = 0;
        shootDuration = 0;
    }
}
