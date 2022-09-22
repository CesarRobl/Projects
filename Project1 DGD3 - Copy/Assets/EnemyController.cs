using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyController : MonoBehaviour
{
    public NavMeshAgent nav;
    private bool footsteps;
    public LayerMask Ground, Player;
    private float patroltimer = 2;
    
    private bool arrived, detected, heard, stop, chasing = false;
    private float randomx, randomz;

    private bool locset;
    private float loctimer = 2;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

  
    void Update()
    {
        
      EnemyState();
     
    }

    void EnemyState()
    {
        if(!detected && !heard || stop) patroltimer -= Time.deltaTime;
        if (patroltimer <= 0 && !chasing)
        {
            GameManagerController.gm.state = GameManagerController.EnemyState.Patrolling;
            stop = true;
            
        }

        if (GameManagerController.gm.detected)
        {
            GameManagerController.gm.state = GameManagerController.EnemyState.Chasing;
        }
        switch (GameManagerController.gm.state)
        {
            case GameManagerController.EnemyState.Searching:
            {
                FollowSteps();
                break;
            }
            case GameManagerController.EnemyState.Patrolling:
            {
                Patrol();
                break;
            }
            case GameManagerController.EnemyState.Chasing:
            {
                nav.destination = GameManagerController.pc.transform.position;
                chasing = true;
                break;
            }
        }
    }

    void FollowSteps()
    {
        if (footsteps)
        {
            nav.destination = GameManagerController.pc.transform.position;
        }
    }

    void Patrol()
    {
        
        Vector3 randomloc = new Vector3(randomx, transform.position.y, randomz);
        if (!locset)
        {
            randomx = Random.Range(-10 + transform.position.x, 10 + transform.position.x);
            randomz = Random.Range(-10 + transform.position.z, 10 + transform.position.z);
            locset = true;
            arrived = false;
        }

        if (!arrived) nav.destination = randomloc;
        if (nav.destination == randomloc)
        {
            Debug.Log("I've arrived");
            arrived = true;
            locset = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManagerController.gm.steps != null)
        {
            footsteps = true;
            GameManagerController.gm.state = GameManagerController.EnemyState.Searching;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManagerController.gm.steps != null)
        {
            footsteps = false;
        }
    }
}
