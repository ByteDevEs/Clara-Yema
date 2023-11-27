using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public TMP_Text masterVolumeText;
    public Slider masterVolumeSlider;
    public TMP_Text musicVolumeText;
    public Slider musicVolumeSlider;
    public TMP_Text sfxVolumeText;
    public Slider sfxVolumeSlider;
    public TMP_Text targetFrameRateText;
    public Slider targetFrameRateSlider;
    public TMP_Dropdown qualityIndex;
    public TMP_Dropdown resolutionIndex;
    public Toggle isFullScreen;
    public Toggle isVSync;

    [SerializeField] GameObject opciones;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject selectCanvas;
    
    public void LoadHistoria()
    {
        SceneManager.LoadScene("EscenaHistoria");
        Play();
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadArcade()
    {
        SceneManager.LoadScene("SceneLaberinto");
        Play();
    }

    public void LoadCredits() 
    {
        SceneManager.LoadScene("Credits");
    }

    public void AbrirOpciones()
    {
        opciones.SetActive(true);
        mainCanvas.SetActive(false);
    }
    
    public void CerrarOpciones()
    {
        opciones.SetActive(false);
        mainCanvas.SetActive(true);
    }
    
    public void AbrirSeleccion()
    {
        selectCanvas.SetActive(true);
        mainCanvas.SetActive(false);
    }
    
    public void CerrarSeleccion()
    {
        selectCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }
    
    private void Start()
    {
        GameSettings.LoadGameSettings();
        masterVolumeText.text = GameSettings.masterVolume.ToString("0.00");
        masterVolumeSlider.value = GameSettings.masterVolume;
        musicVolumeText.text = GameSettings.musicVolume.ToString("0.00");
        musicVolumeSlider.value = GameSettings.musicVolume;
        sfxVolumeText.text = GameSettings.sfxVolume.ToString("0.00");
        sfxVolumeSlider.value = GameSettings.sfxVolume;
        targetFrameRateText.text = GameSettings.targetFrameRate.ToString("0");
        targetFrameRateSlider.value = GameSettings.targetFrameRate;
        
        qualityIndex.value = (int) GameSettings.quality;
        resolutionIndex.value = (int) GameSettings.screenResolution;
        isFullScreen.isOn = GameSettings.isFullScreen;
        isVSync.isOn = GameSettings.isVSync;
        
        qualityIndex.options.Clear();
        foreach (var quality in Enum.GetNames(typeof(GameSettings.Quality)))
        {
            qualityIndex.options.Add(new TMP_Dropdown.OptionData(quality));
        }

        qualityIndex.captionText.text = Enum.GetName(typeof(GameSettings.Quality), GameSettings.quality);
        
        resolutionIndex.options.Clear();
        foreach (var resolution in Enum.GetNames(typeof(GameSettings.ScreenResolution)))
        {
            resolutionIndex.options.Add(new TMP_Dropdown.OptionData(resolution));
            resolutionIndex.options[resolutionIndex.options.Count - 1].text = resolution.Replace("_", "");
        }
        
        resolutionIndex.captionText.text = Enum.GetName(typeof(GameSettings.ScreenResolution), GameSettings.screenResolution).Replace("_", "");
        
        masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
        targetFrameRateSlider.onValueChanged.AddListener(delegate { SetTargetFrameRate(); });
        qualityIndex.onValueChanged.AddListener(delegate { SetQuality(); });
        resolutionIndex.onValueChanged.AddListener(delegate { SetScreenResolution(); });
        isFullScreen.onValueChanged.AddListener(delegate { SetFullScreen(); });
        isVSync.onValueChanged.AddListener(delegate { SetVSync(); });
    }

    public void Play()
    {
        //MusicManager.Instance.PlayGameMusic();
    }

    public void SetMasterVolume()
    {
        GameSettings.masterVolume = masterVolumeSlider.value;
        masterVolumeText.text = masterVolumeSlider.value.ToString("0.00");
        SaveSettings();
    }
    
    public void SetMusicVolume()
    {
        GameSettings.musicVolume = musicVolumeSlider.value;
        musicVolumeText.text = musicVolumeSlider.value.ToString("0.00");
        SaveSettings();
    }
    
    public void SetSFXVolume()
    {
        GameSettings.sfxVolume = sfxVolumeSlider.value;
        sfxVolumeText.text = sfxVolumeSlider.value.ToString("0.00");
        SaveSettings();
    }
    
    public void SetQuality()
    {
        GameSettings.quality = (GameSettings.Quality) qualityIndex.value;
        QualitySettings.SetQualityLevel((int) GameSettings.quality);
        SaveSettings();
    }
    
    public void SetScreenResolution()
    {
        GameSettings.screenResolution = (GameSettings.ScreenResolution) resolutionIndex.value;
        string resolution = Enum.GetName(typeof(GameSettings.ScreenResolution), GameSettings.screenResolution);
        resolution = resolution.Replace("_", "");
        string[] resolutionSplit = resolution.Split('x');
        int width = int.Parse(resolutionSplit[0]);
        int height = int.Parse(resolutionSplit[1]);
        Screen.SetResolution(width, height, isFullScreen.isOn);
        SaveSettings();
    }
    
    public void SetTargetFrameRate()
    {
        GameSettings.targetFrameRate = targetFrameRateSlider.value;
        targetFrameRateText.text = targetFrameRateSlider.value.ToString("0");
        Application.targetFrameRate = (int) GameSettings.targetFrameRate;
        SaveSettings();
    }
    
    public void SetFullScreen()
    {
        GameSettings.isFullScreen = isFullScreen.isOn;
        string resolution = Enum.GetName(typeof(GameSettings.ScreenResolution), GameSettings.screenResolution);
        resolution = resolution.Replace("_", "");
        string[] resolutionSplit = resolution.Split('x');
        int width = int.Parse(resolutionSplit[0]);
        int height = int.Parse(resolutionSplit[1]);
        Screen.SetResolution(width, height, isFullScreen.isOn);
        SaveSettings();
    }
    
    public void SetVSync()
    {
        GameSettings.isVSync = isVSync.isOn;
        QualitySettings.vSyncCount = GameSettings.isVSync ? 1 : 0;
        SaveSettings();
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("masterVolume", GameSettings.masterVolume);
        PlayerPrefs.SetFloat("explosionVolume", GameSettings.sfxVolume);
        PlayerPrefs.SetFloat("musicVolume", GameSettings.musicVolume);
        PlayerPrefs.SetInt("quality", (int) GameSettings.quality);
        PlayerPrefs.SetInt("screenResolution", (int) GameSettings.screenResolution);
        PlayerPrefs.SetFloat("targetFrameRate", GameSettings.targetFrameRate);
        PlayerPrefs.SetInt("isFullScreen", GameSettings.isFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("isVSync", GameSettings.isVSync ? 1 : 0);
        PlayerPrefs.SetInt("isPostProcessing", GameSettings.isPostProcessing ? 1 : 0);
    }
    
    public void LoadSettings()
    {
        GameSettings.LoadGameSettings();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
