using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class EnemyController : MonoBehaviour
{
    public NavMeshAgent nav;
    public AudioSource Enemy,Enemy2;
    
    public LayerMask Ground, Player;
    private float patroltimer = 2;
    
    [HideInInspector]public bool stun,  heard = false, stop,  ground = true, wall, playerfound,footsteps,isaw;
    private float randomx, randomz;
    private bool lol,play;
    private bool locset,stuck;
    private float loctimer;
    private Vector3 randomloc,direction;
    public float stucktimer,chasetimer,searchtimer;
    private Vector3 no;
 public float noisetimer, noisetimer2;
 public int random1, ran2;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        GameManagerController.ec = this;
    }

  
    void Update()
    {
       
        if (!GameManagerController.gm.win)
        {
            if (!GameManagerController.gm.dead)
            {
                direction = GameManagerController.pc.transform.position - transform.position;

                if (GameManagerController.gm.spawn != GameManagerController.SpawnState.Home) EnemyState();
                if (GameManagerController.gm.chasing) CheckforPlayer();
                KillRadius();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(randomloc, new Vector3(2,2,2));
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position,direction.normalized * 4.5f);
    }

    void EnemyState()
    {

        if (GameManagerController.gm.state != GameManagerController.EnemyState.Searching) play = false;
        if (playerfound && !stun) GameManagerController.gm.state = GameManagerController.EnemyState.Chasing;
        if(!playerfound && !lol && !stun|| stop && !stun) patroltimer -= Time.deltaTime;
        if (patroltimer <= 0 && !playerfound && !stun)
        {
            GameManagerController.gm.state = GameManagerController.EnemyState.Patrolling;
            stop = true;
            
        }

        if(lol && !playerfound && !stun )
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
            case GameManagerController.EnemyState.Stunned:
            {
                Stunned();
                break;
            }
        }
    }

    void Chase()
    {
        nav.angularSpeed = 250;
        nav.acceleration = 10;
        nav.speed = 7.5f;
        
        nav.SetDestination(GameManagerController.pc.transform.position);
        if (!isaw) chasetimer += Time.deltaTime;
        if (chasetimer >= 4f)
        {
            playerfound = false;
            GameManagerController.gm.state = GameManagerController.EnemyState.Patrolling;
            GameManagerController.gm.chasing = false;
            chasetimer = 0;
        }
    }

    void CheckforPlayer()
    {



        no = direction;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
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

    void KillRadius()
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 2.5f))
        {
            PlayerController pp = hit.collider.GetComponent<PlayerController>();
            if (pp != null) GameManagerController.gm.dead = true;
        }
    }
    void FollowSteps()
    {
        nav.angularSpeed = 190;
        nav.acceleration = 10;
        nav.speed = 5f;
        if (!play)
        {
            Enemy.clip = ObjectController.obj.EnemyAlert[Random.Range(0, ObjectController.obj.EnemyAlert.Count)];
            play = true;
        }
       Enemy.Play();
      
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
        nav.acceleration = 7;
        nav.speed = 10f;

        noisetimer2 -= Time.deltaTime;
        if (noisetimer2 <= 0)
        {
            Enemy2.clip = ObjectController.obj.EnemyIdle[Random.Range(0, ObjectController.obj.EnemyIdle.Count)];
            Enemy2.Play();
            noisetimer = 2;
        }
        
        if (GameManagerController.gm.arrived)
        {


            randomloc = new Vector3(randomx, transform.position.y, randomz);
            if (!locset || !ground || wall || stuck)
            {
                randomx = Random.Range(Random.Range(-20, 0) + transform.position.x,
                    Random.Range(0, 10) + transform.position.x);
                randomz = Random.Range(Random.Range(-20, 0) + transform.position.z,
                    Random.Range(0, 10) + transform.position.z);
                locset = true;

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

    void Stunned()
    {
        nav.SetDestination(transform.position);
     
        Debug.Log("I am stunned");
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
