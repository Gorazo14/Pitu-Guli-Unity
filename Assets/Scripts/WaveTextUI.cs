using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private float textLifetime = 2f;

    private void Start()
    {
        BattleSystem.Instance.OnNewWave += BattleSystem_OnNewWave;
    }

    private void BattleSystem_OnNewWave(object sender, System.EventArgs e)
    {
        Show();
        waveText.text = "WAVE " + BattleSystem.Instance.GetWaveCount().ToString();
        Invoke(nameof(Hide), textLifetime);
    }

    private void Show()
    {
        waveText.gameObject.SetActive(true);
    }
    private void Hide()
    {
        waveText.gameObject.SetActive(false);
    }
}
