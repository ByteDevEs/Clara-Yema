using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDawn.SplitScreen;
using UnityEngine;

public class ArenaTriggerDetector : MonoBehaviour
{
    [SerializeField]
    private SartenController sarten;
    
    public GameObject bossCamara;

    public GameObject Dialogo;

    [SerializeField]
    private GameObject walls;
    [SerializeField]
    AudioSource background;
    [SerializeField]
    AudioClip music;

    [SerializeField]
    private GameObject canvasTransition;
    [SerializeField]
    private List<GameObject> spawner;
    
    [SerializeField]
    private List<GameObject> players;
    private bool once = false;
    
    private void Update()
    {
        if (players.Count == 2 && !once)
        {
            once = true;
            sarten.bothInside = true;
            walls.SetActive(true);
            int count = 0;
            foreach (var player in players)
            {
                GameObject gO = Instantiate(canvasTransition, transform.position, Quaternion.identity);
                gO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                gO.GetComponent<TransitionCamera>().player = player.transform;
                gO.GetComponent<TransitionCamera>().spawner = spawner[count].transform;
                count++;
            }
            background.Stop();
            background.clip = music;
            background.Play();
            Dialogo.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!players.Contains(other.gameObject))
            {
                players.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (players.Contains(other.gameObject))
            {
                players.Remove(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("BossFightProp"))
        {
            Destroy(other.gameObject, 5f);
        }
    }
}
