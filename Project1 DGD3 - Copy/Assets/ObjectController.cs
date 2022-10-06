using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    public static ObjectController obj;
    public GameObject steps;
    public  GameObject Flashlight;
    public GameObject bulb;
    public GameObject FlashView;
    public List<MeshRenderer> guide;
    public List<Light> lightguide;
    public List<GameObject> bars;
    public Material green;
    public Material grey;

    public Image win;
    public TextMeshProUGUI text;

    public List<AudioClip> stepsound;
    public List<AudioClip> flashsound;
    public List<AudioClip> EnemyIdle;
    public List<AudioClip> EnemyAlert;
    public AudioClip enemychase;
    void Start()
    {
      
        obj = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

