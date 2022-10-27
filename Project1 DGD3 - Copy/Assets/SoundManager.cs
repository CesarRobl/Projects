using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sound;
    public AudioSource burnout;
    public AudioSource exhaust;
    public AudioSource glitchclose;
    private bool stop;

    private void Start()
    {
        sound = this;
    }


    public void LightBurn()
    {
        if (  GameManagerController.gm.nobattery && !stop  )
        {
            burnout.Play();
            stop = true;
        }
         if (!GameManagerController.gm.nobattery && stop)
        {
            burnout.Stop();
            stop = false;
        }
    }
}
