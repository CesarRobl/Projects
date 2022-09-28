using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public static PlayerController pc;
    public static GameManagerController gm;
    public static EnemyController ec;
    [HideInInspector] public GameObject[] step;
    public static bool FlashUpgrade;
    public PlayerMovementState movestate;
    public PlayerCrouching crouching;
    public EnemyState state;
    [Range(0, 20)] public float walkdelay;
    public GameObject steps;
    private bool stop;
    [HideInInspector] public bool detected, chasing;
    public float sensitivex;
    public Light light;
    public bool lights;
    
    void Awake()
    {
       
        gm = this;
    }

    
    void Update()
    {
        HideMouse();
        PlayerSteps();
       
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
            steps = Instantiate(ObjectController.obj.steps, pc.transform.position, Quaternion.Euler(0, 0, 0));
            steps.transform.SetParent(pc.transform);
            steps.transform.localPosition = new Vector3(0,0,0);
            stop = true;
        }
        else if(movestate == PlayerMovementState.Standing)
        {
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
        Searching = 1,
        Patrolling = 2,
        Chasing = 3,
    }
    public enum LightState
    {
        Normal = 1,
        Strong = 2,
    }
}
