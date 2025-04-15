using UnityEngine;
using UnityEngine.UI;

public class WaveDisplay : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public Text waveText;

    void Update()
    {
        if (waveSpawner != null && waveText != null)
        {
            waveText.text = "Wave: " + waveSpawner.GetCurrentWaveNumber() + "/" + waveSpawner.GetTotalWaves();
        }
    }
}