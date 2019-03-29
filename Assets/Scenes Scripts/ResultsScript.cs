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
    public Image goodrunImage;
    public Image youloseImage;
    public CanvasGroup starsGroup;
    public ParticleSystem starsTwinkle;
    public Animator m_animator;
    public Animator monster_animator;
    public GameObject player;
    public GameObject monster;

    void Start()
    {

        GameController.controller.Save();
        distanceText.text = GameController.controller.distance.ToString("0.0");
        speedText.text = GameController.controller.speed.ToString("0.0") + " km/h";
        durationText.text = GameController.controller.duration.ToString("0.0") + " min";
        gemsText.text = GameController.controller.gems.ToString("0");

        if (GameController.controller.lose)
        {
            starsGroup.alpha = 0;
            player.SetActive(false);
            monster_animator.SetTrigger("walk");
            monster.SetActive(true);
            goodrunImage.enabled = false;
            youloseImage.enabled = true;
            //starsTwinkle.emission.enabled = false;
        }

        else
        {
            // Ensure player is in running animation
            m_animator.SetFloat("MoveSpeed", 1f);
            m_animator.SetBool("Grounded", true);
        }
        
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
