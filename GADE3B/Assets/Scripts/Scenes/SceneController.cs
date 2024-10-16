using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("DifficultySelection");
    }

    public void StartNormalMode()
    {
        // Set the difficulty to Normal
        PlayerPrefs.SetString("SelectedDifficulty", "Normal");
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }

    public void StartHardMode()
    {
        // Set the difficulty to Hard
        PlayerPrefs.SetString("SelectedDifficulty", "Hard");
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }





}
