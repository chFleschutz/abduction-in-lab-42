using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HologramsToSpawn
{
    public FilterType type;
    public int amount;
}

public class HologramSpawner : MonoBehaviour
{
    public GameObject hologramPrefab;
    public Transform spawnPosition;
    public float delayBetweenSpawns;
    public Text hologramDisplayText;
    public List<HologramsToSpawn> spawnAmountPerTarget;
    public int spawnedHologramCount; // Public to set already spawned holograms in inspector

    private Dictionary<FilterType, int> spawnIndex = new Dictionary<FilterType, int>();
    private int totalHologramCount;

    private void Start()
    {
        totalHologramCount = spawnedHologramCount;
        foreach (var spawn in spawnAmountPerTarget)
        {
            if (spawnIndex.ContainsKey(spawn.type))
                continue;

            spawnIndex.Add(spawn.type, spawn.amount);
            totalHologramCount += spawn.amount;
        }
        UpdateHologramDisplay();
    }

    public void TargetWasHit(FilterType type)
    {
        if (!spawnIndex.ContainsKey(type))
            return;

        var amount = spawnIndex[type];
        spawnIndex.Remove(type);
        StartCoroutine(SpawnHologramsCo(amount));
    }

    private IEnumerator SpawnHologramsCo(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            Instantiate(hologramPrefab, spawnPosition.position, spawnPosition.rotation);
            spawnedHologramCount++;
            UpdateHologramDisplay();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    private void UpdateHologramDisplay()
    {
        hologramDisplayText.text = spawnedHologramCount + " / " + totalHologramCount;
    }
}
