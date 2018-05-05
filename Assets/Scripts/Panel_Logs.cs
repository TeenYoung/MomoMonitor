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
        logs = Main_Menu.menu.logList;
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
        Main_Menu.menu.LogLoad(); //load loglists in mainmenu
        logs = Main_Menu.menu.logList;
        SetLogsTitle();
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
            //logType = "growth";
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
            if (logTemp.Detail != "") Main_Menu.menu.LogsAdd(logTemp);
        }
                
        if (logButtonTemp == buttonAddGrowth)  //growth
        {
            logTemp.Date = date;
            logTemp.Detail = inputFieldWeight.text.ToString();
            logTemp.Type = "weight";
            if (logTemp.Detail != "") Main_Menu.menu.LogsAdd(logTemp);
            inputFieldWeight.text = "";
            
            logTemp = new Log();
            logTemp.Date = date;
            logTemp.Detail = inputFieldHeight.text.ToString();
            logTemp.Type = "height";            
            if (logTemp.Detail != "") Main_Menu.menu.LogsAdd(logTemp);
            inputFieldHeight.text = "";
        }

        if (logType == "reminder")  //reminder
        {
           
        }
        Main_Menu.menu.LogSave();
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

    public void DeleteSingleLog() // call remove function in mainmenu
    {
        Main_Menu.menu.LogsRemove(logDeleteIndex);
        ClosePanelLogDeleteCheck();
        Main_Menu.menu.LogSave();
        Main_Menu.menu.LogLoad();
        BackToCalendar();
    }   
}

[Serializable]
public class Log
{
    public DateTime Date { get; set; }
    public String Type { get; set; }
    public string Detail { get; set; }
}
