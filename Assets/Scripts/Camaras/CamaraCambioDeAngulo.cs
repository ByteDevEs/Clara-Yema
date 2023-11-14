using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamaraCambioDeAngulo : MonoBehaviour
{
    [System.Serializable]
    private class Angulo
    {
        [SerializeField]
        public Vector3 rotacion;
        [SerializeField]
        public float porciento;
    }
    
    [SerializeField]
    private List<Angulo> angulos;
    
    private enum Eje
    {
        X,
        Y,
        Z
    }
    
    [SerializeField]
    private Eje eje;
    [SerializeField]
    private float ejeMinimo = -10;
    [SerializeField]
    private float ejeMaximo = 10;
    
    [SerializeField]
    float porciento = 0;
    
    [SerializeField]
    GameObject[] targets;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posicion = Vector3.zero;
        foreach (var target in targets)
        {
            posicion += target.transform.position;
        }
        posicion /= targets.Length;
        switch (eje)
        {
            case Eje.X:
                porciento = Mathf.InverseLerp(ejeMinimo, ejeMaximo, posicion.x);
                break;
            case Eje.Y:
                porciento = Mathf.InverseLerp(ejeMinimo, ejeMaximo, posicion.y);
                break;
            case Eje.Z:
                porciento = Mathf.InverseLerp(ejeMinimo, ejeMaximo, posicion.z);
                break;
        }
        
        //Get the closest two angles to the current percentage
        Angulo a = angulos.Where(x => x.porciento <= porciento).OrderByDescending(x => x.porciento).First();
        Angulo b = angulos.Where(x => x.porciento >= porciento).OrderBy(x => x.porciento).First();
        
        //Interpolate between them
        float t = Mathf.InverseLerp(a.porciento, b.porciento, porciento);
        Vector3 rotacion = Vector3.Lerp(a.rotacion, b.rotacion, t);
        print(b.rotacion);
        transform.rotation = Quaternion.Euler(rotacion);
    }

    private void OnDrawGizmos()
    {
        switch (eje)
        {
            case Eje.X:
                Gizmos.DrawLine(new Vector3(ejeMinimo, transform.position.y, transform.position.z), new Vector3(ejeMaximo, transform.position.y, transform.position.z));
                break;
            case Eje.Y:
                Gizmos.DrawLine(new Vector3(transform.position.x, ejeMinimo, transform.position.z), new Vector3(transform.position.x, ejeMaximo, transform.position.z));
                break;
            case Eje.Z:
                Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, ejeMinimo), new Vector3(transform.position.x, transform.position.y, ejeMaximo));
                break;
        }
    }
}
