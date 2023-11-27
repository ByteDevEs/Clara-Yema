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
        if(StartButton != null)
            StartButton.SetActive(false);
        // StartButton add listener
        if(StartButton != null)
            StartButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        
        //Find all the players
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        foreach (var player in players)
        {
            this.players.Add((player, State.None));
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().name != "MenuPrincipal")
            return;
        int index = players.FindIndex(x => x.input.devices.Contains(context.control.device));
        
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
        players[index] = (players[index].input, (State)Mathf.Clamp((int)players[index].state - dir, 0, 2));
        print("Selected " + players[index].state);
        tiempo = tiempoEntreBotones;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "MenuPrincipal")
            return;
        
        if(tiempo > 0)
        {
            tiempo -= Time.unscaledDeltaTime;
        }
        
        State playerState1 = players[0].state;
        if(P1Animator != null)
            if(P1Animator.isActiveAndEnabled)
                P1Animator.SetInteger("State",(int)playerState1);
        
        
        State playerState2 = players[1].state;
        if(P2Animator != null)
            if(P2Animator.isActiveAndEnabled)
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
        else if(StartButton != null)
        {
            StartButton.SetActive(false);
        }
    }

    private string claraScheme = "";
    private InputDevice claraDevice;
    
    private string yemaScheme = "";
    private InputDevice yemaDevice;
    public void StartGame()
    {
        PlayerInput clara = players[0].state == State.Clara ? players[0].input : players[1].input;
        claraScheme = clara.currentControlScheme;
        claraDevice = clara.GetDevice<Gamepad>();
        PlayerInput yema = players[0].state == State.Yema ? players[0].input : players[1].input;
        yemaScheme = yema.currentControlScheme;
        yemaDevice = yema.GetDevice<Gamepad>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if(scene != SceneManager.GetSceneByName("EscenaHistoria"))
            return;
        
        //Set the device to the player
        print(claraScheme + " " + yemaScheme);
        if(claraScheme == "" || yemaScheme == "")
            return;
        GameObject.Find("Clara").GetComponent<PlayerInput>().SwitchCurrentControlScheme(claraScheme, claraDevice);
        GameObject.Find("Yema").GetComponent<PlayerInput>().SwitchCurrentControlScheme(yemaScheme, yemaDevice);
        
        //Destroy this object
        Destroy(this.gameObject);
    }
}
