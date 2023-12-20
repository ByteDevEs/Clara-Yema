using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class CinematicPlayer : MonoBehaviour
{
    public bool PlayOnStart = false;
    public GameObject bgMusic;
    
    public VideoClip videoClipStart;
    private GameObject[] players;

    private void Awake()
    {
        if (PlayOnStart)
            PlayVideo(videoClipStart);
        GetComponent<VideoPlayer>().loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        bgMusic.GetComponent<AudioVolume>().multiplier = 1;
        
        vp.Stop();
        vp.clip = null;
        
        //Enable player movement
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
        
        //Enable dash
        FindObjectOfType<Dash>().enabled = true;
        
        //Enable explosion
        FindObjectOfType<Explosion>().enabled = true;
        
        //Enable clara encoger
        FindObjectOfType<ClaraEncoger>().enabled = true;
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            GetComponent<VideoPlayer>().playbackSpeed = 3;
        }
        else
        {
            GetComponent<VideoPlayer>().playbackSpeed = 1;
        }
    }

    public void PlayVideo(VideoClip clip)
    {
        GetComponent<VideoPlayer>().clip = clip;
        GetComponent<VideoPlayer>().Play();
        GetComponent<VideoPlayer>().SetDirectAudioVolume(0, GameSettings.musicVolume * GameSettings.masterVolume);
        bgMusic.GetComponent<AudioVolume>().multiplier = 0;
        
        //Disable player movement
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
        
        //Disable dash
        FindObjectOfType<Dash>().enabled = false;
        
        //Disable explosion
        FindObjectOfType<Explosion>().enabled = false;
        
        //Disable clara encoger
        FindObjectOfType<ClaraEncoger>().enabled = false;
    }
}
