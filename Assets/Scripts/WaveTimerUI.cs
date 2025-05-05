using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimerUI : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Start()
    {
        BattleSystem.Instance.OnWaveTimerChanged += BattleSystem_OnWaveTimerChanged;
    }

    private void BattleSystem_OnWaveTimerChanged(object sender, BattleSystem.OnWaveTimerChangedEventArgs e)
    {
        image.fillAmount = e.waveTimerNormalized;
    }
}
