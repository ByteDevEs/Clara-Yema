﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    public void LoadMainMenu()
    {
        if (mainCanvas) { 
            SceneManager.LoadScene(1);
            Time.timeScale = 1f;

        }
    }

    private void Start()
    {
        GameSettings.LoadGameSettings();
       
    }
}
