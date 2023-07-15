using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;

public class GameOverVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private WaveController waveController;

    // Start is called before the first frame update
    void Start()
    {
        VFXReset();
        waveController.OnLoose.AddListener(ActivateVFX);
        waveController.OnReset.AddListener(VFXReset);
    }


    private void VFXReset()
    {
        vfx.Stop();
    }

    private void OnDestroy()
    {
        waveController.OnLoose.RemoveListener(ActivateVFX);
        waveController.OnReset.RemoveListener(VFXReset);
    }

    public void ActivateVFX()
    {
        vfx.Play();
    }
}
