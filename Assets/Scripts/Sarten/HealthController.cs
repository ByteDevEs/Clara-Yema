using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("HealthSystem")]
    [SerializeField] protected int maxHealth = 100;
    protected float health = 100;

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float deltaTime)
    {
        health -= deltaTime;
    }
    
    public float GetHealth()
    {
        return health;
    }
    
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
