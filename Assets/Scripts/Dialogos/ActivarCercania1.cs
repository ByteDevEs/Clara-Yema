using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarCercania1 : MonoBehaviour
{
    public GameObject toActivate;
    [SerializeField] protected Animator animator;

    public float cercania = 5;
    bool activated = false;
    private GameObject[] players;
    PlayerController[] playerControllers = new PlayerController[2];
    Dash[] dash;
    ClaraEncoger claraEncoger;
    Explosion explosion;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartDialogue()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        playerControllers = new PlayerController[players.Length];
        dash = new Dash[players.Length];
        explosion = new Explosion();
        claraEncoger = new ClaraEncoger();
        for (int i = 0; i < players.Length; i++)
        {
            playerControllers[i] = players[i].GetComponent<PlayerController>();
            dash[i] = players[i].GetComponent<Dash>();
        }
        explosion = FindObjectOfType<Explosion>();
        claraEncoger = FindObjectOfType<ClaraEncoger>();
        foreach (var p in playerControllers)
        {
            p.enabled = false;
        }
        foreach (var d in dash)
        {
            d.enabled = false;
        }
        explosion.enabled = false;
        claraEncoger.enabled = false;
    }

    public void EndDialogue()
    {
        toActivate.SetActive(false);
        foreach (var p in playerControllers)
        {
            p.enabled = true;
            p.gameObject.GetComponentInChildren<Animator>().SetFloat("Speed", 0);
            p.gameObject.GetComponentInChildren<Animator>().SetBool("Jump", false);
            p.gameObject.GetComponentInChildren<Animator>().SetBool("Duck", false);
            p.gameObject.GetComponentInChildren<Animator>().SetBool("Explode", false);
        }
        foreach (var d in dash)
        {
            d.enabled = true;
        }
        explosion.enabled = true;
        claraEncoger.enabled = true;
        animator.SetBool("Alejar", true);
        FindObjectOfType<SartenController>().state = SartenController.States.Awake;
    }

    /*public void StartTalking()
    {
        animator.SetBool("Hablando", true);
        Invoke("NoTalk", 1f);
    }

    IEnumerator NoTalk()
    {
        animator.SetBool("Hablando", false);
        return null;
    }*/
}
