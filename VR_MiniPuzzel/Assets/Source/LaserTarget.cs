using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserTarget : MonoBehaviour
{
    public Material hitMaterial;
    public Material unhitMaterial;
    public MeshRenderer meshRenderer;
    public bool destroyOnHit = false;
    public FilterType requiredFilterType;

    public HologramSpawner holoSpawner;
    [Tooltip("Game objects to activate when target is hit and deactivate when its no longer hit")]
    public List<GameObject> activateOnHit;

    public UnityEvent onHit;
    public UnityEvent onNotHit;

    private bool wasHit = false;

    private void LateUpdate()
    {
        if (!wasHit)
            OnNotBeingHit();

        wasHit = false;
    }

    public void OnBeingHit()
    {
        meshRenderer.material = hitMaterial;
        wasHit = true;
        
        if (holoSpawner)
            holoSpawner.TargetWasHit(requiredFilterType);
        
        ActivateGameObjects(true);
        onHit.Invoke();

        if (destroyOnHit)
            Destroy(gameObject);
    }

    public void OnNotBeingHit()
    {
        meshRenderer.material = unhitMaterial;
        ActivateGameObjects(false);
        onNotHit.Invoke();
    }

    private void ActivateGameObjects(bool active)
    {
        foreach (var obj in activateOnHit)
        {
            if (obj)
                obj.SetActive(active);
        }
    }
}
