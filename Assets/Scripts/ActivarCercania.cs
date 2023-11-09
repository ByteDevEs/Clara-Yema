using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarCercania : MonoBehaviour
{
    public GameObject toActivate;

    public float cercania = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if(Vector3.Distance(player.transform.position, transform.position) < cercania)
            {
                if(toActivate.activeSelf == false)
                    toActivate.SetActive(true);
            }
        }
    }
}
