using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SartenController : MonoBehaviour
{
    // public bool isCooking = false;
    public bool bothInside = false;
    
    public int playerCount = 0;

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
