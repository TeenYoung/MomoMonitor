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

    //public Color colorSleep;
    public List<Color> colors;

    public List<Entry> breastfeedList = new List<Entry>();
    public List<Entry> sleepList = new List<Entry>();
    public List<Entry> bottlefeedList = new List<Entry>();
    public List<Entry> pumpList = new List<Entry>();
    public List<Entry> playList = new List<Entry>();
    public List<Entry> nappyList = new List<Entry>();
    public List<Entry> foodList = new List<Entry>();
    //add more

    public List<Log> logList = new List<Log>();
    public List<Log> noteList = new List<Log>();
    public List<Log> weightList = new List<Log>();
    public List<Log> heightList = new List<Log>();

    //public Dictionary<string, List<Timer>> timerLists = new Dictionary<string, List<Timer>>(); //del
    //public Dictionary<string, List<Counter>> counterLists = new Dictionary<string, List<Counter>>(); //del
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
        LogLoad();
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
            foodList = foodList,
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
            nappyList = data.nappyList;
            foodList = data.foodList;
            //add more
        }

        entryLists.Add("Button_Breastfeed", breastfeedList);
        entryLists.Add("Button_Sleep", sleepList);
        entryLists.Add("Button_Bottle", bottlefeedList);
        entryLists.Add("Button_Pump", pumpList);
        entryLists.Add("Button_Play", playList);
        entryLists.Add("Button_Nappy", nappyList);
        entryLists.Add("Button_Food", foodList);
        //add more
    }

    public void LogSave()
    {
        BinaryFormatter bfLog = new BinaryFormatter();
        FileStream fileLog = File.Create(Application.persistentDataPath + "/calendarLogs.dat");

        LogData logData = new LogData
        {
            logList = logList,
            noteList = noteList,
            weightList = weightList,
            heightList = heightList
        };

        bfLog.Serialize(fileLog, logData);
        fileLog.Close();
    }

    public void LogLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/calendarLogs.dat"))
        {
            BinaryFormatter bfLog = new BinaryFormatter();
            FileStream fileLog = File.Open(Application.persistentDataPath + "/calendarLogs.dat", FileMode.Open);
            LogData logData = (LogData)bfLog.Deserialize(fileLog);
            fileLog.Close();

            logList = logData.logList;
            noteList = logData.noteList;
            weightList = logData.weightList;
            heightList = logData.heightList;
            //add more
        }
    }

    public void LogsAdd(Log log)
    {
        logList.Add(log);
        if (log.Type == "note") noteList.Add(log);
        if (log.Type == "weight") weightList.Add(log);
        if (log.Type == "height") heightList.Add(log);
    }   

    public void LogsRemove(int index)
    {        
        if (logList[index].Type == "note")
        {
            for(int i= noteList.Count-1; i>=0;i--)
                if (logList[index].Date.Date == noteList[i].Date.Date && logList[index].Detail == noteList[i].Detail)
                    noteList.RemoveAt(i);
        }
            
        if (logList[index].Type == "weight")
        {
            for (int i = weightList.Count - 1; i >= 0; i--)
                if (logList[index].Date.Date == weightList[i].Date.Date && logList[index].Detail == weightList[i].Detail)
                    weightList.RemoveAt(i);
        }
        
        if (logList[index].Type == "height")
        {
            for (int i = heightList.Count - 1; i >= 0; i--)
                if (logList[index].Date.Date == heightList[i].Date.Date && logList[index].Detail == heightList[i].Detail)
                    heightList.RemoveAt(i);
        }
        logList.RemoveAt(index);
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

    public void PatternButtonOnClick()
    {
        if (SceneManager.GetActiveScene().name != "Pattern")
            SceneManager.LoadScene("Pattern", LoadSceneMode.Single);
    }

    public void CalendarButtonOnClick()
    {
        if (SceneManager.GetActiveScene().name != "Calendar")
            SceneManager.LoadScene("Calendar", LoadSceneMode.Single);
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
    public List<Entry> nappyList;
    public List<Entry> foodList;
    //add more

}

[Serializable]
class LogData
{
    public List<Log> logList;
    public List<Log> noteList;
    public List<Log> weightList;
    public List<Log> heightList;
    //add more
}



