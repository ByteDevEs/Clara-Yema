using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HealthController))]
public class SartenController : MonoBehaviour
{
    public HealthController healthController;
    
    [Header("Animations")]
    Animator animator;
    
    
    public bool bothInside = false;
    
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
                if (healthController.GetHealth() > healthController.GetMaxHealth() / 2f)
                {
                    animator.SetInteger("Stage", Random.Range(0, 2));
                }
                else
                {
                    animator.SetInteger("Stage", Random.Range(0, 3));
                }
            }
        }
        if(healthController.GetHealth() <= 0)
        {
            //TEMP
            SceneManager.LoadScene(0);
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
}
