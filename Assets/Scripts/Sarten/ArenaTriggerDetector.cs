using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTriggerDetector : MonoBehaviour
{
    [SerializeField]
    private SartenController sarten;
    private int playerCount = 0;

    private void Update()
    {
        if (playerCount == 2)
        {
            sarten.state = SartenController.States.Awake;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerCount--;
        }
    }
}
