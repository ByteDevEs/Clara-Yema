using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    public static float masterVolume = 1;
    public static float sfxVolume = 1;
    public static float musicVolume = 1;
    
    public static float getSFXVolume()
    {
        var dbVolume = Mathf.Log10(((sfxVolume * masterVolume) + 0.2f) * 5);
        return dbVolume * 0.1f;
    }
    
    public static float getMusicVolume()
    {
        var dbVolume = Mathf.Log10(((musicVolume * masterVolume) + 0.2f) * 5);
        return dbVolume * 0.1f;
    }
    
    public enum Quality
    {
        Low,
        Medium,
        High,
        Ultra
    }
    
    public static Quality quality = Quality.High;
    
    public enum ScreenResolution
    {
        _800x600,
        _1024x768,
        _1280x720,
        _1920x1080,
        _2560x1440,
        _3840x2160
    }
    
    public static ScreenResolution screenResolution = ScreenResolution._1920x1080;
    
    public static float targetFrameRate = 60;
    
    public static bool isFullScreen = true;
    
    public static bool isVSync = true;
    
    public static bool isMotionBlur = true;
    
    public static bool isPostProcessing = true;

    public static void LoadGameSettings()
    {
        PlayerPrefs.GetFloat("masterVolume", 1f);
        PlayerPrefs.GetFloat("explosionVolume", 0.8f);
        PlayerPrefs.GetFloat("musicVolume", 0.6f);
        PlayerPrefs.GetInt("quality", 2);
        PlayerPrefs.GetInt("screenResolution", 3);
        PlayerPrefs.GetFloat("targetFrameRate", 60);
        PlayerPrefs.GetInt("isFullScreen", 1);
        PlayerPrefs.GetInt("isVSync", 1);
        PlayerPrefs.GetInt("isMotionBlur", 1);
        PlayerPrefs.GetInt("isPostProcessing", 1);
    }
}
