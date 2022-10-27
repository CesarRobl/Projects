using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Light night;
    public Animation ani;
    public AudioSource Audio,Audio2,flash;
    [HideInInspector] public float hp = 1;
    private Vector3 speed;
    [SerializeField] public bool charging, charged, flashed, stun, stunenemy;
    public bool onlight;
    [HideInInspector] public float zspeed = 10, mousex, mousey, charge, nightimer;
    public bool ParentEnemy,running;

    void Start()
    {
        GameManagerController.pc = this;
        GameManagerController.gm.dead = false;
        if (GameManagerController.gm.light == null)
            GameManagerController.gm.lights = gameObject.GetComponentInChildren<Light>();
    }

    void Update()
    {
        
        if (!GameManagerController.gm.dead && !GameManagerController.gm.win)
        {
            // Debug.Log(GameManagerController.gm.chasing);
            PlayerMovement();
            Flashlight();
            NightVision();
            CheckSpawns();
            PlaySounds();
        }

        if (ParentEnemy) transform.SetParent(GameManagerController.ec.transform);
    }

    void PlaySounds()
    {
        if (running)
        {
            
        }
    }
    void PlayerMovement()
    {
        
        mousey = Mathf.Clamp(mousey, -90, 90);
        mousex = Input.GetAxis("Mouse X") * GameManagerController.gm.sensitivex;
        mousey = -Input.GetAxis("Mouse Y") * GameManagerController.gm.sensitivex;
        transform.Rotate(0, mousex, 0);
        Vector3 vel = new Vector3(0, 0, 0);
        speed = new Vector3(1.5f, 0, 0);
        Vector3 sidespeed = transform.right * zspeed;
        Vector3 fowardspeed = transform.forward * zspeed;

        if (Input.GetKey(KeyCode.W))
        {

            vel += fowardspeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            vel += -fowardspeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            vel += -sidespeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            vel += sidespeed;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !GameManagerController.gm.exhausted)
        {
            
            GameManagerController.gm.crouching = GameManagerController.PlayerCrouching.Running;
            zspeed = 10;
        }
       

        else if (Input.GetKey(KeyCode.LeftControl))
        {
            GameManagerController.gm.crouching = GameManagerController.PlayerCrouching.Crouching;
            zspeed = 3;
        }
        else
        {
            GameManagerController.gm.crouching = GameManagerController.PlayerCrouching.Nothing;
            zspeed = 7;
        }

        vel = Vector3.Lerp(rb.velocity, vel, GameManagerController.gm.walkdelay * Time.deltaTime);
        rb.velocity = vel;
    }

    void Flashlight()
    {
        bool on;
        if (!GameManagerController.gm.nobattery)
        {
            if (Input.GetMouseButtonDown(0) && !charging)

            {
                if (!onlight) GameManagerController.gm.lights = true;
                else if (onlight) GameManagerController.gm.lights = false;

                if (GameManagerController.gm.lights) onlight = true;
                else if (!GameManagerController.gm.lights) onlight = false;
                flash.Play();
            }
        }
        else
        {

            GameManagerController.gm.lights = false;
        }
        if (GameManagerController.gm.lights) GameManagerController.gm.light.range = 30;


        else GameManagerController.gm.light.range = 0;

        if (GameManagerController.gm.lights && !charged) GameManagerController.gm.bulblight.range = .5f;
        else if (!GameManagerController.gm.lights && !charged) GameManagerController.gm.bulblight.range = 0f;

       

        if (Input.GetMouseButton(1))
        {
            GameManagerController.gm.light.range = 0;
            GameManagerController.gm.bulblight.range = 0f;
          
            charging = true;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                charge += .3f;
            }

            if (charge >= 10)
            {
                charged = true;
               
            }
        }
        

        if (Input.GetMouseButtonUp(1))
        {
            charging = false;
            if (charged)
            {
                GameManagerController.gm.bulblight.range = 1f;
                GameManagerController.gm.bulblight.type = LightType.Directional;
                GameManagerController.gm.bulblight.intensity = 1;
                stun = true;
              
                flashed = true;
            }
            
            if( GameManagerController.gm.lights && !charged && !flashed)GameManagerController.gm.lights = true;
            
            charge = 0;
        }
        
        if (flashed)
        {
            GameManagerController.gm.bulblight.intensity -= .001f;
            if (GameManagerController.gm.bulblight.intensity <= 0f)
            {
                GameManagerController.gm.bulblight.type = LightType.Spot;
                GameManagerController.gm.bulblight.intensity = 1;
                flashed = false;
                charged = false;
            }
        }
        
    }

    void NightVision()
    {
        if (!charging && !GameManagerController.gm.lights) nightimer += Time.deltaTime;
        else
        {
            nightimer = 0;
            night.intensity = 0;
        }
       
        if (nightimer >= 2.5f)
        {
            if (night.intensity <= .5f) night.intensity += Time.deltaTime;
        }
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + transform.forward * 1, new Vector3(20,1.75f,20) * 2);
    }

    void CheckSpawns()
    {
      
    }
    IEnumerator stuntimer()
    {
        yield return new WaitForSeconds(1);
        stun = false;
    }

    

    private void OnTriggerStay(Collider other)
    {
        WinboxScript win = other.gameObject.GetComponent<WinboxScript>();
        if (win != null)
        {
            GameManagerController.gm.Winscreen();
        }
        
        boxcast box = other.gameObject.GetComponent<boxcast>();
        if (box != null  && !box.player)
        {
            GameManagerController.gm.enemyclose = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        FieldOfView fov = other.gameObject.GetComponent<FieldOfView>();
        {
            if(fov != null && !fov.flash) GameManagerController.gm.chasing = false;
            
        }
        
        boxcast box = other.gameObject.GetComponent<boxcast>();
        if (box != null  && !box.player)
        {
            GameManagerController.gm.enemyclose = false;
        }
        
    }
}
