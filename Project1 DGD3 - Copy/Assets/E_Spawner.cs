using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class E_Spawner : MonoBehaviour
{
    private Vector3 diff;
    public bool playerseen, inrange;
    private bool stop;
    
    void Start()
    {
        
    }

     void LateUpdate()
    {
        diff = GameManagerController.pc.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        CheckForPlayer();
    }

    private void OnDrawGizmos()
    {
       
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position,diff);
    }

    void CheckForPlayer()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y +.1f, transform.position.z), diff, out RaycastHit hit))
        {
            PlayerController pc = hit.collider.GetComponent<PlayerController>();
            if (pc != null)
            {
                playerseen = true;
            }
            else playerseen = false;
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        boxcast box = other.gameObject.GetComponent<boxcast>();
        if (box != null && !stop && box.player)
        {
            
            GameManagerController.gm.e_spawn.Add(this);
            stop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        boxcast box = other.gameObject.GetComponent<boxcast>();
        if (box != null && box.player)
        {
            GameManagerController.gm.e_spawn.Remove(this);
            stop = false;
        }
    }
}

