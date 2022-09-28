using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Light night;
    [HideInInspector] public float hp = 1;
    private Vector3 speed;
    [SerializeField]public bool charging,charged,flashed;
    public bool onlight;
    [HideInInspector] public float zspeed = 10, mousex, mousey,charge,nighttimer;
    
    void Awake()
    {
        GameManagerController.pc = this;
    }
    
    void Update()
    {
        PlayerMovement();
        Flashlight();
        Nightvision();
    }

    void PlayerMovement()
    {
        mousey = Mathf.Clamp(mousey, -90, 90);
        mousex = Input.GetAxis("Mouse X") * GameManagerController.gm.sensitivex;
        mousey =  -Input.GetAxis("Mouse Y") * GameManagerController.gm.sensitivex;
        transform.Rotate(0,mousex,0);
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            GameManagerController.gm.crouching = GameManagerController.PlayerCrouching.Running;
            zspeed = 20;
        }
        
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            GameManagerController.gm.crouching = GameManagerController.PlayerCrouching.Crouching;
            zspeed = 5;
        }
        else
        {
            GameManagerController.gm.crouching = GameManagerController.PlayerCrouching.Nothing;
            zspeed = 10;
        }

        vel = Vector3.Lerp(rb.velocity, vel, GameManagerController.gm.walkdelay * Time.deltaTime);
        rb.velocity = vel;
    }

    void Flashlight()
    {
        bool on;

        if (!charging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!onlight) GameManagerController.gm.lights = true;
                else if (onlight) GameManagerController.gm.lights = false;

                if (GameManagerController.gm.lights) onlight = true;
                else if (!GameManagerController.gm.lights) onlight = false;
            }

            if (GameManagerController.gm.lights)
            {
                GameManagerController.gm.light.range = 80;
                GameManagerController.gm.bulblight.intensity = .4f;
            }
            else
            {
                GameManagerController.gm.light.range = 0;
                GameManagerController.gm.bulblight.intensity = 0;
            }
        }

        

        if (Input.GetMouseButton(1))
        {
            GameManagerController.gm.lights = false;
            charging = true;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                charge += .3f;
            }

            if (charge >= 10)
            {
                charged = true;
                Debug.Log("I am charged");
            }
        }
        

        if (Input.GetMouseButtonUp(1))
        {
            charging = false;
            if (charged)
            {
                GameManagerController.gm.bulblight.type = LightType.Directional;
                GameManagerController.gm.bulblight.intensity = 1;
                ObjectController.obj.FlashView.SetActive(true);
                flashed = true;
            }
            
            if(!charged && !flashed)GameManagerController.gm.lights = true;

            charge = 0;
        }
        
        if (flashed)
        {
            GameManagerController.gm.bulblight.intensity -= .001f;
            if (GameManagerController.gm.bulblight.intensity <= 0f)
            {
                GameManagerController.gm.bulblight.type = LightType.Spot;
                flashed = false;
                charged = false;
            }
        }
        
    }

    void Nightvision()
    {
        if (!GameManagerController.gm.lights && !charging)
        {
            if(nighttimer <=5)nighttimer += Time.deltaTime;
            if (nighttimer >= 5)
            {
                if (night.intensity <= .8f) night.intensity += Time.deltaTime;
            }
        }

        if (GameManagerController.gm.lights || charging)
        {
            nighttimer = 0;
            night.intensity = 0;
        }
    }
    void Die()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        FieldOfView fov = other.gameObject.GetComponent<FieldOfView>();
        if (fov != null && !fov.flash)
        {
            GameManagerController.gm.chasing = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FieldOfView fov = other.gameObject.GetComponent<FieldOfView>();
        {
            if(fov != null && !fov.flash) GameManagerController.gm.chasing = false;
            
        }
        
    }
}
