using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JuiceController : MonoBehaviour
{
    public static JuiceController jc;
    
    public Animation FlashlightWalk;
    public Animator juice;
    public Animator enemyclose;
    public AnimationCurve curve;

    public GameObject static1;
    
    private Vector3 diff;
    private bool walking, running,done;
    public float shake;

    private float distance, dist,statictimer;
    void Start()
    {
        jc = this;
        Lighting();
    }

    
    void Update()
    {
       diff = GameManagerController.ec.transform.position - GameManagerController.pc.transform.position;
       // Debug.Log(diff.normalized.x);
       Flashlight();
        EnemyJuice();
        PlayerJuice();
    }

    void Flashlight()
    {
        if (GameManagerController.gm.movestate == GameManagerController.PlayerMovementState.Moving  )
        {
            juice.enabled = true;
            if (!walking && GameManagerController.gm.crouching == GameManagerController.PlayerCrouching.Nothing)
            {
                juice.SetTrigger("Walking");
                walking = true;
                running = false;
            }
            if (!running && GameManagerController.gm.crouching == GameManagerController.PlayerCrouching.Running)
            {
                juice.SetTrigger("Running");
                running = true;
                walking = false;
            }
            
        }
        else
        {

            juice.enabled = false;
            running = false;
            walking = false;
        }
        
        
    }

    void Lighting()
    {
        ObjectController.obj.enemyspot.intensity = 1;
        ObjectController.obj.EnemyPoint.intensity = 1;
    }

    void PlayerJuice()
    {
        if (GameManagerController.gm.crouching == GameManagerController.PlayerCrouching.Crouching) GameManagerController.cam.transform.position = Vector3.MoveTowards(GameManagerController.cam.transform.position, new Vector3( GameManagerController.pc.transform.position.x, 1.3f, GameManagerController.pc.transform.position.z), 2f * Time.deltaTime);
        else GameManagerController.cam.transform.position = Vector3.MoveTowards(GameManagerController.cam.transform.position, new Vector3(GameManagerController.pc.transform.position.x, 1.78f, GameManagerController.pc.transform.position.z), 2f * Time.deltaTime);
          CameraShake();
    }

    void CameraShake()
    {
         distance = Vector3.Distance(GameManagerController.ec.transform.position,
            GameManagerController.pc.transform.position);

         dist = 30;
        
        if (distance < dist)
        {
            float strength = dist - distance;
            
            GameManagerController.cam.transform.position +=
                new Vector3(Random.insideUnitSphere.x * strength/ shake + Time.deltaTime, 0, Random.insideUnitSphere.y * strength/ shake + Time.deltaTime);
        }
        
        else if (GameManagerController.gm.crouching != GameManagerController.PlayerCrouching.Crouching &&
                 distance >= dist)
            GameManagerController.cam.transform.position = Vector3.MoveTowards(
                GameManagerController.cam.transform.position,
                new Vector3(GameManagerController.pc.transform.position.x, 1.78f, GameManagerController.pc.transform.position.z), 2f * Time.deltaTime);
}
    void EnemyJuice()
    {
        EnemyEncounter();
    }

    void EnemyEncounter()
    {
        if (distance < dist - 10 && !done && GameManagerController.gm.state != GameManagerController.EnemyState.Chasing && !GameManagerController.gm.dead)
        {
            StartCoroutine(Glitch());
        }
    }

    

    IEnumerator Glitch()
    {
        done = true;
       juice.SetTrigger("StaticClose");
        static1.SetActive(true);
        SoundManager.sound.glitchclose.Play();
        yield return new WaitForSeconds(.4f);
        static1.SetActive(false);
        yield return new WaitForSeconds(6f);
        done = false;
        
    }
    
}
