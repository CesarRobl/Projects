using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawn : MonoBehaviour
{
    public bool button1, button2, button3;
    void Start()
    {
        if(button1) GameManagerController.gm.ButtonLoc.Add(transform);
        else if (button2) GameManagerController.gm.ButtonLoc2.Add(transform);
        else if (button3) GameManagerController.gm.ButtonLoc3.Add(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
