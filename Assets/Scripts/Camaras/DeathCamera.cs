using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    [HideInInspector]
    public MuerteFueraRango muerteFueraRango;
    public void ResetPosition()
    {
        muerteFueraRango.ResetPosition();
    }
    
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
