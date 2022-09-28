using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    [HideInInspector] public float hp = 1;
    private Vector3 speed;
    [SerializeField]public bool charging,charged,flashed;
    public bool onlight;
    [HideInInspector] public float zspeed = 10, mousex, mousey,charge;
    
    void Awake()
    {
        GameManagerController.pc = this;
    }
    
    void Update()
    {
        PlayerMovement();
        Flashlight();
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
        
        if (Input.GetMouseButtonDown(0) && !charging)
        {
            if(!onlight)GameManagerController.gm.lights = true;
            else if (onlight) GameManagerController.gm.lights = false;

            if (GameManagerController.gm.lights) onlight = true;
            else if (!GameManagerController.gm.lights) onlight = false;
        }

        if (GameManagerController.gm.lights) GameManagerController.gm.light.range = 20;
        else GameManagerController.gm.light.range = 0;

        if (Input.GetMouseButton(1))
        {
            GameManagerController.gm.light.range = 0;
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
                GameManagerController.gm.bulblight.enabled = true;
                GameManagerController.gm.bulblight.intensity = 1;
                ObjectController.obj.FlashView.SetActive(true);
                flashed = true;
            }
            
            if(!charged && !flashed)GameManagerController.gm.light.range = 20;
            charge = 0;
        }
        
        if (flashed)
        {
            GameManagerController.gm.bulblight.intensity -= .001f;
            if (GameManagerController.gm.bulblight.intensity <= 0f)
            {
                GameManagerController.gm.bulblight.enabled = false;
                flashed = false;
                charged = false;
            }
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
