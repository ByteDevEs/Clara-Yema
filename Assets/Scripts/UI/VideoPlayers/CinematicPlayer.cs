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
    }

    public void PlayVideo(VideoClip clip)
    {
        GetComponent<VideoPlayer>().clip = clip;
        GetComponent<VideoPlayer>().Play();
        GetComponent<VideoPlayer>().SetDirectAudioVolume(0, GameSettings.musicVolume * GameSettings.masterVolume);
        bgMusic.GetComponent<AudioVolume>().multiplier = 0;
    }
}
