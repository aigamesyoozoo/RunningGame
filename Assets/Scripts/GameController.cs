using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Globalization;

public class GameController : MonoBehaviour
{
    public static GameController controller;
    const string RUN_DATA_DESTINATION = "/runsData.dat";
    const string PLAYER_DATA_DESTINATION = "/playerData.dat";

    public double distance;
    public float duration;
    public float speed;
    public DateTime date;
    public int gems;

    public List<RunItem> runList = new List<RunItem>();
    
    void Awake(){
        date = DateTime.Now;
        if(controller == null){
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if(controller != this){
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        Debug.Log("pre-save");
        BinaryFormatter bf = new BinaryFormatter();
        runList = Load();
        runList.Add(new RunItem(distance, duration, speed, DateTime.Now));
        FileStream file = File.Exists(Application.persistentDataPath + RUN_DATA_DESTINATION) ? 
            File.Open(Application.persistentDataPath + RUN_DATA_DESTINATION, FileMode.Open) :
            File.Create(Application.persistentDataPath + RUN_DATA_DESTINATION);
        bf.Serialize(file, runList);
        file.Close();
        Debug.Log("saved run data: ");
        foreach(RunItem r in runList)
        {
            Debug.Log(r);
        }


        // Gems
        BinaryFormatter bf2 = new BinaryFormatter();
        int totalGems = GetTotalGems() + gems;
        FileStream file2 = File.Exists(Application.persistentDataPath + PLAYER_DATA_DESTINATION) ?
            File.Open(Application.persistentDataPath + PLAYER_DATA_DESTINATION, FileMode.Open) :
            File.Create(Application.persistentDataPath + PLAYER_DATA_DESTINATION);
        bf2.Serialize(file2, totalGems);
        file2.Close();
        Debug.Log("saved player data: " + totalGems);
    }

    public List<RunItem> Load(){
        runList.Clear();
        if (File.Exists(Application.persistentDataPath + RUN_DATA_DESTINATION)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + RUN_DATA_DESTINATION, FileMode.Open);
            runList = (List<RunItem>) bf.Deserialize(file);
            file.Close();
        }
        return runList;
    }

    public int GetTotalGems()
    {
        if (File.Exists(Application.persistentDataPath + PLAYER_DATA_DESTINATION))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PLAYER_DATA_DESTINATION, FileMode.Open);
            int g = (int)bf.Deserialize(file);
            file.Close();
            return g;
        }
        return 0;
    }
}

[Serializable]
public class RunItem{
    public float distance;
    public float speed;
    public float duration;
    public DateTime date;

    public RunItem(double distance, float duration, float speed, DateTime date){
        this.distance = (float) distance;
        this.speed = speed;
        this.duration = duration;
        this.date = date;
    }

    public override string ToString()
    {
        return "RI: " + distance + " " + speed + " " + duration + " " + date;
    }
}
