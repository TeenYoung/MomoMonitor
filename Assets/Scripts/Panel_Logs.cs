using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Logs : MonoBehaviour {
    public GameObject panelCalendar, panelLogTypeChoose, panelLogEditor;
    public GameObject buttonBackToCalendar, buttonAddLogs, buttonShowAll,sourceDayButton;
    public GameObject buttonAddNote, buttonAddReminder, buttonAddGrowth;

    public GameObject panelLogDeleteCheck;

    public GameObject buttonSingleLogPrefab, gameObjectContent;// textSingleLogPrefab, 
    public List<GameObject> dailyLogList;

    public InputField inputFieldLog, inputFieldWeight, inputFieldHeight;
    public Text textLogDetail, textLogTitle, textLogDate;
    private string logDetailDelete;
    private int logDeleteIndex;

    private DateTime date;
    private string logType;
    private List<Log> logsList;
    private GameObject logButtonTemp;

    public List<Log> logs = new List<Log>();

    // Use this for initialization
    void Start() {
    }

    public void GetButtonDate(DateTime dateOfButton)
    {
        date = dateOfButton;        
    }

    public void SetLogsTitle() //set date of three kinds of logs here
    {
        textLogTitle.text = date.Day + " " + date.ToString("MMM", new CultureInfo("en-us")) + " " + date.Year;
        textLogDate.text = textLogTitle.text;
    }

    private void OnEnable()
    {
        Load();
        foreach (GameObject dailyLogList in dailyLogList) Destroy(dailyLogList);
        dailyLogList.Clear();
        gameObject.SetActive(true);
        panelCalendar.SetActive(false);
        panelLogTypeChoose.SetActive(false);
        panelLogEditor.SetActive(false);
        for (int i = 0; i < logs.Count; i++) //有log的日期顯示log
        {
            GameObject gobLogTemp; 
            if (logs[i].Date.Date == date) 
            {
                gobLogTemp = Instantiate(buttonSingleLogPrefab, gameObjectContent.transform);
                if(logs[i].Type == "note")gobLogTemp.GetComponentInChildren<Text>().text = logs[i].Detail;
                else if (logs[i].Type == "weight") gobLogTemp.GetComponentInChildren<Text>().text = "weight : " + logs[i].Detail + " kg";
                else if (logs[i].Type == "height") gobLogTemp.GetComponentInChildren<Text>().text = "height : " + logs[i].Detail + " cm";
                gobLogTemp.GetComponent<Button_SingleLog>().GetPanelDeleteCheck(panelLogDeleteCheck, gameObject, i);
                dailyLogList.Add(gobLogTemp);
            }          
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelLogDeleteCheck.activeInHierarchy) panelLogDeleteCheck.SetActive(false);
            else BackToCalendar();
        }            
    }

    public void BackToCalendar()
    {
        gameObject.SetActive(false);
        panelLogTypeChoose.SetActive(false);
        panelLogEditor.SetActive(false);
        panelCalendar.GetComponent<Panel_Calendar>().BackToCertainDay(date);
        panelCalendar.SetActive(true);        
    }

    public void OpenPanelLogChoose()
    {
        panelLogTypeChoose.SetActive(true);       
    }

    public void ClosePanelLogDeleteCheck()
    {
        panelLogDeleteCheck.SetActive(false);
    }

    public void ClickAddLogs(GameObject logButton)  //add log button點擊通用,設if 判斷切換打開的editor 樣式,note，growth,reminder等；
    {
        gameObject.SetActive(true);
        if (logButton == buttonAddNote)    //note
        {
            panelLogEditor.SetActive(true);
            inputFieldLog.gameObject.SetActive(true);
            inputFieldWeight.gameObject.SetActive(false);
            inputFieldHeight.gameObject.SetActive(false);
            panelCalendar.SetActive(false);
            logType = "note";
            inputFieldLog.Select();
        }       

        if (logButton == buttonAddGrowth)  //growth
        {
            panelLogEditor.SetActive(true);
            inputFieldLog.gameObject.SetActive(false);
            inputFieldWeight.gameObject.SetActive(true);
            inputFieldHeight.gameObject.SetActive(true);
            panelCalendar.SetActive(false);
            logType = "growth";
            inputFieldWeight.Select();
        }

        if (logButton == buttonAddReminder)  //reminder
        {
            logType = "reminder";
        }
        logButtonTemp = logButton;
    }   

    public void SaveLog()
    {
        Log logTemp = new Log();
        if (logType == "note")    //note
        {
            logTemp.Date = date;
            logTemp.Detail = inputFieldLog.text.ToString();
            logTemp.Type = logType;
            panelLogEditor.GetComponent<Panel_LogEditor>().GetLog(logTemp);
            inputFieldLog.text = "";
            if (logTemp.Detail != "") logs.Add(logTemp);//設一個log class 内含date，type，detail等properties，儲存時按照type分別存爲不同list
        }

        //if (logType== "growth")  
        if (logButtonTemp == buttonAddGrowth)  //growth
        {
            logTemp.Date = date;
            logTemp.Detail = inputFieldWeight.text.ToString();
            logTemp.Type = "weight";
            if (logTemp.Detail != "") logs.Add(logTemp);
            inputFieldWeight.text = "";
            
            logTemp = new Log();
            logTemp.Date = date;
            logTemp.Detail = inputFieldHeight.text.ToString();
            logTemp.Type = "height";            
            if (logTemp.Detail != "") logs.Add(logTemp);
            inputFieldHeight.text = "";
        }

        if (logType == "reminder")  //reminder
        {
           
        }
        Save();
        panelCalendar.GetComponent<Panel_Calendar>().GetLogs(logs);
        BackToCalendar();
    }

    public void DiscardLog()
    {
        BackToCalendar();
        inputFieldLog.text = "";
    }

    public void GetDeleteLogIndex(int num)
    {
        logDeleteIndex = num;
    }

    public void LoadLogsToCalendar()
    {
        Load();
        panelCalendar.GetComponent<Panel_Calendar>().GetLogs(logs);
    }

    public void DeleteSingleLog()
    {
        logs.RemoveAt(logDeleteIndex);
        ClosePanelLogDeleteCheck();
        Save();
        Load();
        panelCalendar.GetComponent<Panel_Calendar>().GetLogs(logs);
        BackToCalendar();
    }

    void Save()
    {
        BinaryFormatter bfLog = new BinaryFormatter();
        FileStream fileLog = File.Create(Application.persistentDataPath + "/calendarLogs.dat");

        LogData data = new LogData
        {
            logList = logs
        };

        bfLog.Serialize(fileLog, data);
        fileLog.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/calendarLogs.dat"))
        {
            BinaryFormatter bfLog = new BinaryFormatter();
            FileStream fileLog = File.Open(Application.persistentDataPath + "/calendarLogs.dat", FileMode.Open);
            LogData data = (LogData)bfLog.Deserialize(fileLog);
            fileLog.Close();

            logs = data.logList;
            //add more
        }
    }    
}

[Serializable]
public class Log
{
    public DateTime Date { get; set; }
    public String Type { get; set; }
    public string Detail { get; set; }
}

[Serializable]
class LogData
{
    public List<Log> logList;
    //add more
}
