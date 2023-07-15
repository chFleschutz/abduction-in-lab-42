using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private Text textComponent;
    [SerializeField] private WaveController waveController;
    // Start is called before the first frame update
    void Start()
    {
        TextReset();
        waveController.OnLoose.AddListener(ActivateText);
        waveController.OnReset.AddListener(TextReset);
    }

    private void TextReset()
    {
        textComponent.enabled = false;
    }

    private void OnDestroy()
    {
        waveController.OnLoose.RemoveListener(ActivateText);
        waveController.OnReset.RemoveListener(TextReset);
    }

    public void ActivateText()
    {
        textComponent.enabled = true;
    }
}
