using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SartenController : MonoBehaviour
{
    [Header("HealthSystem")]
    public int maxHealth = 100;
    public float health = 100;
    public Slider healthBar;
    [Header("Animations")]
    Animator animator;
    
    public bool bothInside = false;
    
    public int playerCount = 0;
    float timer = 0;
    
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
                animator.SetInteger("Stage", Random.Range(0, 3));
            }
        }
        healthBar.value = health/maxHealth;
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
