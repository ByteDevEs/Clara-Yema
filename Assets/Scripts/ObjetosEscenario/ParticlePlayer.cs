using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystemDerecha;
    [SerializeField]
    private ParticleSystem particleSystemIzquierda;
    
    public void PlayIzq()
    {
        particleSystemIzquierda.Play();
    }
    
    public void PlayDer()
    {
        particleSystemDerecha.Play();
    }
}
