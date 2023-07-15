using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelLights
{
    public List<LevelLight> Lights;

    public void activate()
    {
        foreach (var L in Lights)
        {
            L.Light.intensity = L.intensity; 
            Debug.Log(L.Light.intensity);
        }
    }
}