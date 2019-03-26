using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController controller;

    private float distance;
    private int steps;
    private int duration;
    private float speed;
    private DateTime date;
    
    void Awake(){
        if(controller == null){
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if(controller != this){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData(distance,steps,duration,speed,date);
        

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load(){
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            distance = data.distance;
        }
    }
}

[Serializable]
Class PlayerData(){
    private float distance;
    private float speed;
    private int steps;
    private int duration;
    private DateTime date;

    public PlayerData(float distance, int steps, int duration, float speed, DateTime date){
        this.distance = distance;
        this.speed = speed;
        this.steps = steps;
        this.duration = duration;
        this.date = date;
    }

    List<PlayerData> playerdata;

    public void saveRecords(PlayerData p){
        playerdata.Add(p);
    }
}
