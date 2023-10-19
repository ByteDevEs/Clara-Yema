using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SartenHealthController : MonoBehaviour
{
    SartenController sartenController;
    // Start is called before the first frame update
    void Start()
    {
        sartenController = GetComponentInParent<SartenController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Pala"))
        {
            if (sartenController.bothInside)
            {
                sartenController.TakeDamage(Time.deltaTime);
            }
        }
    }
}
