using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 200f;

    public void TakeDamage(float amaount) 
    {
        health -= amaount;
        if (health <= 0f)
        {
            Die();
        } 
    }

    void Die()
    {
        Destroy(gameObject);
    }

}