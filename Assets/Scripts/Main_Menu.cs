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
    //public List<Timer> bfTimerList = new List<Timer>();
    //public List<Timer> sleepTimerList = new List<Timer>();
    //public List<Timer> playTimerList = new List<Timer>();

    public List<Counter> bottleCounterList = new List<Counter>();
    public List<Counter> pumpCounterList = new List<Counter>();

    public List<Nappy> nappyList = new List<Nappy>();

    public List<Entry> breastfeedList = new List<Entry>();
    public List<Entry> sleepList = new List<Entry>();
    //add more


    //public Dictionary<string, List<Timer>> timerLists = new Dictionary<string, List<Timer>>(); //del
    public Dictionary<string, List<Counter>> counterLists = new Dictionary<string, List<Counter>>(); //del
    public Dictionary<string, List<Entry>> entryLists = new Dictionary<string, List<Entry>>();

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
            breastfeedList = breastfeedList,
            sleepList = sleepList,
            //playTimerList = playTimerList,

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

            breastfeedList = data.breastfeedList;
            sleepList = data.sleepList;
            //playTimerList = data.playTimerList;

            bottleCounterList = data.bottleCounterList;
            pumpCounterList = data.pumpCounterList;

            nappyList = data.nappyList;
        }

        //timerLists.Add("Breastfeed_Button", bfTimerList);
        //timerLists.Add("Sleep_Button", sleepTimerList);
        //timerLists.Add("Play_Button", playTimerList);

        counterLists.Add("Bottle_Button", bottleCounterList);
        counterLists.Add("Pump_Button", pumpCounterList);

        entryLists.Add("Button_Breastfeed", breastfeedList);
        entryLists.Add("Button_Sleep", sleepList);
        //add more
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

    public string FormatTimeSpan(TimeSpan timeSpan)
    {
        string d, h, m, dhm;

        if (timeSpan > new TimeSpan(0, 0, 59))
        {
            if (timeSpan.Days == 0)
            {
                d = "";
                m = timeSpan.Minutes + "m";
            }
            else
            {
                d = timeSpan.Days + "d";
                m = "";
            }


            if (timeSpan.Hours == 0)
            {
                h = "";
                m = timeSpan.Minutes + "m";
            }
            else
                h = timeSpan.Hours + "h";

            if (timeSpan.Minutes == 0)
                m = "";

            dhm = d + h + m;
        }
        else dhm = "< 1min";


        return dhm;
    }

}

[Serializable]
class RecordData
{
    //public List<Timer> bfTimerList;
    //public List<Timer> sleepTimerList;
    //public List<Timer> playTimerList;
    public List<Counter> bottleCounterList;
    public List<Counter> pumpCounterList;
    public List<Nappy> nappyList;

    public List<Entry> breastfeedList;
    public List<Entry> sleepList;
    public List<Entry> playList;
    public List<Entry> bottlefeedList;
    public List<Entry> pumpList;

}



