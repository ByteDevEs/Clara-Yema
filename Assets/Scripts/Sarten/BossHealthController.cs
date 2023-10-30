using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : HealthController
{
    [SerializeField] Slider healthBar;
    // Update is called once per frame
    void Update()
    {
        healthBar.value = health/maxHealth;
    }
}
