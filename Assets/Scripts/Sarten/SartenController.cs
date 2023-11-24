using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HealthController), typeof(CharacterController))]
public class SartenController : MonoBehaviour
{
    [Header("Forks")]
    [SerializeField] private GameObject leftFork;
    [SerializeField] private GameObject rightFork;
    
    public HealthController healthController;
    private CharacterController cController;
    private Animator animator;
    
    
    [HideInInspector]
    public bool bothInside = false;
    
    [SerializeField] [ReadOnly] int playerCount = 0;
    [SerializeField] [ReadOnly] float timer = 0;
    [SerializeField] float attackDelay = 0;
    [SerializeField] float dashSpeed = 5;

    private List<GameObject> players;

    public enum States
    {
        WaitingForPlayers,
        Awake,
        Stomp,
        LaunchSpatula,
        Mix,
        Smash,
        Dash,
        Dizzy,
        Defeated
    }

    public States state;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        cController = GetComponent<CharacterController>();
        players = FindObjectsOfType<PlayerController>().ToList().ConvertAll(x => x.gameObject);
		AI();
    }

    void GenerateNewOuterAttackState()
    {
        int rOuterAttack = Random.Range(0, 2);
        if (rOuterAttack == 0)
        {
            state = States.Stomp;
        }
        else
        {
            state = States.LaunchSpatula;
        }
    }
    
    void GenerateNewInnerAttackState()
    {
        int rInnerAttack = Random.Range(0, 2);
        if (rInnerAttack == 0)
        {
            state = States.Mix;
        }
        else
        {
            state = States.Smash;
        }
    }

    void GenerateNewState()
    {
        int rState = Random.Range(0, 2);
        if (rState == 0)
        {
            GenerateNewOuterAttackState();
        }
        else
        {
            state = States.Dash;
        }
    }
    
    private void AI()
    {
        switch (state)
        {
            case States.WaitingForPlayers:
                Invoke("AI", 0);
                return;
            case States.Awake:
                GenerateNewState();
                Invoke("AI", 0);
                return;
            case States.Stomp:
                Stomp();
                state = States.Awake;
                break;
            case States.LaunchSpatula:
                LaunchFork();
                state = States.Awake;
                break;
            case States.Dash:
                DashToPlayer();
                break;
            case States.Dizzy:
                break;
            case States.Mix:
                break;
            case States.Smash:
                break;
            case States.Defeated:
                break;
        }

        Invoke("AI", attackDelay);
    }

    private void LaunchFork()
    {
        GameObject playerNearest = GetNearestPlayer();
        GameObject playerFurthest = GetFurthestPlayer();
        
        
        if (playerNearest != null)
        {
            StartCoroutine(MoveForkTime(leftFork, attackDelay/1.5f, playerNearest.transform.position));
        }
        
        if (playerFurthest != null)
        {
            StartCoroutine(MoveForkTime(rightFork, attackDelay/1.5f, playerFurthest.transform.position));
        }
        
        animator.SetTrigger("LaunchFork");
    }
    
    IEnumerator MoveForkTime(GameObject fork, float time, Vector3 position)
    {
        animator.enabled = false;
        Vector3 initialPosition = fork.transform.position;
        Vector3 direction = position - fork.transform.position;
        direction.Normalize();
        position += (direction * 10f);
        Transform t = fork.transform;
        t.transform.forward = direction;
        t.transform.Rotate(90, 0, 0);
        //Divide time in the time to go and the time to return
        float timeLeft = 0;
        while (timeLeft < time)
        {
            timeLeft += Time.deltaTime / 0.7f;
            
            fork.transform.rotation = Quaternion.Lerp(fork.transform.rotation, t.transform.rotation, timeLeft / time);
            fork.transform.position = Vector3.Lerp(fork.transform.position, position - direction * 0.5f, timeLeft / time);
            yield return new WaitForEndOfFrame();
        }
        //Lerp back
        timeLeft = 0;
        while (timeLeft < time)
        {
            timeLeft += Time.deltaTime / 0.3f;
            
            fork.transform.rotation = Quaternion.Lerp(fork.transform.rotation, t.transform.rotation, timeLeft / time);
            fork.transform.position = Vector3.Lerp(position - direction * 0.5f, initialPosition, timeLeft / time);
            yield return new WaitForEndOfFrame();
        }
        animator.enabled = true;
    }

    private void Stomp()
    {
        GameObject player = GetNearestPlayer();

        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            transform.forward = direction;
            transform.Rotate(0, 90, 0);
        }

        animator.SetTrigger("Stomp");
    }
    
    
    Coroutine stompCoroutine;
    private void MoveToPlayerStomp()
    {
        GameObject player = GetNearestPlayer();

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        if(stompCoroutine == null)
            stompCoroutine = StartCoroutine(MoveTime(5f, direction));
    }
    
    private void StopStompMovement()
    {
        if(stompCoroutine != null)
        {
            StopCoroutine(stompCoroutine);
            stompCoroutine = null;
        }
    }

    private void DashToPlayer()
    {
        GameObject player = GetNearestPlayer();
            
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            transform.forward = direction;
            transform.Rotate(0, 90, 0);
            animator.SetTrigger("Dash");
            StartCoroutine(MoveTime(attackDelay/1.5f, direction));
        }
    }
    
    IEnumerator MoveTime(float time, Vector3 direction)
    {
        float timeLeft = time;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float speed = 1 - Mathf.Pow((time - timeLeft) / 3, 0.05f);
            cController.Move(direction * (dashSpeed * speed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        
        if(state != States.Dizzy)
            state = States.Awake;
    }

    private GameObject GetNearestPlayer()
    {
        GameObject nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }
    
    private GameObject GetFurthestPlayer()
    {
        GameObject furthestPlayer = null;
        float furthestDistance = 0;
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                furthestPlayer = player;
            }
        }
        return furthestPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;
            if (playerCount == 2)
            {
                bothInside = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BossFightProp"))
        {
            if(state == States.Dash)
                state = States.Dizzy;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount--;
            if (playerCount != 2)
            {
                bothInside = false;
            }
        }
    }
}
