using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuAutoSeleccionManager : MonoBehaviour
{
    public MenuAutoSeleccion[] menus;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float tiempoEntreBotones = 0.2f;
    public float tiempo;
    
    // Update is called once per frame
    void Update()
    {
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
                if(menu.gameObject.activeSelf)
                {
                    menu.Select();
                    break;
                }
            }
        }
    }
}
