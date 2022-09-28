using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public static ObjectController obj;
    public GameObject steps;
    public  GameObject Flashlight;
    
    void Awake()
    {
        obj = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
