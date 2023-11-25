using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class SplashController : MonoBehaviour
{
    public void OnButton(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }
}
