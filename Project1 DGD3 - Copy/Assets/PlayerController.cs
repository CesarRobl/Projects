using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    [HideInInspector] public float hp = 1;
    private Vector3 speed;
    [SerializeField]public bool walking;
    [SerializeField] public bool standing;
    [HideInInspector] public float zspeed = 10;
    void Awake()
    {
        GameManagerController.pc = this;
    }
    
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
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

    void Die()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        FieldOfView fov = other.gameObject.GetComponent<FieldOfView>();
        if (fov != null) GameManagerController.gm.detected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        FieldOfView fov = other.gameObject.GetComponent<FieldOfView>();
        if (fov != null) GameManagerController.gm.detected = false;
    }
}
