using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SartenController : MonoBehaviour
{
    [Header("HealthSystem")]
    public int maxHealth = 100;
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
}
