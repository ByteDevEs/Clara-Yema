using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarCercania1 : MonoBehaviour
{
    public GameObject toActivate;
    [SerializeField] protected Animator animator;

    public float cercania = 5;
    bool activated = false;
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerControllers = new PlayerController[players.Length];
        dash = new Dash[players.Length];
        explosion = FindObjectOfType<Explosion>();
        claraEncoger = FindObjectOfType<ClaraEncoger>();
        for (int i = 0; i < players.Length; i++)
        {
            playerControllers[i] = players[i].GetComponent<PlayerController>();
            dash[i] = players[i].GetComponent<Dash>();
        }
        foreach (var p in playerControllers)
        {
            p.enabled = false;
            p.animator.SetFloat("Speed", 0);
            p.animator.SetBool("Jump", false);
            p.animator.SetBool("Duck", false);
            p.animator.SetBool("Explode", false);
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
            p.animator.SetFloat("Speed", 0);
            p.animator.SetBool("Jump", false);
            p.animator.SetBool("Duck", false);
            p.animator.SetBool("Explode", false);
        }
        foreach (var d in dash)
        {
            d.enabled = true;
        }
        explosion.enabled = true;
        claraEncoger.enabled = true;
        animator.SetBool("Alejar", true);
        FindObjectOfType<SartenController>().StartFight();
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
