using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    AudioSource audioSource;
    
    public AudioClip menuClip;
    public AudioClip gameClip;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayMenuMusic();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = GameSettings.getMusicVolume();
    }
    
    public void PlayMenuMusic()
    {
        audioSource.clip = menuClip;
        audioSource.Play();
    }
    
    public void PlayGameMusic()
    {
        audioSource.clip = gameClip;
        audioSource.Play();
    }
}