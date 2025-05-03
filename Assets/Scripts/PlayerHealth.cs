using GDL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event EventHandler<OnPlayerHealthChangedEventArgs> OnPlayerHealthChanged;
    public class OnPlayerHealthChangedEventArgs : EventArgs
    {
        public float healthNormalized;
    }

    private float maxHealth = 200f;
    [SerializeField] private float health = 200f;
    [SerializeField] private float damage = 10f;
    private void Start()
    {
        EnemyShooter.OnAnyPlayerHit += EnemyShooter_OnAnyPlayerHit;
    }

    private void EnemyShooter_OnAnyPlayerHit(object sender, System.EventArgs e)
    {
        health -= damage;
        OnPlayerHealthChanged?.Invoke(this, new OnPlayerHealthChangedEventArgs
        {
            healthNormalized = health / maxHealth
        });

        if (health <= 0f)
        {
            Debug.Log("GameOver!");
        }
    }
}
