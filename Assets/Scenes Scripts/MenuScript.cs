using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuScript : MonoBehaviour
{
    public Text runData;
    public Animator panelAchievements;
    public Animator startButton;
    public Animator achievementsButton;
    public Text gemsText;

    void Start()
    {
        LoadAchievements();
        LoadGems();
    }

    public void StartRun()
    {
        SceneManager.LoadScene("StepCounter");
    }

    private void LoadGems()
    {
        gemsText.text = GameController.controller.GetTotalGems().ToString();
    }

    private void LoadAchievements()
    {
        List<RunItem> runList = GameController.controller.Load();
        if (runList.Count < 1) runData.text = "No runs yet...";
        else
        {
            string all = "";
            foreach (RunItem r in runList)
            {
                all += string.Format("{0,-14}", r.date.ToString("dd/MM/y")) +
                    string.Format("{0, 9:0.00}", r.distance) +
                    string.Format("{0, 15:0.0}", r.duration) +
                    string.Format("{0, 14:0.0}", r.speed) + "\n";
            }
            runData.text = all;
        }
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
}
