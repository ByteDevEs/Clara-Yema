using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class MandoDialogo : MenuAutoSeleccionManager
{
    public GameObject[] players;
    
    
    public bool uiUpdated = false;
    private void Update()
    {
        if(tiempo > 0)
        {
            tiempo -= Time.unscaledDeltaTime;
        }
        bool isUIOpen = false;
        foreach (var menu in menus) 
        {
            if(menu.gameObject.activeSelf)
            {
                isUIOpen = true;
                break;
            }
        }

        if(isUIOpen==uiUpdated)
            return;
        
        uiUpdated = isUIOpen;
        if (isUIOpen)
        {
            foreach (var player in players)
            {
                player.GetComponent<PlayerInput>().DeactivateInput();
            }
        }
        else
        {
            foreach (var player in players)
            {
                player.GetComponent<PlayerInput>().ActivateInput();
            }
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        base.OnMove(context);
    }
    
    [YarnCommand("close_dialogue")]
    public void CloseDialog() {
        print("Cerrando dialogo");
        foreach (var menu in menus) 
        {
            menu.gameObject.SetActive(false);
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.triggered && tiempo <= 0)
        {
            tiempo = tiempoEntreBotones;
            foreach (var menu in menus) 
            {
                if(menu.gameObject.activeSelf)
                {
                    menu.Select();
                    break;
                }
            }
        }
    }
}
