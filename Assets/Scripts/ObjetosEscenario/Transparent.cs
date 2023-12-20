using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerChangeShader : MonoBehaviour {

    public List<Renderer> objectsToChangeShader = new List<Renderer>();
    public Shader baseShader;
    public Shader fadeShader;

    void OnTriggerEnter(Collider other) {
        Debug.Log("Player has entered the trigger");

        if (other.gameObject.CompareTag("Player")) {
            foreach (Renderer renderer in objectsToChangeShader) {
                renderer.material.shader = fadeShader;
            }
        }
    }

    // void OnTriggerExit(Collider other) {
    //     Debug.Log("Player has exited the trigger");

    //     if (other.gameObject.CompareTag("Player")) {
    //         foreach (Renderer renderer in objectsToChangeShader) {
    //             renderer.material.shader = baseShader;
    //         }
    //     }
    // }
}
