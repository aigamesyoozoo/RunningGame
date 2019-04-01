using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupGem : MonoBehaviour
{
    public GameObject Gem;
    public Text GemText;
    public AudioSource gemSound;
    GameObject GemObj;
    float Interval = 4.0f;
    double Interval_distance = 0.02;
    double goal_distance = 0.02;
    Vector3 addtion = new Vector3(0,0.84f,2.6f);

    // Start is called before the first frame update
    void Start()
    {
        GemObj = (GameObject)Instantiate(Gem);
        GemObj.SetActive(false);
        InvokeRepeating("DisplayGem",Interval,Interval);
        //InvokeRepeating("IncreaseDistance",0.1f,0.1f);
    }

    void IncreaseDistance(){
        GameController.controller.distance = GameController.controller.distance + 0.01 ;
    }

    void DisplayGem(){
        if(GameController.controller.distance >= goal_distance){
            if (!GemObj.activeSelf)
            {
                goal_distance = goal_distance + Interval_distance;
                GemObj.transform.position = transform.position + addtion;
                GemObj.transform.rotation = transform.rotation;
                GemObj.SetActive(true);
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Gem"){
            gemSound.Play();
            GemObj.SetActive(false);
            GameController.controller.gems++;
            GemText.text = GameController.controller.gems.ToString();
        }
    }        
}
