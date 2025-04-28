using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 200f;
    [SerializeField] private float damage = 10f;
    private void Start()
    {
        EnemyShooter.OnAnyPlayerHit += EnemyShooter_OnAnyPlayerHit;
    }

    private void EnemyShooter_OnAnyPlayerHit(object sender, System.EventArgs e)
    {
        health -= damage;
    }
}
