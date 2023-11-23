using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TransitionCamera : MonoBehaviour
{
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Transform spawner;
    
    public void ResetPosition()
    {
        player.position = spawner.position;
        player.rotation = spawner.rotation;
        //Disable all cameras
        foreach (var cam in FindObjectsOfType<Camera>())
        {
            cam.gameObject.SetActive(false);
        }
        FindObjectOfType<ArenaTriggerDetector>().bossCamara.SetActive(true);
        Camera camera = FindObjectOfType<ArenaTriggerDetector>().bossCamara.GetComponent<BossCamFollowPlayers>().cam;
        camera.gameObject.SetActive(true);
    }
    
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
