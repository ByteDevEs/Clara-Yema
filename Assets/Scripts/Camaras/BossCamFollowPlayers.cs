using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossCamFollowPlayers : MonoBehaviour
{
    public Camera cam;

    public CinemachineVirtualCamera animCam;
    public CinemachineVirtualCamera bossCam;
    

    public void EnableMovement()
    {
        GetComponent<Animator>().enabled = false;
        animCam.Priority = 0;
        bossCam.Priority = 1;
    }
    
    public void DisableMovement()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<Animator>().enabled = true;
        animCam.Priority = 1;
        bossCam.Priority = 0;
    }
}
