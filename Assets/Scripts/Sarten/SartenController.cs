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
    [SerializeField] private GameObject leftForkTrial;
    [SerializeField] private GameObject rightFork;
    [SerializeField] private GameObject rightForkTrial;
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
    private Vector3 initialPosition;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        players = FindObjectsOfType<PlayerController>().ToList().ConvertAll(x => x.gameObject);
        initialPosition = transform.position;
		AI();
    }

    private void Update()
    {
        if(playerInside.Count == 2)
            bothInside = true;
        else
            bothInside = false;
        
        fallingPropsList.RemoveAll(item => item == null);
        
        if(state == States.Dizzy || state == States.Mix || state == States.Smash)
            bossCollider.isTrigger = true;
        
        fallingPropsList.RemoveAll(item => item == null);

        if(state == States.Dizzy)
            animator.SetBool("Dizzy", true);
        else
            animator.SetBool("Dizzy", false);

        if (healthController.health <= 0)
        {
            state = States.Defeated;
            animator.SetTrigger("Dead");
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
                Invoke("CheckDizzy", 2f);
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

    public void EndAnimation()
    {
        Invoke("AI", attackDelay);
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
    
    void Smash()
    {
        animator.SetTrigger("Smash");
    }
    
    public void EndInnerAttack()
    {
        Invoke("AI", attackDelay);
        state = States.Awake;
        //Move all players upwards and make them land in one spawn point of the arena (the furthest one)
        Vector3[] spawners = GameObject.FindGameObjectsWithTag("Spawner").Select(x => x.transform.position).ToArray();
        
        //Remove the spawner which is blocked by a collision
        foreach (Vector3 spawner in spawners)
        {
            if (Physics.BoxCast(spawner, Vector3.one * 0.5f, Vector3.up, Quaternion.identity, 1f))
            {
                spawners = spawners.Where(x => x != spawner).ToArray();
            }
        }
        
        //Get the furthest spawner
        Vector3 furthestSpawner = spawners.OrderByDescending(x => Vector3.Distance(x, transform.position)).First();
        
        //Move the player to the furthest spawner, make an arc to the sky and then fall to the ground, use PlayerController
        foreach (GameObject player in players)
        {
            player.transform.position = furthestSpawner;
            player.GetComponent<CharacterController>().Move(Vector3.up * 10);
            //Move to the furthest spawner
            Vector3 direction = furthestSpawner - player.transform.position;
            player.GetComponent<CharacterController>().Move(direction.normalized * 10);
        }
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
        
        animator.enabled = false;
        
        if (playerNearest != null)
        {
            Vector3 initialPosition = leftFork.transform.position;
            Vector3 position = playerNearest.transform.position;
            Vector3 direction = position - leftFork.transform.position;
            direction.Normalize();
            position += (direction * 10f);
            Transform t = leftFork.transform;
            t.transform.forward = direction;
            t.transform.Rotate(90, 0, 0);
            
            leftForkTrial.transform.position = leftFork.transform.position;
            leftForkTrial.SetActive(true);
            StartCoroutine(MoveForkTime(leftFork, attackDelay/1.5f, playerNearest.transform.position, direction));
        }
        
        if (playerFurthest != null)
        {
            Vector3 initialPosition = rightFork.transform.position;
            Vector3 position = playerFurthest.transform.position;
            Vector3 direction = position - rightFork.transform.position;
            direction.Normalize();
            position += (direction * 10f);
            Transform t = rightFork.transform;
            t.transform.forward = direction;
            t.transform.Rotate(90, 0, 0);
            rightForkTrial.transform.position = rightFork.transform.position;
            rightForkTrial.SetActive(true);
            leftForkTrial.transform.position = playerFurthest.transform.position;
            StartCoroutine(MoveForkTime(rightFork, attackDelay/1.5f, playerFurthest.transform.position, direction));
        }
        
        animator.SetTrigger("LaunchFork");
        Invoke("AI", attackDelay+3f);
    }
    
    IEnumerator MoveForkTime(GameObject fork, float time, Vector3 position, Vector3 direction)
    {
        //Divide time in the time to go and the time to return
        float timeLeft = 1;
        //wait 2 seconds
        Vector3 playerNearest = GetNearestPlayer().transform.position;
        Vector3 playerFurthest = GetFurthestPlayer().transform.position;
        Vector3 initialRightPosition = rightFork.transform.position;
        Vector3 initialLeftPosition = leftFork.transform.position;
        while (timeLeft > 0)
        {
            //Lerp to the player
            
            rightForkTrial.transform.position = Vector3.Lerp(initialRightPosition, playerNearest, (1 - timeLeft) * 1.25f);
            leftForkTrial.transform.position = Vector3.Lerp(initialLeftPosition, playerFurthest, (1 - timeLeft) * 1.25f);
            timeLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        Vector3 initialPosition = fork.transform.position;
        while (timeLeft < time)
        {
            timeLeft += Time.deltaTime / 0.7f;
            Transform t = fork.transform;
            t.transform.forward = direction;
            t.transform.Rotate(90, 0, 0);
            
            fork.transform.rotation = Quaternion.Lerp(fork.transform.rotation, t.transform.rotation, timeLeft / time);
            fork.transform.position = Vector3.Lerp(fork.transform.position, position - direction * 0.5f, timeLeft / time);
            yield return new WaitForEndOfFrame();
        }
        //Lerp back
        timeLeft = 0;
        while (timeLeft < time)
        {
            timeLeft += Time.deltaTime / 0.3f;
            Transform t = fork.transform;
            t.transform.forward = direction;
            t.transform.Rotate(90, 0, 0);
            
            fork.transform.rotation = Quaternion.Lerp(fork.transform.rotation, t.transform.rotation, timeLeft / time);
            fork.transform.position = Vector3.Lerp(position - direction * 0.5f, initialPosition, timeLeft / time);
            yield return new WaitForEndOfFrame();
        }
        animator.enabled = true;
        leftForkTrial.SetActive(false);
        rightForkTrial.SetActive(false);
        leftForkTrial.transform.position = leftFork.transform.position;
        rightForkTrial.transform.position = rightFork.transform.position;
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
        Invoke("SpawnProp", 1.5f);
    }


    public void SpawnProp()
    {
        print("SpawnProp");
        if(fallingPropsList.Count > 1)
            return;
        int num = Random.Range(1, fallingProps.Length);
        for (int i = 0; i < num; i++)
        {
            int r = Random.Range(0, fallingProps.Length);
            //Circle spawn
            Vector3 spawnPos = new Vector3(Random.Range(-76.99f,-46.726f), 62.92f, Random.Range(-16.888f, 12.9f));
            GameObject gO = Instantiate(fallingProps[r], spawnPos + Vector3.up * 10, Quaternion.identity);
            fallingPropsList.Add(gO);
        }
    }
    
    private void MoveToPlayerStomp()
    {
        GameObject player = GetNearestPlayer();

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        StartCoroutine(MoveTimeStomp(0.5f, direction));
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
            if (!playerInside.Contains(other.gameObject))
            {
                playerInside.Add(other.gameObject);
            }
        }
        else if(other.CompareTag("Damager"))
        {
            if(state == States.Mix || state == States.Smash)
                healthController.TakeDamage(1);
        }
        else if (other.gameObject.CompareTag("BossFightProp"))
        {
            Destroy(other.gameObject);
            if(state == States.Dash)
                state = States.Dizzy;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BossFightProp"))
        {
            Destroy(other.gameObject);
            if(state == States.Dash)
                state = States.Dizzy;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BossFightProp"))
        {
            Destroy(other.gameObject);
            if(state == States.Dash)
                state = States.Dizzy;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside.Remove(other.gameObject);
            if(playerInside.Count == 0)
                bossCollider.isTrigger = false;
        }
        else if (other.gameObject.CompareTag("Arena"))
        {
            transform.position = initialPosition;
        }
    }
}
