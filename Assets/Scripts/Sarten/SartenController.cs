using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SartenController : MonoBehaviour
{
    [Header("HealthSystem")]
    [SerializeField] int maxHealth = 100;
    float health = 100;
    [SerializeField] Slider healthBar;
    [Header("Animations")]
    Animator animator;
    
    public bool bothInside { get; private set; } = false;
    
    [SerializeField] [ReadOnly] int playerCount = 0;
    [SerializeField] [ReadOnly] float timer = 0;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        animator.SetBool("bothInside", bothInside);
        if (bothInside)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer = 0;
                if (health > maxHealth / 2f)
                {
                    animator.SetInteger("Stage", Random.Range(0, 2));
                }
                else
                {
                    animator.SetInteger("Stage", Random.Range(0, 3));
                }
            }
        }
        healthBar.value = health/maxHealth;
        
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;
            if (playerCount == 2)
            {
                bothInside = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount--;
            if (playerCount != 2)
            {
                bothInside = false;
            }
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
}
