using System;
using UnityEngine;

public class Target : MonoBehaviour 
{
    public event EventHandler<OnTargetHitEventArgs> OnTargetHit;

    public class OnTargetHitEventArgs : EventArgs
    {
        public float healthNormalized;
    }

    private float healthMax = 200f;
    private float health = 200f;

    public void TakeDamage(float ammount) 
    {
        health -= ammount;
        OnTargetHit?.Invoke(this, new OnTargetHitEventArgs
        {
            healthNormalized = health / healthMax
        });
        if (health <= 0f)
        {
            Die();
        } 
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}