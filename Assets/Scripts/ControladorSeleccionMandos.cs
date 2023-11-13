using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ControladorSeleccionMandos : MonoBehaviour
{
    public enum State
    {
        Clara,
        None,
        Yema
    }
    (InputDevice device,State state) P1;
    (InputDevice device,State state) P2;
    
    public int state = 0;
    public Animator P1Animator;
    public Animator P2Animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        P1.device = InputSystem.GetDevice("Controller 1");
        P1.state = State.None;
        P2.device = InputSystem.GetDevice("Controller 2");
        P2.state = State.None;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        
        int dir = input.x > 0 ? -1 : 1;

        State savedState = State.None;
        switch (context.control.device.name)
        {
            case "Controller 1":
                savedState = P1.state;
                P1.state = (State)Mathf.Clamp((int)P1.state + dir, 0, 2);
                if (P1.state == P2.state && P1.state != State.None)
                {
                    P1.state = savedState;
                }
                break;
            case "Controller 2":
                savedState = P2.state;
                P2.state = (State)Mathf.Clamp((int)P2.state + dir, 0, 2);
                if (P1.state == P2.state && P2.state != State.None)
                {
                    P2.state = savedState;
                }
                break;
        }
    }

    private void Update()
    {
        P1Animator.SetInteger("State", (int)P1.state);
        P2Animator.SetInteger("State", (int)P2.state);
        print(P1.state + " " + P2.state);
    }
}
