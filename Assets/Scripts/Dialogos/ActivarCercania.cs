using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarCercania : MonoBehaviour
{
    public GameObject toActivate;

    public float cercania = 5;
    bool activated = false;
    PlayerController[] playerControllers;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
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
                    if(!activated)
                    {
                        activated = true;
                        toActivate.SetActive(true);
                        foreach (var p in playerControllers)
                        {
                            p.enabled = false;
                        }
                    }
            }
        }
    }

    public void EndDialogue()
    {
        toActivate.SetActive(false);
        foreach (var p in playerControllers)
        {
            p.enabled = false;
        }
    }
}
