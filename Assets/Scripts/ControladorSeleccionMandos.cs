using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControladorSeleccionMandos : MonoBehaviour
{
    public enum State
    {
        Clara,
        None,
        Yema
    }
    List<(PlayerInput input, State state)> players = new List<(PlayerInput, State)>();
    
    public int state = 0;
    public Animator P1Animator;
    public Animator P2Animator;
    public GameObject StartButton;
    
    public float tiempoEntreBotones = 0.2f;
    float tiempo;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        StartButton.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
        // StartButton add listener
        StartButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        
        //Find all the players
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        foreach (var player in players)
        {
            this.players.Add((player, State.None));
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnMoveP1(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
            return;
        
        Vector2 input = context.ReadValue<Vector2>();

        if(tiempo > 0)
            return;
        
        int dir = 0;
        if (input.x > 0.5f)
        {
            dir = -1;
        }
        else if (input.x < -0.5f)
        {
            dir = 1;
        }
        
        if(dir == 0)
            return;

        State savedState = State.None;
        players[0] = (players[0].input, (State)Mathf.Clamp((int)players[0].state - dir, 0, 2));
        tiempo = tiempoEntreBotones;
    }
    
    public void OnMoveP2(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
            return;
        
        Vector2 input = context.ReadValue<Vector2>();

        if(tiempo > 0)
            return;
        
        int dir = 0;
        if (input.x > 0.5f)
        {
            dir = -1;
        }
        else if (input.x < -0.5f)
        {
            dir = 1;
        }
        
        if(dir == 0)
            return;

        State savedState = State.None;
        players[1] = (players[1].input, (State)Mathf.Clamp((int)players[1].state - dir, 0, 2));
        tiempo = tiempoEntreBotones;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
            return;
        
        if(tiempo > 0)
        {
            tiempo -= Time.unscaledDeltaTime;
        }
        
        State playerState1 = players[0].state;
        P1Animator.SetInteger("State",(int)playerState1);
        
        
        State playerState2 = players[1].state;
        P2Animator.SetInteger("State",(int)playerState2);

        if (players.Count >= 2 && playerState1 != State.None && playerState2 != State.None)
        {
            if (playerState1 != playerState2)
            {
                StartButton.SetActive(true);
            }
            else
            {
                StartButton.SetActive(false);
            }
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    private string claraScheme;
    private InputDevice claraDevice;
    
    private string yemaScheme;
    private InputDevice yemaDevice;
    public void StartGame()
    {
        PlayerInput clara = players[0].state == State.Clara ? players[0].input : players[1].input;
        claraScheme = clara.currentControlScheme;
        claraDevice = clara.devices[0];
        PlayerInput yema = players[0].state == State.Yema ? players[0].input : players[1].input;
        yemaScheme = yema.currentControlScheme;
        yemaDevice = yema.devices[0];
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if(scene != SceneManager.GetSceneByBuildIndex(1))
            return;
        
        //Set the device to the player
        print(claraScheme + " " + yemaScheme);
        FindObjectOfType<ClaraEncoger>().gameObject.GetComponent<PlayerInput>().SwitchCurrentControlScheme(claraScheme, claraDevice);
        FindObjectOfType<Dash>().gameObject.GetComponent<PlayerInput>().SwitchCurrentControlScheme(yemaScheme, yemaDevice);
    }
}
