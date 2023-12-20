using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    AudioSource audio;
    [HideInInspector]
    public float multiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        GameSettings.LoadGameSettings();
        audio.volume = GameSettings.musicVolume * GameSettings.masterVolume;
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = GameSettings.musicVolume * GameSettings.masterVolume * multiplier;
    }
}
