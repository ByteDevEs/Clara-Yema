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

[RequireComponent(typeof(HealthController))]
public class SartenController : MonoBehaviour
{
    [Header("Forks")]
    [SerializeField] private GameObject leftFork;
    [SerializeField] private GameObject rightFork;
    [SerializeField] private Collider bossCollider;
    
    public HealthController healthController;
    private Rigidbody rb;
    private Animator animator;
    
    [Header("Boss Fight")]
    [SerializeField]
    private GameObject[] fallingProps;
    private List<GameObject> fallingPropsList = new List<GameObject>();
    
    public bool bothInside = false;
    
    [SerializeField] [ReadOnly] private List<GameObject> playerInside;
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
        rb = GetComponent<Rigidbody>();
        players = FindObjectsOfType<PlayerController>().ToList().ConvertAll(x => x.gameObject);
		AI();
    }

    private void Update()
    {
        fallingPropsList.RemoveAll(item => item == null);

        if (state is not (States.Awake or States.Stomp or States.Dash or States.LaunchSpatula))
        {
            bossCollider.isTrigger = true;
        }

        if (healthController.health <= 0)
        {
            state = States.Defeated;
            animator.SetTrigger("Defeated");
            Invoke("GoToCredits", 5f);
        }
    }
    
    private void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
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
                break;
            case States.LaunchSpatula:
                LaunchFork();
                break;
            case States.Dash:
                DashToPlayer();
                break;
            case States.Dizzy:
                Invoke("CheckDizzy", 5f);
                break;
            case States.Mix:
                Mix();
                break;
            case States.Smash:
                Smash();
                break;
            case States.Defeated:
                break;
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
        int rState = Random.Range(0, 3);
        if (rState == 0)
        {
            if(fallingPropsList.Count > 1)
                state = States.Dash;
            else
                state = States.Stomp;
        }
        else if (rState == 1)
        {
            if(fallingPropsList.Count == 0)
                state = States.LaunchSpatula;
            else
                state = States.Dash;
        }
        else
        {
            if(fallingPropsList.Count > 0)
                state = States.Dash;
            else
                state = States.Stomp;
        }
    }
    
    private void Mix()
    {
        animator.SetTrigger("Mix");
    }
    
    public void EndMix()
    {
        Invoke("AI", attackDelay);
        state = States.Awake;
    }
    
    void Smash()
    {
        animator.SetTrigger("Smash");
    }
    
    public void EndSmash()
    {
        Invoke("AI", attackDelay);
        state = States.Awake;
    }

    private void CheckDizzy()
    {
        if(bothInside)
            GenerateNewInnerAttackState();
        else
            state = States.Awake;
        Invoke("AI", 0);
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
        Invoke("AI", attackDelay+1f);
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
        state = States.Awake;
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


    public void SpawnProp()
    {
        if(fallingPropsList.Count > 1)
            return;
        int num = Random.Range(1, fallingProps.Length);
        for (int i = 0; i < num; i++)
        {
            int r = Random.Range(0, fallingProps.Length);
            //Circle spawn
            float angle = Random.Range(0, 360);
            Vector3 spawnPos = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * Random.Range(15,20);
            GameObject gO = Instantiate(fallingProps[r], spawnPos + Vector3.up * 10, Quaternion.identity);
            fallingPropsList.Add(gO);
        }
    }
    
    Coroutine stompCoroutine;
    private void MoveToPlayerStomp()
    {
        GameObject player = GetNearestPlayer();

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        if(stompCoroutine == null)
            stompCoroutine = StartCoroutine(MoveTimeStomp(5f, direction));
    }
    
    IEnumerator MoveTimeStomp(float time, Vector3 direction)
    {
        float timeLeft = time;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float speed = 1 - Mathf.Pow((time - timeLeft) / 3, 0.05f);
            rb.MovePosition(transform.position + direction * (dashSpeed * speed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
    }
    
    private void StopStompMovement()
    {
        if(stompCoroutine != null)
        {
            StopCoroutine(stompCoroutine);
            stompCoroutine = null;
        }
        state = States.Awake;
        Invoke("AI", attackDelay);
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
            rb.MovePosition(transform.position + direction * (dashSpeed * speed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        if(state != States.Dizzy)
            state = States.Awake;
        Invoke("AI", attackDelay);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().rb.isKinematic = false;
            other.GetComponent<PlayerController>().rb.AddExplosionForce(100, transform.position, 10);
            if (!playerInside.Contains(other.gameObject))
            {
                playerInside.Add(other.gameObject);
                if (playerInside.Count == 2)
                {
                    bothInside = true;
                }
            }
        }
        else if(other.CompareTag("Damager"))
        {
            healthController.TakeDamage(1);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BossFightProp"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100000, transform.position, 100);
            if(state == States.Dash)
                state = States.Dizzy;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInside.Contains(other.gameObject))
            {
                StartCoroutine(DelayedRecompose(other));
                playerInside.Remove(other.gameObject);
                if (playerInside.Count != 2)
                {
                    bothInside = false;
                }
                if(playerInside.Count == 0)
                    bossCollider.isTrigger = false;
            }
        }
    }
    
    IEnumerator DelayedRecompose(Collider other)
    {
        yield return new WaitForSeconds(0.5f);
        other.GetComponent<PlayerController>().rb.isKinematic = true;
    }
}
