using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private Text textComponent;
    [SerializeField] private Text winTextComponent;
    [SerializeField] private WaveController waveController;
    [SerializeField] private LightingSwitcher lightSwitcher;
    // Start is called before the first frame update
    void Start()
    {
        TextReset();
        waveController.OnLoose.AddListener(ActivateText);
        waveController.OnReset.AddListener(TextReset);
        waveController.OnWin.AddListener(ActivateWinText);
    }

    private void TextReset()
    {
        textComponent.enabled = false;
        winTextComponent.enabled = false;
    }

    private void OnDestroy()
    {
        waveController.OnLoose.RemoveListener(ActivateText);
        waveController.OnReset.RemoveListener(TextReset);
        waveController.OnWin.RemoveListener(ActivateWinText);
    }

    public void ActivateText()
    {
        textComponent.enabled = true;
    }

    public void ActivateWinText()
    {
        winTextComponent.enabled = true;
        lightSwitcher.SetLevelLights(3);
    }
}
