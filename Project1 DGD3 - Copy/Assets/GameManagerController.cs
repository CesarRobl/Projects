using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManagerController : MonoBehaviour
{
    public static PlayerController pc;
    public static GameManagerController gm;
    public static EnemyController ec;
    public static CameraController cam;
    public GameObject steps;
    public List<Transform> ButtonLoc;
    public List<Transform> ButtonLoc2;
    public List<Transform> ButtonLoc3;
    public List<GameObject> Button;
    public List<E_Spawner> e_spawn;
    public List<GameObject> flashlight;

    public List<Image> StartEnd;

    public static bool FlashUpgrade;
    public PlayerMovementState movestate;
    public PlayerCrouching crouching;
    public EnemyState state;
    public SpawnState spawn;
    [Range(0, 20)] public float walkdelay, hometimer, spawntimer;

    private bool stop;
    [HideInInspector] public bool detected = false, chasing, free, dead, exhausted, nobattery,win;
    public bool arrived = false;
    [HideInInspector] public int spawnnumber;
    public float sensitivex, scrollwheel;
    public int buttonspressed;
    public Light light;
    [HideInInspector] public Light bulblight;
    public bool lights;
    public Vector3 Loc;
    private bool stops;
    private float timer, timer2, timer3, wintimer;
    private float alpha1 = 255, alpha2;

    void Start()
    {
        dead = false;
        spawn = SpawnState.Home;
        bulblight = ObjectController.obj.bulb.GetComponent<Light>();
        gm = this;
     
    }



    void Update()
    {
        Debug.Log(dead);
       
        if (dead)
        {
            StartEnd[1].color += new Color(0, 0, 0, .47f * Time.deltaTime);
            timer2 += Time.deltaTime;
            timer3 += Time.deltaTime;
            if (timer2 >= 4.5f)
                StartEnd[2].rectTransform.localScale += new Vector3(3.3f * Time.deltaTime, 3.3f * Time.deltaTime, 0);
            if (timer3 >= 5.5f) SceneManager.LoadSceneAsync(0);
        }

        StartEnd[0].color -= new Color(0, 0, 0, .2f * Time.deltaTime);
        if (!dead && !win)
        {
            if (!stops)
            {
                SpawnButton();
                stops = true;
            }

            PlayerSteps();
            EnemySpawns();
            Progress();
        }

        HideMouse();


    }

    public void Winscreen()
    {
        win = true;
        ObjectController.obj.win.color += new Color(0, 0, 0, 1.3f * Time.deltaTime);
        wintimer += Time.deltaTime;
        if(wintimer >= 2) ObjectController.obj.text.color  += new Color(0, 0, 0, 2f * Time.deltaTime);

    }


void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void PlayerSteps()
    {
       
        
        if (pc.rb.velocity != Vector3.zero) movestate = PlayerMovementState.Moving;
        else movestate = PlayerMovementState.Standing;
        if (movestate == PlayerMovementState.Moving && stop == false)
        {
            if (crouching != PlayerCrouching.Running)
            {
                pc.Audio2.Pause();
                pc.Audio.clip = ObjectController.obj.stepsound[0];
                pc.Audio.Play();
            }
            else 
            {
                pc.Audio.Pause();
                pc.Audio2.clip = ObjectController.obj.stepsound[1];
                pc.Audio2.Play();
            }
        }
        
        if (movestate == PlayerMovementState.Moving && stop == false)
        {
           

            steps = Instantiate(ObjectController.obj.steps, pc.transform.position, Quaternion.Euler(0, 0, 0));
            steps.transform.SetParent(pc.transform);
            steps.transform.localPosition = new Vector3(0,0,0);
            stop = true;
        }
        else if(movestate == PlayerMovementState.Standing)
        {
            pc.Audio2.Pause();
            pc.Audio.Pause();
            pc.Audio.clip = null;
            ec.footsteps = false;
            Destroy(steps);
            stop = false;
        }
        
        Vector3 runningsteps = new Vector3(14,3,14);
        Vector3 crouchingsteps = new Vector3(2.5f, 3, 2.5f);
        Vector3 normal = new Vector3(8, 3, 8);
        switch (crouching)
        {
            case PlayerCrouching.Running:
            {
               

                if(steps != null) steps.GetComponent<Transform>().localScale = runningsteps;
                break;
            }
            case PlayerCrouching.Crouching:
            {
               
                if(steps != null) steps.GetComponent<Transform>().localScale = crouchingsteps;
                break;
            }
            case PlayerCrouching.Nothing:
            {
               
                if(steps != null) steps.GetComponent<Transform>().localScale = normal;
                break;
            }
        }
    }

    void EnemySpawns()
    {
        if(spawn != SpawnState.Spawn && spawn != SpawnState.Spawned && spawn != SpawnState.Non)spawntimer += Time.deltaTime;
        if (spawntimer >= 3) spawn = SpawnState.Spawn;
        switch (spawn)
        {
            case SpawnState.Home:
            {
                SpawnHome();
                break;
            }
           
            case SpawnState.Spawn:
            {
                if(e_spawn.Count > 0)SpawnEnemy();
                break;
            }
            case SpawnState.Spawned:
            {
                RerollOrSpawn();
               
                break;
            }
            case SpawnState.Non:
            {
                hometimer += Time.deltaTime;
                if (hometimer >= 20)
                {
                    
                    spawn = SpawnState.Spawn;
                }
                break;
            }
        }
    }

    void SpawnHome()
    {
        
        hometimer = 0;
    }

    void SpawnEnemy()
    {
        arrived = false;
        spawntimer = 0;
        hometimer = 0;
        spawnnumber = Random.Range(0,e_spawn.Count);
        Loc = e_spawn[spawnnumber].transform.position;
        spawn = SpawnState.Spawned;
        
    }

    void RerollOrSpawn()
    {
        
        foreach (var i in e_spawn)
        {
            if (i.playerseen)
            {
                
            }
            else
            {
               

                if (!arrived)
                {
                    ec.nav.SetDestination(Loc);
                    
                }
                
            }
        }

        if (ec.transform.position == Loc + new Vector3(0,1.55f,0)|| state == EnemyState.Chasing || state == EnemyState.Searching)
        {
           
            arrived = true;
        }
        if(arrived)spawn = SpawnState.Non;
        // Debug.Log(Loc);
    }

    void SpawnButton()
    {
        Button[0].gameObject.transform.position = ButtonLoc[Random.Range(0,9)].position;
        Button[1].gameObject.transform.position = ButtonLoc2[Random.Range(0,8)].position;
        Button[2].gameObject.transform.position = ButtonLoc3[Random.Range(0,7)].position;
    }

    void Progress()
    {
        if (buttonspressed >= 1)
        {
            ObjectController.obj.guide[0].material = ObjectController.obj.green;
            ObjectController.obj.lightguide[0].color = Color.green;
        }
        if (buttonspressed >= 2)
        {
            ObjectController.obj.guide[1].material = ObjectController.obj.green;
            ObjectController.obj.lightguide[1].color = Color.green;
        }
        if (buttonspressed == 3)
        {
            ObjectController.obj.guide[2].material = ObjectController.obj.green;
            ObjectController.obj.lightguide[2].color = Color.green;
            foreach (var i in ObjectController.obj.bars)
            {
                i.transform.position -= new Vector3(0, 1 * Time.deltaTime, 0);
            }
        }
    }
    
    public enum PlayerMovementState
    {
      Moving = 0,
      Standing = 1,
      
    }

    public enum PlayerCrouching
    {
        Crouching = 1,
        Running = 2,
        Nothing = 3,
    }

    public enum EnemyState
    {
        Non = 0,
        Searching = 1,
        Patrolling = 2,
        Chasing = 3,
        Stunned = 4
    }
    public enum LightState
    {
        Normal = 1,
        Strong = 2,
    }

    public enum SpawnState
    {
        Home = 0,
        Spawn = 1,
        Spawned = 2,
        Non = 3,
    }
}
