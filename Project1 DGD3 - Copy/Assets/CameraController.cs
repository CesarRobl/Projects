using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    void Start()
    {
        GameManagerController.cam = this;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        
        transform.Rotate(GameManagerController.pc.mousey,0,0);
    }
}
