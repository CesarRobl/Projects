using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class ButtonSpawn : MonoBehaviour
{
    public bool button1, button2, button3;
    void Start()
    {
        
        if(button1) GameManagerController.gm.buttonLoc.Add(gameObject.transform);
        else if (button2) GameManagerController.gm.buttonLoc2.Add(gameObject.transform);
        else if (button3) GameManagerController.gm.buttonLoc3.Add(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
