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
    
    public LayerMask Ground, Player;
    private float patroltimer = 2;
    
    [HideInInspector]public bool arrived,  heard = false, stop,  ground = true, wall, playerfound,footsteps,isaw;
    private float randomx, randomz;
    private bool lol;
    private bool locset,stuck;
    private float loctimer;
    private Vector3 randomloc,direction;
    public float stucktimer,chasetimer,searchtimer;
    private Vector3 no;
    
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        GameManagerController.ec = this;
    }

  
    void Update()
    {
        direction = GameManagerController.pc.transform.position - transform.position;
        
      EnemyState();
     if(GameManagerController.gm.chasing)CheckforPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(randomloc, new Vector3(2,2,2));
        Gizmos.DrawRay(transform.position,no);
    }

    void EnemyState()
    {
        if (playerfound) GameManagerController.gm.state = GameManagerController.EnemyState.Chasing;
        if(!playerfound && !lol|| stop) patroltimer -= Time.deltaTime;
        if (patroltimer <= 0 && !playerfound)
        {
            GameManagerController.gm.state = GameManagerController.EnemyState.Patrolling;
            stop = true;
            
        }

        if(lol && !playerfound )
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
        if (!isaw) chasetimer += Time.deltaTime;
        if (chasetimer >= 4f)
        {
            playerfound = false;
            GameManagerController.gm.state = GameManagerController.EnemyState.Patrolling;
            chasetimer = 0;
        }
    }

    void CheckforPlayer()
    {
        nav.angularSpeed = 250;
        nav.acceleration = 10;
        nav.speed = 7.5f;
        Debug.Log(GameManagerController.gm.chasing);
         
        no = direction;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100))
        {
            PlayerController pc = hit.collider.GetComponent<PlayerController>();
            if (pc != null)
            {
                playerfound = true;
                isaw = true;
            }
            else isaw = false;

        }
        

    }

    void FollowSteps()
    {
        nav.angularSpeed = 190;
        nav.acceleration = 10;
        nav.speed = 5f;
        
        if (footsteps)
        {
            nav.SetDestination(GameManagerController.pc.transform.position);
            
        }
        else searchtimer += Time.deltaTime;

        if (searchtimer >= 4f)
        {
            GameManagerController.gm.state = GameManagerController.EnemyState.Patrolling;
            lol = false;
            Debug.Log("I've stopped");
            searchtimer = 0;
        }
    }

    void Patrol()
    {
        nav.angularSpeed = 120;
        nav.acceleration = 8;
        nav.speed = 3.5f;
        
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
        DetectController dc = other.gameObject.GetComponent<DetectController>();
        if (dc != null)
        {
            lol = true;
            footsteps = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DetectController dc = other.gameObject.GetComponent<DetectController>();
        if (dc != null)
        {
            footsteps = false;
        }
    }
}
