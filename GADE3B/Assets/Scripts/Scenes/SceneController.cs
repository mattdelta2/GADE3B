using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Quit()
    {
        Application.Quit();
    }


    void Play()
    {
        SceneManager.LoadScene("GameScene");
    }


    void EndScreen()
    {
        SceneManager.LoadScene("EndScene");
    }


}
