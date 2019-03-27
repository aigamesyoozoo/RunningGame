using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsScript : MonoBehaviour
{
    public Text distanceText;
    public Text speedText;
    public Text durationText;
    public Text gemsText;

    void Start()
    {
        double d = 6.000;
        float f = 3f;
        Debug.Log("a: " + d/f);
        GameController.controller.Save();
        distanceText.text = GameController.controller.distance.ToString("0.00");
        speedText.text = GameController.controller.speed.ToString("0.0") + "km/h";
        durationText.text = GameController.controller.duration.ToString("0.0") + "min";
        gemsText.text = GameController.controller.gems.ToString("0");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
