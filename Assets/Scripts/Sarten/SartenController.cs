using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SartenController : MonoBehaviour
{
    public GameObject pala1, pala2;
    private Vector3 pala1Pos, pala2Pos;
    
    private List<GameObject> players = new List<GameObject>();
    
    // public bool isCooking = false;
    public bool bothInside = false;
    
    public int playerCount = 0;
    
    private void Start()
    {
        pala1Pos = pala1.transform.position;
        pala2Pos = pala2.transform.position;
    }
    
    private void Update()
    {
        if (!bothInside)
        {
            pala1.transform.position = Vector3.Lerp(pala1.transform.position, pala1Pos, Time.deltaTime);
            pala2.transform.position = Vector3.Lerp(pala2.transform.position, pala2Pos, Time.deltaTime);
        }
        else
        {
            pala1.transform.position = Vector3.Lerp(pala1.transform.position, players[0].transform.position + new Vector3(0, 0.25f, 0), Time.deltaTime);
            pala2.transform.position = Vector3.Lerp(pala2.transform.position, players[1].transform.position + new Vector3(0, 0.25f, 0), Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            players.Add(other.gameObject);
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
            players.Remove(other.gameObject);
            playerCount--;
            if (playerCount != 2)
            {
                bothInside = false;
            }
        }
    }
}
