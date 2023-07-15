using System;
using UnityEngine;

[Serializable]
public class Wave
{
    [Tooltip("When you tick this, the next Attribute is irrelevant")]
    public bool SpawnFromBothSpawners;
    [Tooltip("Choose the spawner form which the wave should spawn from, 0 being left and 1 being right")]
    [Range(0f, 1f)]
    public int Spawner;
    [Space]
    public bool RelativeToLastWave = true;
    public float TimeTillSpawn;
    public float TimeBetweenSpawns;
    public int NumberOfEnemies;
}
