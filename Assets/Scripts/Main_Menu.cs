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

    public Text timeLabel;


    public List<Entry> breastfeedList = new List<Entry>();
    public List<Entry> sleepList = new List<Entry>();
    public List<Entry> bottlefeedList = new List<Entry>();
    public List<Entry> pumpList = new List<Entry>();
    public List<Entry> playList = new List<Entry>();
    //add more

    public List<Nappy> nappyList = new List<Nappy>();


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
            bottlefeedList = bottlefeedList,
            pumpList = pumpList,
            playList = playList,
            //add more

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
            bottlefeedList = data.bottlefeedList;
            pumpList = data.pumpList;
            playList = data.playList;
            //add more

            nappyList = data.nappyList;
        }

        entryLists.Add("Button_Breastfeed", breastfeedList);
        entryLists.Add("Button_Sleep", sleepList);
        entryLists.Add("Button_Bottle", bottlefeedList);
        entryLists.Add("Button_Pump", pumpList);
        entryLists.Add("Button_Play", playList);
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
    public List<Entry> breastfeedList;
    public List<Entry> sleepList;
    public List<Entry> bottlefeedList;
    public List<Entry> pumpList;
    public List<Entry> playList;
    //add more

    public List<Nappy> nappyList;
}



