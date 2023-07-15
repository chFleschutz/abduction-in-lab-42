using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoloClone : MonoBehaviour
{
    public Collider mirrorCollider;
    public float dissolveDuration = 1.0f;

    private float targetDissolveVal;
    private float elapsedTime;
    
    private List<Material> materialList = new List<Material>();

    private void Start()
    {
        InstantiateMaterialOf(transform);
        foreach (Transform child in transform)
        {
            InstantiateMaterialOf(child);
        }
    }

    private void OnDestroy()
    {
        foreach (var material in materialList)
        {
            Destroy(material);
        }
        materialList.Clear();
    }
    
    public void Hide()
    {
        SetDissolveTarget(1.0f);
        if (mirrorCollider)
            mirrorCollider.enabled = false;
    }

    public void Show()
    {
        SetDissolveTarget(0.0f);
        if (mirrorCollider)
            mirrorCollider.enabled = true;
    }

    private void SetDissolveTarget(float val)
    {
        if (Mathf.Approximately(val, targetDissolveVal))
            return;

        targetDissolveVal = val;
        if (elapsedTime < dissolveDuration) // Enable smooth transition while animation is not finished
        {
            elapsedTime = dissolveDuration - elapsedTime;
        }
        else
        {
            elapsedTime = 0.0f;
        }

        StartCoroutine(DissolveCo());
    }

    private IEnumerator DissolveCo()
    {
        // Set Dissolve Amount
        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            var dissolveVal = Mathf.Lerp(1.0f - targetDissolveVal, targetDissolveVal, elapsedTime / dissolveDuration);
            
            foreach (var material in materialList)
            {
                material.SetFloat("_DissolveAmount", dissolveVal);
            }

            yield return new WaitForSeconds(0.025f);
        }
    }

    private void InstantiateMaterialOf(Transform obj)
    {
        if (!obj)
            return;

        var meshRenderer = obj.GetComponent<MeshRenderer>();
        if (!meshRenderer)
            return;

        var material = Instantiate(meshRenderer.sharedMaterial);
        meshRenderer.material = material;
        materialList.Add(material);
    }
}
