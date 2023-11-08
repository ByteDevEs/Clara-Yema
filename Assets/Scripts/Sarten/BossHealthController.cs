using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : HealthController
{
    [SerializeField] GameObject bossCanvasPrefab;
    GameObject bossCanvas;
    
    Slider healthBar;

    private void Start()
    {
        bossCanvas = Instantiate(bossCanvasPrefab);
        healthBar = bossCanvas.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health/maxHealth;
    }
}
