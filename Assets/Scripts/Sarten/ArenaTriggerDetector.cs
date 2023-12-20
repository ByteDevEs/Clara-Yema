using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDawn.SplitScreen;
using UnityEngine;
using Yarn.Unity;

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

    public GameObject canvasTransition;
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
            StartCoroutine("SetDialog");
        }
    }
    
    IEnumerator SetDialog()
    {
        yield return new WaitForSeconds(0.3f);
     
        bossCamara.SetActive(true);
        bossCamara.GetComponent<Animator>().enabled = true;
        bossCamara.GetComponent<Animator>().SetTrigger("Start");
        //Teleport players
        foreach (var player in players)
        {
            player.transform.position = spawner[players.IndexOf(player)].transform.position;
            player.transform.rotation = spawner[players.IndexOf(player)].transform.rotation;
        }
        Dialogo.SetActive(true);
        try
        {
            Dialogo.GetComponent<DialogueRunner>().Stop();
            Dialogo.GetComponent<DialogueRunner>().ResetDialogue();
        }
        catch (Exception e) { }
    }
    
    public void ResetBossFight()
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
        FindObjectOfType<BossCamFollowPlayers>().DisableMovement();
        FindObjectOfType<SartenController>().StopFight();
        StartCoroutine("SetDialog");
        
        //sarten.StartFight();
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
