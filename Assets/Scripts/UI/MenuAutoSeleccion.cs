using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuAutoSeleccion : MonoBehaviour
{
    public List<Selectable> botones;
    [SerializeField] int index = 0;
    public float tiempoEntreBotones = 0.2f;
    float tiempo;

    // Start is called before the first frame update
    void Start()
    {
        botones[0].Select();
    }

    private Vector2 input;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            //If is a button
            if (botones[index] is Button)
            {
                Button boton = botones[index] as Button;
                boton.onClick.Invoke();
            }
            //If is a toggle
            else if (botones[index] is Toggle)
            {
                Toggle toggle = botones[index] as Toggle;
                toggle.isOn = !toggle.isOn;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        botones[index].Select();
        if(tiempo > 0)
        {
            tiempo -= Time.unscaledDeltaTime;
        }
        if(Mathf.Abs(input.y) < 0.2f)
            return;
        
        if(botones.Count == 0)
            return;
        int dir = input.y > 0 ? -1 : 1;
        if (input.y != 0 && tiempo <= 0)
        {
            //Deseleccionar el boton actual
            //botones[index].OnDeselect(null);
            tiempo = tiempoEntreBotones;
            index += dir;
            if (index < 0)
            {
                index = botones.Count - 1;
            }
            else if (index >= botones.Count)
            {
                index = 0;
            }
        }
    }
}
