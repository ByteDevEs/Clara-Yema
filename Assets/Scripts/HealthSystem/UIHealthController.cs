using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthController : HealthController
{
    [SerializeField]
    private Slider healthBar;

    private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health/maxHealth;
    }
}
