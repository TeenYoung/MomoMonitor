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

    public InputField inputFieldLog;
    public Text textLogDetail, textLogTitle, textLogDate;
    private string logDetailDelete;

    private DateTime date;
    private string logType;
    private List<Log> logsList;
    
    public List<Log> logs = new List<Log>();

    // Use this for initialization
    void Start() {

    }

    public void SetLogsTitle() //set date of three kinds of logs here
    {
        textLogTitle.text = date.Day + " " + date.ToString("MMM", new CultureInfo("en-us")) + " " + date.Year;
        textLogDate.text = textLogTitle.text;
    }

    public void GetButtonDate(DateTime dateOfButton)
    {
        date = dateOfButton;
        SetLogsTitle();
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        panelCalendar.SetActive(false);
        panelLogTypeChoose.SetActive(false);
        panelLogEditor.SetActive(false);
        foreach (GameObject dailyLogList in dailyLogList) Destroy(dailyLogList);
        dailyLogList.Clear();
        for (int i = 0; i < logs.Count; i++) //有log的日期顯示log tag
        {
            GameObject gobLogTemp; ;
            if (logs[i].Date.Date == date) //textLogDetail.text += logs[i].Detail;
            {
                gobLogTemp = Instantiate(buttonSingleLogPrefab, gameObjectContent.transform);
                gobLogTemp.GetComponentInChildren<Text>().text = logs[i].Detail;
                gobLogTemp.GetComponent<Button_SingleLog>().GetPanelDeleteCheck(panelLogDeleteCheck,gameObject);
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
        //textLogDetail.text = "";
    }

    public void OpenPanelLogChoose()
    {
        panelLogTypeChoose.SetActive(true);       
    }   
    
    public void ClosePanelLogDeleteCheck()
    {
        panelLogDeleteCheck.SetActive(false);
    }

    public void GetDeleteLogDetail(string logDetail)
    {
        logDetailDelete = logDetail;
    }
    
    public void DeleteSingleLog()
    {
        for(int i = logs.Count-1; i >= 0; i--)
        {
            if (logDetailDelete == logs[i].Detail)
            {
                //DateTime dateOfDeleteTemp = logs[i].Date;
                logs.RemoveAt(i);
            }            
        }
        ClosePanelLogDeleteCheck();
        BackToCalendar();
    }
    //public void GetSourceButton(GameObject sourceButton)
    //{
    //    sourceDayButton = sourceButton;
    //}

    public void ClickAddLogs(GameObject logButton)  //add log button點擊通用,設if 判斷切換打開的editor 樣式,note，growth,reminder等；
    {
        gameObject.SetActive(true);
        SaveLog(logButton);
        if (logButton == buttonAddNote)    //增加note記錄
        {
            panelLogEditor.gameObject.SetActive(true);
            panelCalendar.SetActive(false);
            logType = "note";
            inputFieldLog.Select();
        }

        if (logButton == buttonAddReminder)
        {
            logType = "reminder";
        }

        if (logButton == buttonAddGrowth)
        {
            logType = "growth";
        }
    }

    public void SaveLog(GameObject logButton)
    {        
        Log logTemp = new Log();
        logTemp.Date = date;
        logTemp.Detail = inputFieldLog.text.ToString();
        logTemp.Type = logType;
        if(logTemp.Detail!="")
        logs.Add(logTemp);//設一個log class 内含date，type，detail等properties，儲存時按照type分別存爲不同list
        panelCalendar.GetComponent<Panel_Calendar>().GetLogs(logs);
        //sourceDayButton.GetComponent<Button_CalendarDay>().GetLogs(logs); //把記錄傳入button calendardays
        //panelCalendar.GetComponent<Panel_Calendar>().SetDays(date.Year,date.Month);
        BackToCalendar();
        panelLogEditor.GetComponent<Panel_LogEditor>().GetLog(logTemp);
        //sourceDayButton.GetComponent<Button_CalendarDay>().SaveLogs(logType, inputFieldNote.text.ToString());
        inputFieldLog.text = "";
    }

    public void DiscardLog()
    {
        BackToCalendar();
        inputFieldLog.text = "";
    }

    public void Save()
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
            FileStream fileLog = File.Open(Application.persistentDataPath + "/record.dat", FileMode.Open);
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
