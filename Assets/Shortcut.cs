using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Get all players
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            //Set the spawn position for each player
            foreach (PlayerController player in players)
            {
                player.transform.position = new Vector3(-47, 61.91f, 25);
            }
        }
    }
}
