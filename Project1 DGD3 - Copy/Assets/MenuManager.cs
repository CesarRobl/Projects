using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image panel;
    public Animation scary;
    public AudioSource button;
    private float timer;
    private bool startgame;
    void Start()
    {
        
    }

   
    void Update()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayAnimation();
        
        if(startgame)  panel.color += new Color(0, 0, 0, 2 * Time.deltaTime);
        if(panel.color.a >= 1) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void PlayAnimation()
    {
        if (timer <= 0)
        {
            scary.Play();
            timer = 7;
        }

        if (timer >= 0) timer -= Time.deltaTime;
    }
    
    public void PlayGame()
    {
        startgame = true;
        button.Play();
       
    }

    public void ExitGame()
    {
        button.Play();
        Application.Quit();
       

    }
}
