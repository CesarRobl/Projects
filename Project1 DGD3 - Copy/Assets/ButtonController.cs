using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool pressed;
    public MeshRenderer mesh;
    public Light light;
    public AudioSource button;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        PlayerController pp = other.gameObject.GetComponent<PlayerController>();
        if (pp != null && !pressed)
        {
            GameManagerController.gm.buttonspressed++;
            mesh.material = ObjectController.obj.grey;
            light.enabled = false;
           button.Play();
            pressed = true;
        }
    }
}
