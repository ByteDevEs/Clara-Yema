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
    PlayerController[] playerControllers;
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
        playerControllers = new PlayerController[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                playerControllers[i] = players[i].GetComponent<PlayerController>();
            }
            foreach (var p in playerControllers)
            {
                p.enabled = false;
            }
    }

    public void EndDialogue()
    {
        toActivate.SetActive(false);
        foreach (var p in playerControllers)
        {
            p.enabled = true;
        }
        animator.SetBool("Alejar", true);
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
