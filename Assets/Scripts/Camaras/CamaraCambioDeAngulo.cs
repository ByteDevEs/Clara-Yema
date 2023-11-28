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
        public Vector3 posicion;
    }
    
    [SerializeField]
    private List<Angulo> angulos;
    
    [SerializeField]
    private Vector3 constraintMinimo = new Vector3(-100, -100, -100);
    [SerializeField]
    private Vector3 constraintMaximo = new Vector3(100, 100, 100);
    
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
        
        //Get the nearest points to the current position
        Angulo a = angulos.Where(x => x.posicion.x <= posicion.x).OrderByDescending(x => x.posicion.x).First();
        Angulo b = angulos.Where(x => x.posicion.x >= posicion.x).OrderBy(x => x.posicion.x).First();
        
        
        //Interpolate between them based on the current position on every axis
        Vector3 t = new Vector3(
            Mathf.InverseLerp(a.posicion.x, b.posicion.x, posicion.x),
            Mathf.InverseLerp(a.posicion.y, b.posicion.y, posicion.y),
            Mathf.InverseLerp(a.posicion.z, b.posicion.z, posicion.z)
        );
        
        //Interpolate between the rotations
        Vector3 rotacion = Vector3.Lerp(a.rotacion, b.rotacion, t.x);
        transform.rotation = Quaternion.Euler(rotacion);
    }

    private void OnDrawGizmos()
    {
        //Draw the zone limited by the minimum and maximum values
        Gizmos.color = Color.blue;
        //Draw 8 spheres to represent the zone
        Gizmos.DrawSphere(constraintMinimo, 1f);
        Gizmos.DrawSphere(constraintMaximo, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(constraintMinimo.x, constraintMinimo.y, constraintMaximo.z), 1f);
        Gizmos.DrawSphere(new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMinimo.z), 1f);
        Gizmos.DrawSphere(new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMinimo.z), 1f);
        Gizmos.DrawSphere(new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMaximo.z), 1f);
        Gizmos.DrawSphere(new Vector3(constraintMaximo.x, constraintMaximo.y, constraintMinimo.z), 1f);
        Gizmos.DrawSphere(new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMaximo.z), 1f);
        
        //Draw the 12 lines between the spheres forming a wireframe cube
        Gizmos.color = Color.green;
        Gizmos.DrawLine(constraintMinimo, new Vector3(constraintMinimo.x, constraintMinimo.y, constraintMaximo.z));
        Gizmos.DrawLine(constraintMinimo, new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMinimo.z));
        Gizmos.DrawLine(constraintMinimo, new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMinimo.z));
        Gizmos.DrawLine(new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMaximo.z), new Vector3(constraintMinimo.x, constraintMinimo.y, constraintMaximo.z));
        Gizmos.DrawLine(new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMaximo.z), new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMinimo.z));
        Gizmos.DrawLine(new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMaximo.z), new Vector3(constraintMaximo.x, constraintMaximo.y, constraintMaximo.z));
        Gizmos.DrawLine(new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMaximo.z), new Vector3(constraintMinimo.x, constraintMinimo.y, constraintMaximo.z));
        Gizmos.DrawLine(new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMaximo.z), new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMinimo.z));
        Gizmos.DrawLine(new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMaximo.z), new Vector3(constraintMaximo.x, constraintMaximo.y, constraintMaximo.z));
        Gizmos.DrawLine(new Vector3(constraintMaximo.x, constraintMaximo.y, constraintMinimo.z), new Vector3(constraintMinimo.x, constraintMaximo.y, constraintMinimo.z));
        Gizmos.DrawLine(new Vector3(constraintMaximo.x, constraintMaximo.y, constraintMinimo.z), new Vector3(constraintMaximo.x, constraintMinimo.y, constraintMinimo.z));
        Gizmos.DrawLine(new Vector3(constraintMaximo.x, constraintMaximo.y, constraintMinimo.z), new Vector3(constraintMaximo.x, constraintMaximo.y, constraintMaximo.z));
        
        //Draw the points
        Gizmos.color = Color.yellow;
        
        foreach (var angulo in angulos)
        {
            Gizmos.DrawSphere(angulo.posicion, 5f);
        }
    }
}
