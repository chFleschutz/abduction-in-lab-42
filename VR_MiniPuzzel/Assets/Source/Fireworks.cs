using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Fireworks : MonoBehaviour
{
    [SerializeField] private VisualEffect fireworkEffect;
    [SerializeField] private WaveController waveController;

    // Start is called before the first frame update
    private void Start()
    {
        FireworksReset();
        waveController.OnWin.AddListener(StartFireworks);
        waveController.OnReset.AddListener(FireworksReset);
    }

    private void FireworksReset()
    {
        fireworkEffect.Stop();
    }

    private void OnDestroy()
    {
        waveController.OnWin.RemoveListener(StartFireworks);
        waveController.OnReset.RemoveListener(FireworksReset);
    }
    private void StartFireworks()
    {
        Debug.Log("Yeah we got here");
        fireworkEffect.Play();
    }
}
