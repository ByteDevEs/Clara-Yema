using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectDawn.SplitScreen;
using UnityEngine;
using Gizmos = UnityEngine.Gizmos;

public class CamaraCambioDeAngulo : MonoBehaviour
{
    [System.Serializable]
    private class Angulo
    {
        [SerializeField]
        public Vector3 rotacion;
        [SerializeField]
        public Vector3 posicion;
        [SerializeField]
        public float distancia = 15f;
    }
    
    [SerializeField]
    private List<Angulo> angulos;
    
    [SerializeField]
    private Vector3 constraintMinimo = new Vector3(-100, -100, -100);
    [SerializeField]
    private Vector3 constraintMaximo = new Vector3(100, 100, 100);
    
    [SerializeField]
    GameObject[] targets;
    
    private SplitScreenEffect splitScreenEffect;

    private void Start()
    {
        splitScreenEffect = GetComponentInChildren<SplitScreenEffect>();
        Vector3 posicion = Vector3.zero;
        foreach (var target in targets)
        {
            posicion += target.transform.position;
        }
        posicion /= targets.Length;

        //Rotate the camera to fit the nearest angle to the current position of the targets, smoothly between the nearest angles depending on the distance
        float minDistance = float.MaxValue;
        Angulo anguloMasCercano = null;
        foreach (var angulo in angulos)
        {
            float distancia = Vector3.Distance(posicion, angulo.posicion);
            if (distancia < minDistance)
            {
                minDistance = distancia;
                anguloMasCercano = angulo;
            }
        }
        transform.rotation = Quaternion.Euler(anguloMasCercano.rotacion);
        splitScreenEffect.Distance = anguloMasCercano.distancia;
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

        List<Angulo> angulosOrdenados = angulos.OrderBy(angulo => Vector3.Distance(posicion, angulo.posicion)).ToList();
        Angulo a = angulosOrdenados[0];
        Angulo b = angulosOrdenados[1];
        
        //Rotate the camera to fit the nearest angle to the current position of the targets, smoothly between the nearest angles depending on the distance
        float distanciaA = Vector3.Distance(posicion, a.posicion);
        float distanciaB = Vector3.Distance(posicion, b.posicion);
        float distanciaTotal = distanciaA + distanciaB;
        float porcentajeA = distanciaA / distanciaTotal;
        float porcentajeB = distanciaB / distanciaTotal;
        Quaternion r = Quaternion.Lerp(Quaternion.Euler(a.rotacion), Quaternion.Euler(b.rotacion), porcentajeA);
        float d = Mathf.Lerp(a.distancia, b.distancia, porcentajeA);
        transform.rotation = Quaternion.Lerp(transform.rotation, r, Time.deltaTime);
        splitScreenEffect.Distance = Mathf.Lerp(splitScreenEffect.Distance, d, Time.deltaTime);;
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
