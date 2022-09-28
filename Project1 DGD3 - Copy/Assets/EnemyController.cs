using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class EnemyController : MonoBehaviour
{
    public NavMeshAgent nav;
    private bool footsteps;
    public LayerMask Ground, Player;
    private float patroltimer = 2;
    
    private bool arrived,  heard, stop,  ground = true, wall, playerfound;
    private float randomx, randomz;
    
    private bool locset,stuck;
    private float loctimer;
    private Vector3 randomloc;
    public float stucktimer;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        GameManagerController.ec = this;
    }

  
    void Update()
    {
        
      EnemyState();
     if(GameManagerController.gm.chasing)CheckforPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(randomloc, new Vector3(2,2,2));
    }

    void EnemyState()
    {
        if(playerfound! && !heard || stop) patroltimer -= Time.deltaTime;
        if (patroltimer <= 0 && !playerfound)
        {
            GameManagerController.gm.state = GameManagerController.EnemyState.Patrolling;
            stop = true;
            
        }

        if (GameManagerController.gm.detected)
        {
            GameManagerController.gm.state = GameManagerController.EnemyState.Searching;
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
                Chase();
                break;
            }
        }
    }

    void Chase()
    {
        nav.SetDestination(GameManagerController.pc.transform.position);
    }

    void CheckforPlayer()
    {
        Vector3 direction = transform.position - GameManagerController.pc.transform.position;
        
        if (Physics.Raycast(transform.position, direction))
        {
            
        }

    }

    void FollowSteps()
    {
        if (footsteps)
        {
            nav.SetDestination(GameManagerController.pc.transform.position);
        }
    }

    void Patrol()
    {
        
       randomloc = new Vector3(randomx, transform.position.y, randomz);
        if (!locset || !ground || wall || stuck)
        {
            randomx = Random.Range(Random.Range(-20,0) + transform.position.x, Random.Range(0,10) + transform.position.x);
            randomz = Random.Range(Random.Range(-20,0)+ transform.position.z, Random.Range(0,10) + transform.position.z);
            locset = true;
            arrived = false;
            stuck = false;
            stucktimer = 0;
        }

         stucktimer += Time.deltaTime;
        if (stucktimer >= 8)
        {
            stuck = true;
        }
        
         nav.SetDestination(randomloc);
        if (transform.position == randomloc)
        {
            stucktimer = 0;
            loctimer += Time.deltaTime;
        }

        if (loctimer >= 4)
        {
            locset = false;
            loctimer = 0;
        }
        if (Physics.Raycast(randomloc, -transform.up,10 ,LayerMask.NameToLayer("Ground")))
        {
            ground = true;
        }
        else ground = false;

        if (Physics.CheckBox(randomloc, new Vector3(1f, 1f, 1f), Quaternion.Euler(0, 0, 0), LayerMask.NameToLayer("Wall")))
        {
            wall = true;
        }
        else wall = false;
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
