using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuScript : MonoBehaviour
{
    public Animator panelAchievements;
    public Animator startButton;
    public Animator achievementsButton;
    public Text gemsText;
    public Text runData;
    public Text pbText;
    public Text startText;


    void Start()
    {
        LoadGems();
        if (LoadAchievements() == 0)
            startText.text = "Begin Quest";
    }

    public void StartRun()
    {
        SceneManager.LoadScene("StepCounter");
    }

    private void LoadGems()
    {
        gemsText.text = GameController.controller.GetTotalGems().ToString();
    }

    private int LoadAchievements()
    {
        float pb = 0;
        List<RunItem> runList = GameController.controller.Load();
        if (runList.Count < 1)
        {
            runData.text = "No runs yet...";
            pbText.text = "???";
        }
        else
        {
            string all = "";
            foreach (RunItem r in runList)
            {
                if (r.distance > pb) pb = r.distance;
                all += string.Format("{0,-12}", r.date.ToString("dd/MM/y")) +
                    string.Format("{0, 10:0.00}", r.distance) +
                    string.Format("{0, 11:0.0}", r.speed) +
                    string.Format("{0, 13:0.0}", r.duration) + "\n";
            }
            runData.text = all;
            pbText.text = pb.ToString("0.00") + " KM";
        }
        return runList.Count;
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
