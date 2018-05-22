using System.Collections;
using System.Collections.Generic;
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

    private bool logsTotal = false;
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
        logs.Sort(new LogComp());
        SetLogsTitle();
        foreach (GameObject dailyLogList in dailyLogList) Destroy(dailyLogList);
        dailyLogList.Clear();
        //dailyLogList = new List<GameObject>();
        gameObject.SetActive(true);
        panelCalendar.SetActive(false);
        panelLogTypeChoose.SetActive(false);
        panelLogEditor.SetActive(false);
        for (int i = 0; i < logs.Count; i++) //有log的日期顯示log
        {
            //GameObject gobLogTemp;
            string logDetailTemp;
            if (logs[i].Date.Date == date) 
            {
                if (logs[i].Type == "note") logDetailTemp = logs[i].Detail;
                else if (logs[i].Type == "weight") logDetailTemp = "weight : " + logs[i].Detail + " kg";
                else if (logs[i].Type == "height") logDetailTemp = "height : " + logs[i].Detail + " cm";
                else logDetailTemp = "";
                SetPrefab(i, logDetailTemp);
            }          
        }        
    }

    public void ShowLogsByType(string logType)
    {
        Main_Menu.menu.LogLoad(); //load loglists in mainmenu
        logs = Main_Menu.menu.logList;
        logs.Sort(new LogComp());
        logsTotal = true;        
        foreach (GameObject dailyLogList in dailyLogList) Destroy(dailyLogList);
        dailyLogList.Clear();
        //dailyLogList = new List<GameObject>();
        gameObject.SetActive(true);
        panelCalendar.SetActive(false);
        panelLogTypeChoose.SetActive(false);
        panelLogEditor.SetActive(false);
        for (int i = 0; i < logs.Count; i++) //有log的日期顯示log
        {
            string logDetailTemp;
            if (logType == "all")
            {
                textLogTitle.text = "Logs";
                string logUnitTemp;
                string logsTypeTemp = logs[i].Type + " :  ";
                if (logs[i].Type == "weight") logUnitTemp = " kg";
                else if (logs[i].Type == "height") logUnitTemp = " cm";
                else
                {
                    logsTypeTemp = "";
                    logUnitTemp = "";
                }                
                logDetailTemp = logs[i].Date.ToShortDateString() + "\n" + "    " + logsTypeTemp + logs[i].Detail + logUnitTemp;
                SetPrefab(i, logDetailTemp);
                logsTypeTemp = "";
                logUnitTemp = "";
            }
            else if (logType == "note")
            {
                textLogTitle.text = "Notes";
                if (logs[i].Type == "note")
                {
                    logDetailTemp = logs[i].Date.ToShortDateString() + " :  " + logs[i].Detail;
                    SetPrefab(i, logDetailTemp);
                } 
            } 
            else if (logType == "weight")
            {
                textLogTitle.text = "Weights";
                if (logs[i].Type == "weight")
                {
                    logDetailTemp = logs[i].Date.ToShortDateString() + " :  " + logs[i].Detail + " kg";
                    SetPrefab(i, logDetailTemp);
                }                
            }
            else if (logType == "height")
            {
                textLogTitle.text = "Heights";
                if (logs[i].Type == "height")
                {
                    logDetailTemp = logs[i].Date.ToShortDateString() + " :  " + logs[i].Detail + " cm";
                    SetPrefab(i, logDetailTemp);
                }                
            }            
        }        
    }

    void SetPrefab(int indexTemp, string text)
    {
        GameObject gobLogTemp;
        gobLogTemp = Instantiate(buttonSingleLogPrefab, gameObjectContent.transform);
        gobLogTemp.GetComponent<Button_SingleLog>().GetPanelDeleteCheck(panelLogDeleteCheck, gameObject, indexTemp);
        gobLogTemp.GetComponentInChildren<Text>().text = text;
        dailyLogList.Add(gobLogTemp);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelLogDeleteCheck.activeInHierarchy) panelLogDeleteCheck.SetActive(false);
            else
            {
                if (logsTotal == true)
                {
                    date = DateTime.Today;
                    logsTotal = false;
                }
                BackToCalendar();
            }            
        }            
    }

    public void BackToCalendar()
    {
        gameObject.SetActive(false);
        panelLogTypeChoose.SetActive(false);
        panelLogEditor.SetActive(false);
        if (logsTotal == true)
        {
            date = DateTime.Today;
            logsTotal = false;
        }
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
    public string Type { get; set; }
    public string Detail { get; set; }
}

public class LogComp : IComparer<Log>
{
    public int Compare(Log x, Log y)
    {
        if (x.Date == y.Date)
        {
            return x.Date .CompareTo(y.Date );
        }
        else return x.Date .CompareTo(y.Date);
    }
}
