using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManagerController : MonoBehaviour
{
    public static PlayerController pc;
    public static GameManagerController gm;
    public static EnemyController ec;
    public static CameraController cam;
    public GameObject steps;
    public ParticleSystem steam;
    public List<Transform> buttonLoc;
    public List<Transform> buttonLoc2;
    public List<Transform> buttonLoc3;
    public List<GameObject> Button;
    public List<E_Spawner> e_spawn;
    public List<GameObject> flashlight;

    public List<Image> StartEnd;

    
    public PlayerMovementState movestate;
    public PlayerCrouching crouching;
    public EnemyState state;
    public SpawnState spawn;
    [Range(0, 20)] public float walkdelay, hometimer, spawntimer;

    private bool stop;
    [HideInInspector] public bool effect, chasing, breath, dead, exhausted, nobattery,win,enemyclose;
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
        buttonspressed = 0;
        win = false;
    }



    void Update()
    {

        Debug.Log(dead);
        
        if (dead) StartCoroutine(GameOver());
        
        StartEnd[0].color -= new Color(0, 0, 0, .2f * Time.deltaTime);
        if (StartEnd[0].color.a <= 0) ObjectController.obj.control.color -= new Color(0, 0, 0, 1.5f * Time.deltaTime);
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
            Lightburnout();
            Exhaust();
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

    public void Lightburnout()
    {
SoundManager.sound.LightBurn();
        if (nobattery && !effect)
        {
            steam.Play();
           
            effect = true;
        }

        if (steam.time >= 3)
        {
            steam.Stop();
            effect = false;
        }
       
    }

    void Exhaust()
    {
        if (exhausted && !breath)
        {
            SoundManager.sound.exhaust.Play();
            breath = true;
        }

        if (!exhausted && breath) breath = false;

    }
    
void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void PlayerSteps()
    {
       
        StepSounds();
        if (pc.rb.velocity != Vector3.zero) movestate = PlayerMovementState.Moving;
        else movestate = PlayerMovementState.Standing;
        
        if (movestate == PlayerMovementState.Moving && stop == false)
        {
           

            steps = Instantiate(ObjectController.obj.steps, pc.transform.position, Quaternion.Euler(0, 0, 0));
            steps.transform.SetParent(pc.transform);
            steps.transform.localPosition = new Vector3(0,0,0);
            stop = true;
        }
        else if(movestate == PlayerMovementState.Standing)
        {
          
            ec.footsteps = false;
            Destroy(steps);
            stop = false;
        }
      
        
        Vector3 runningsteps = new Vector3(18,3,18);
        Vector3 crouchingsteps = new Vector3(2.5f, 3, 2.5f);
        Vector3 normal = new Vector3(9.5f, 3, 9.5f);
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

    void StepSounds()
    {
        if (movestate == PlayerMovementState.Moving)
        {
               
            if (crouching == PlayerCrouching.Nothing)
            {
                if(pc.Audio2.isPlaying) pc.Audio2.Pause();
                if(!pc.Audio.isPlaying) pc.Audio.Play();
            }
            else if (crouching == PlayerCrouching.Running)
            {
                if(pc.Audio.isPlaying) pc.Audio.Pause();
                if(!pc.Audio2.isPlaying) pc.Audio2.Play();
            }
        }

        if (movestate == PlayerMovementState.Standing)
        {
            if(pc.Audio.isPlaying) pc.Audio.Pause();
            if(pc.Audio2.isPlaying) pc.Audio2.Pause();
            
            
        }
    }

    void EnemySpawns()
    {
        if(spawn != SpawnState.Spawn && spawn != SpawnState.Spawned && spawn != SpawnState.Non)spawntimer += Time.deltaTime;
        if (spawntimer >= 5.5f) spawn = SpawnState.Spawn;
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
        Button[0].gameObject.transform.position = buttonLoc[Random.Range(0,buttonLoc.Count)].position;
        Button[1].gameObject.transform.position = buttonLoc2[Random.Range(0,buttonLoc2.Count)].position;
        Button[2].gameObject.transform.position = buttonLoc3[Random.Range(0,buttonLoc3.Count)].position;
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

     IEnumerator GameOver()
    {
        foreach (var i in gm.flashlight)
        {
            i.SetActive(false);
        }
        pc.gameObject.SetActive(false);
       cam.transform.SetParent(ec.transform);
        cam.transform.localPosition = new Vector3(0 ,0, 2);
        cam.transform.LookAt(ec.transform);
        ec.nav.SetDestination(ec.transform.position);
        
        StartEnd[1].color += new Color(0, 0, 0, .47f * Time.deltaTime);
        
        yield return new WaitForSeconds(4.5f);
        JuiceController.jc.static1.SetActive(true);
        JuiceController.jc.juice.SetTrigger("DeathStatic");
            // StartEnd[2].rectTransform.localScale += new Vector3(3.3f * Time.deltaTime, 3.3f * Time.deltaTime, 0);
            yield return new WaitForSeconds(5.5f);
         SceneManager.LoadSceneAsync(0);
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
