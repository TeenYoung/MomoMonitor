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

    public Text timeLabel;

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
    //private void OnEnable()
    //{
    //    Load();
    //}

    // Update is called once per frame
    void Update () {
        timeLabel.text = DateTime.Now.ToShortTimeString();
	}

    //public void Save()
    //{
    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Create(Application.persistentDataPath + "history.dat");

    //    HistoryData data = new HistoryData();
    //    //add data later

    //    bf.Serialize(file, data);
    //    file.Close();
    //}

    //public void Load()
    //{
    //    if (File.Exists(Application.persistentDataPath + "history.dat"))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(Application.persistentDataPath + "history.dat", FileMode.Open);
    //        HistoryData data = (HistoryData)bf.Deserialize(file);
    //        file.Close();

    //        //add some data later
    //    }
    //}

    //[Serializable]
    //class HistoryData
    //{

    //}

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
