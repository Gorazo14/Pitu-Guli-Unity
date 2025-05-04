using System;
using UnityEngine;
using GDL;

public class Target : MonoBehaviour 
{
    public event EventHandler<OnTargetHitEventArgs> OnTargetHit;
    public event EventHandler OnEnemyDeath;

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject healthBar;

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
            Invoke("Die", 2f);
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
            GetComponent<EnemyFind>().enabled = false;
            gun.SetActive(false);
            healthBar.SetActive(false);
        } 
    }
    public bool IsAlive()
    {
        return health > 0f;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}