using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossCamFollowPlayers : MonoBehaviour
{
    public List<Transform> players;
    public Camera cam;

    public CinemachineVirtualCamera animCam;
    public CinemachineVirtualCamera bossCam;
    
    private void OnEnable()
    {
        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    public void EnableMovement()
    {
        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }

        GetComponent<Animator>().enabled = false;
        animCam.Priority = 0;
        bossCam.Priority = 1;
    }
}
