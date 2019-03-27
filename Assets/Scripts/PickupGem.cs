using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupGem : MonoBehaviour
{
    public GameObject Gem;
    public Text GemText;
    GameObject GemObj;
    int collectedGems = 0;
    float Interval = 4.0f;
    double Interval_distance = 20;
    double goal_distance = 20;
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
        GameController.controller.distance = GameController.controller.distance + 1.1f ;
    }

    void DisplayGem(){
        if(GameController.controller.distance * 3.28084 * 0.0003048 * 1000 >= goal_distance){
            
            goal_distance = goal_distance + Interval_distance;
            GemObj.transform.position = transform.position + addtion;
            //Quaternion target = Quaternion.Euler(90,0,0);
            GemObj.transform.rotation = transform.rotation;
            //coinObj.transform.rotation = Quaternion.Slerp(transform.rotation, target, 1);
            GemObj.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other){
        //Debug.Log("hit");
        if(other.tag == "Gem"){
            //Destroy(other.gameObject);
            GemObj.SetActive(false);
            //Debug.Log("hit");
            collectedGems++;
            GemText.text = collectedGems.ToString();
            GameController.controller.gems = collectedGems;
                        
        }
    }        
}
