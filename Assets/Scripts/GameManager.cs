using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public Text runData;
    public Animator panelAchievements;
    public Animator startButton;
    public Animator achievementsButton;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        AddScore(5.55f, 30.333f);
        AddScore(4f, 28f);
        AddScore(6.77f, 90f);
        LoadAchievements();
    }

    public void StartRun()
    {
        //SceneManager.LoadScene("StepCounter");
    }

    private void LoadAchievements()
    {
        runData.text = PlayerPrefs.HasKey("runs") ? PlayerPrefs.GetString("runs") : "No runs yet...";
    }

    public void OpenAchievements()
    {
        panelAchievements.SetBool("isHidden", false); 
        startButton.SetBool("isHidden", true);
        achievementsButton.SetBool("isHidden", true);
    }

    public void CloseAchievements()
    {
        panelAchievements.SetBool("isHidden", true);
        startButton.SetBool("isHidden", false);
        achievementsButton.SetBool("isHidden", false);
    }

    public void AddScore(float distance, float duration)
    {
        string currentRun = string.Format("{0,-14}", DateTime.Now.ToString("dd/MM/y")) +
            string.Format("{0, 9:0.00}", distance) +
            string.Format("{0, 12:0.00}", duration) + "\n";
        PlayerPrefs.SetString("runs", (PlayerPrefs.HasKey("runs") ? PlayerPrefs.GetString("runs") : "") + currentRun);
    }
}
