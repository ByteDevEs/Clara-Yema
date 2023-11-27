using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuAutoSeleccionManager : MonoBehaviour
{
    public MenuAutoSeleccion[] menus;

    public float tiempoEntreBotones = 0.2f;
    public float tiempo;


    // Update is called once per frame
    public void Update()
    {
        if(menus.All(seleccion => seleccion == null))
            Destroy(this.gameObject);
        if(tiempo > 0)
        {
            tiempo -= Time.unscaledDeltaTime;
        }
        if(Mathf.Abs(input.y) < 0.5f)
            return;
        
        int dir = input.y > 0 ? -1 : 1;
        if (input.y != 0 && tiempo <= 0)
        {
            tiempo = tiempoEntreBotones;
            foreach (var menu in menus) 
            {
                if (menu.gameObject.activeSelf)
                {
                    print("Mover");
                    menu.Mover(dir);
                }
            }
        }
    }
    
    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.action.triggered && tiempo <= 0)
        {
            tiempo = tiempoEntreBotones;
            foreach (var menu in menus) 
            {
                if(menu.gameObject.activeSelf)
                {
                    menu.Volver();
                    break;
                }
            }
        }
    }
    
    private Vector2 input;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.triggered && tiempo <= 0)
        {
            tiempo = tiempoEntreBotones;
            foreach (var menu in menus) 
            {
                if(menu.gameObject.activeInHierarchy)
                {
                    menu.Select();
                    break;
                }
            }
        }
    }
}
