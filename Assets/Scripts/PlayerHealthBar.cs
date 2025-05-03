using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth.OnPlayerHealthChanged += PlayerHealth_OnPlayerHealthChanged;
    }

    private void PlayerHealth_OnPlayerHealthChanged(object sender, PlayerHealth.OnPlayerHealthChangedEventArgs e)
    {
        image.fillAmount = e.healthNormalized;
    }
}
