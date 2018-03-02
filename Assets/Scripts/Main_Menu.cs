using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO; 

public class Main_Menu : MonoBehaviour {

    public static Main_Menu menu;
    //public GameObject breastfeedButton;
    public Text timeLabel;
    public List<Timer> bfTimerList = new List<Timer>();
    public List<Timer> sleepTimerList = new List<Timer>();
    public List<Timer> playTimerList = new List<Timer>();

    // Use this for initialization
    void Awake () {

        //make sure only one Main Menu exists
        if (menu == null) {
            DontDestroyOnLoad(gameObject);
            menu = this;
        }
        else if (menu != this) Destroy(gameObject);

	}

    //auto load
    private void OnEnable()
    {
        Load();
    }

    // Update is called once per frame
    void Update () {
        timeLabel.text = DateTime.Now.ToShortTimeString();
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/record.dat");

        RecordData data = new RecordData();
        data.bfTimerList = bfTimerList;
        data.sleepTimerList = sleepTimerList;
        data.playTimerList = playTimerList;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/record.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/record.dat", FileMode.Open);
            RecordData data = (RecordData)bf.Deserialize(file);
            file.Close();

            bfTimerList = data.bfTimerList;
            sleepTimerList = data.sleepTimerList;
            playTimerList = data.playTimerList;
        }
    }

    [Serializable]
    class RecordData
    {
        public List<Timer> bfTimerList;
        public List<Timer> sleepTimerList;
        public List<Timer> playTimerList;

    }

    public void MainButtonOnClick()
    {
        if(SceneManager.GetActiveScene().name != "Main")
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void InfoButtonOnClick()
    {
        if (SceneManager.GetActiveScene().name != "BabyInfo")
        SceneManager.LoadScene("BabyInfo", LoadSceneMode.Single);
    }

}
