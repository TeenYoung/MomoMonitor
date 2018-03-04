using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Main_Menu : MonoBehaviour
{

    public static Main_Menu menu;

    // if add new timer/counter button have to manuly add a list here
    public Text timeLabel;
    public List<Timer> bfTimerList = new List<Timer>();
    public List<Timer> sleepTimerList = new List<Timer>();
    public List<Timer> playTimerList = new List<Timer>();

    public List<Counter> bottleCounterList = new List<Counter>();
    public List<Counter> pumpCounterList = new List<Counter>();

    public List<Nappy> nappyList = new List<Nappy>();

    public Dictionary<string, List<Timer>> timerLists = new Dictionary<string, List<Timer>>();
    

    // Use this for initialization
    void Awake()
    {

        //make sure only one Main Menu exists
        if (menu == null)
        {
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
    void Update()
    {
        timeLabel.text = DateTime.Now.ToShortTimeString();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/record.dat");


        RecordData data = new RecordData
        {
            bfTimerList = bfTimerList,
            sleepTimerList = sleepTimerList,
            playTimerList = playTimerList,

            bottleCounterList = bottleCounterList,
            pumpCounterList = pumpCounterList,

            nappyList = nappyList,
        };

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

            bottleCounterList = data.bottleCounterList;
            pumpCounterList = data.pumpCounterList;

            nappyList = data.nappyList;
        }

        timerLists.Add("Breastfeed_Button", bfTimerList);
        timerLists.Add("Sleep_Button", sleepTimerList);
        timerLists.Add("Play_Button", playTimerList);
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

[Serializable]
class RecordData
{
    public List<Timer> bfTimerList;
    public List<Timer> sleepTimerList;
    public List<Timer> playTimerList;
    public List<Counter> bottleCounterList;
    public List<Counter> pumpCounterList;
    public List<Nappy> nappyList;
}



