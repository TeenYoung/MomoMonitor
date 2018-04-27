using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Records_Panel : MonoBehaviour
{
    public List<GameObject> contents; //contains all record prefabs
    public GameObject sourceButton; //由button record傳入
    public int buttonType; //由button record傳入
    public string sourceButtonUnit;  //由button record傳入
    public GameObject scrollPanel, buttonManualInput;
    public GameObject buttonEntry;
    private List<Entry> sourceList;

    //private List<string> records;//every member keep the records in the same day
    //private string tempRecord;   // 暫時存儲每條記錄
    //public Dictionary<string, List<string>> daysOfRecords = new Dictionary<string, List<string>>();

    //variables of records prefab to show records
    public GameObject textSingleRecordPrefab, buttonSingleRecordPrefab, recordsView, buttonLoadMore;

    private int startFromIndex = 0;
    private int initialContentNum = 6; //change num to N to see N+1 days of records when the first time records panel be loaded 
    private int everyClickAddContentNum = 7, contentsNumTotal; //when every click LoadMore Button, change num to N to see N more days of records 
    private GameObject gob;// to hold prefab when they created
    private string record;//用于計算每日記錄，并寫入到prefab里

    //TimeSpan dr = new TimeSpan(); //hold every duration        
    //string tempDailyTS = ""; // hold duration of a whole day

    //variables use for delete record
    public GameObject panelRecordDeleteCheck, buttonRDDelete, buttonRDCancelDelete;
    public Text textChosenRecordToDelete;
    private int deleteIndex;

    // Use this for initialization at every time when this panel be called
    void OnEnable()
    {
        sourceList = Main_Menu.menu.entryLists[name];
        ShowRecords();
        HideButtonLoadMore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseRecordsPanel();
    }

    public void CloseRecordsPanel()
    {
        gameObject.SetActive(false);
        panelRecordDeleteCheck.SetActive(false);
        initialContentNum = 6;
    }

    //public void SetRecordsText(int num)
    //{
    //    foreach (GameObject contents in contents) Destroy(contents);
    //    GameObject gob;
    //    //if (daysOfRecords.Count <= num)
    //    //{
    //    //    for (int i = daysOfRecords.Count-1 ; i >= 0 ; i--)
    //    //    {
    //    //        List<string> tempRecords = new List<string>();
    //    //        string dayRecordTemp;
    //    //        dayRecordTemp = DateTime.Now.AddDays(-i).ToString();
    //    //        daysOfRecords.TryGetValue(dayRecordTemp, out tempRecords);
    //    //        print(tempRecords[0]);
    //    //        for(int j = 0; j < tempRecords.Count; j++)
    //    //        {
    //    //            gob = Instantiate(recordTextPrefab, recordsView.transform);
    //    //            gob.gameObject.GetComponent<Text>().text = tempRecords[j].ToString();
    //    //            contents.Add(gob);
    //    //        }   
    //    //    }
    //    //}
    //    //else if (daysOfRecords.Count > num)
    //    //{
    //    //    for (int i = 0; i < num; i++)
    //    //    {
    //    //        List<string> tempRecords = new List<string>();
    //    //        daysOfRecords.TryGetValue(DateTime.Now.Date.ToString(), out tempRecords);
    //    //        for (int j = 0; j < tempRecords.Count; j++)
    //    //        {
    //    //            gob = Instantiate(recordTextPrefab, recordsView.transform);
    //    //            gob.gameObject.GetComponent<Text>().text = tempRecords[j].ToString();
    //    //            contents.Add(gob);
    //    //        }
    //    //    }
    //    //}

    //    if (records.Count <= num)
    //    {
    //        for (int i = 0; i < records.Count; i++)
    //        {
    //            gob = Instantiate(recordTextPrefab, recordsView.transform);
    //            gob.gameObject.GetComponent<Text>().text = records[i].ToString();
    //            contents.Add(gob);
    //        }
    //    }
    //    else if (records.Count > num)
    //    {
    //        int j = records.Count - num;
    //        for (int i = 0; i < num; i++)
    //        {
    //            gob = Instantiate(recordTextPrefab, recordsView.transform);
    //            gob.gameObject.GetComponent<Text>().text = records[j].ToString();
    //            contents.Add(gob);
    //            j++;
    //        }
    //    }
    //}

    public void AddButtonSingleRecordPrefab(int index)  //add record with timer detail
    {
        gob = Instantiate(buttonSingleRecordPrefab, recordsView.transform);
        gob.gameObject.GetComponentInChildren<Text>().text = record;
        gob.gameObject.GetComponent<Button_SingleRecord_Main>().GetPanelRecordDeleteCheck(
            panelRecordDeleteCheck, textChosenRecordToDelete, record, 
            buttonManualInput.GetComponent<Image>().color, gameObject, index);
        contents.Add(gob);
    }

    public void AddTextSingleRecordPrefab() //add record's date line and total num/duration text
    {
        gob = Instantiate(textSingleRecordPrefab, recordsView.transform);
        gob.gameObject.GetComponent<Text>().text = record;
        contents.Add(gob);
    }

    void ShowTimerRecords()
    {
        TimeSpan dr = new TimeSpan(); //hold every duration        
        string tempDailyTS = ""; // hold duration of a whole day
        foreach (GameObject contents in contents) Destroy(contents);
        bool newdayBegin = false;
        startFromIndex = 0;
        for (int i = 0; i < sourceList.Count; i++) //get the first records index based on initialContentNum
        {
            if (sourceList[i].StartTime.Date == DateTime.Today.AddDays(-initialContentNum).Date)
            {
                startFromIndex = i;
                break;
            }
        }
        for (int i = startFromIndex; i < sourceList.Count; i++)
        {
            //print(sourceList.Count);
            if (i == 0 || sourceList[i - 1].EndTime.Date != sourceList[i].StartTime.Date)
            {
                newdayBegin = true;
                if (tempDailyTS != "" && newdayBegin)
                {
                    record = "\n total duration : " + tempDailyTS; //input duration of last day
                    dr = new TimeSpan();
                    AddTextSingleRecordPrefab();
                }
            }
            if (newdayBegin) //show date line when new days begins
            {
                record = "\n ----------------------" + sourceList[i].StartTime.ToString("d")
                    + "---------------------- \n";
                AddTextSingleRecordPrefab();
                newdayBegin = false;
                record = "";
            }
            if (!newdayBegin) // show every record detail
            {
                record = sourceList[i].StartTime.ToShortTimeString() + " ~ ";
                if (sourceList[i].EndTime != new DateTime())
                {
                    record += sourceList[i].EndTime.ToShortTimeString() + "     Duration:"
                            + Main_Menu.menu.FormatTimeSpan(sourceList[i].CalculateDuration());
                    AddButtonSingleRecordPrefab(i);
                    dr += sourceList[i].CalculateDuration();
                    tempDailyTS = Main_Menu.menu.FormatTimeSpan(dr);//duration of this day 
                }
                else if (sourceList[i].EndTime == new DateTime()) AddButtonSingleRecordPrefab(i);
            }

            //print date and total duration for last record if date changes but no new record add 
            if (i == sourceList.Count - 1 && sourceList[i].EndTime.Date != DateTime.Now.Date && 
                sourceList[i].EndTime.Date != new DateTime())  //when it's last record and not timing
            {
                record = "\n total duration : " + tempDailyTS; //input duration of last day
                dr = new TimeSpan();
                AddTextSingleRecordPrefab();
                record = "\n ----------------------" + DateTime.Now.ToString("d") 
                    + "---------------------- \n";//show time line of today
                AddTextSingleRecordPrefab();
                record = "";
            }
        }
        HideButtonLoadMore();
    }

    //void ShowTimerRecords() //records里的每一元素為每一天的記錄
    //{
    //    tempRecord = "";
    //    record = "";
    //    //record = "\n --------------------  --------------------\n";
    //    records.Clear();
    //    bool newdayBegin = false;
    //    int j = 0;
    //    TimeSpan dr = new TimeSpan(); //傳遞計算縂時長        
    //    for (int i = 0; i < sourceList.Count; i++)
    //    {
    //        //if(newdayBegin)record = " --------------------" + sourceList[i].StartTime.Date.ToShortDateString() + "  --------------------\n";
    //        tempRecord = sourceList[i].StartTime.ToShortTimeString() + " ~ ";
    //        if (sourceList[i].EndTime != new DateTime())
    //        {
    //            tempRecord += sourceList[i].EndTime.ToShortTimeString() + "     Duration:"
    //                + Main_Menu.menu.FormatTimeSpan(sourceList[i].CalculateDuration()) + "\n";
    //            dr += sourceList[i].CalculateDuration();
    //        }
    //        record += tempRecord;
    //        tempDailyTS = Main_Menu.menu.FormatTimeSpan(dr); //記錄當日縂時長

    //        // if date changes 將前一日記錄賦給records，records內的記錄從index=0的元素起，日期依次增長index越大，日期越接近now
    //        if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
    //            && sourceList[i].StartTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date)
    //        {
    //            record = record + "\n total duration : " + tempDailyTS + "\n"; //在輸出次日日期前輸入當日縂時長
    //            record = record + "\n -------------------- " +
    //               sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
    //            records.Insert(j, record);
    //            j++;
    //            tempRecord = "";
    //            record = "";
    //            dr = new TimeSpan();
    //        }
    //        if (i == sourceList.Count - 1 && !newdayBegin)
    //        {
    //            tempRecord = sourceList[i].StartTime.ToShortTimeString() + " ~ ";
    //            records.Insert(j, record); //將未結束的當天記錄計入records
    //        }
    //    }
    //}

    //void ShowCounterRecords() //records里的每一元素為每一天的記錄
    //{
    //    tempRecord = "";
    //    record = "";
    //    records.Clear();

    //    int j = 0;
    //    int tempTotal = 0;
    //    for (int i = 0; i < sourceList.Count; i++)
    //    {
    //        tempRecord = sourceList[i].EndTime.ToShortTimeString() + "  " + sourceList[i].Number + sourceButton.GetComponent<Button_Entry>().unit + "\n";
    //        record += tempRecord;
    //        tempTotal += sourceList[i].Number;

    //        //將前一日記錄賦給records，records內的記錄從index = 0的元素起，日期依次增長index越大，日期越接近now
    //        if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
    //            && sourceList[i].EndTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].EndTime.Date)
    //        {
    //            record = record + "\n total amount : " + tempTotal + sourceButtonUnit + "\n"; //在輸出次日日期前輸入當日縂量
    //            record = record + "-------------------- " +
    //              sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
    //            records.Insert(j, record);
    //            print("records[j]" + j + records[j]);
    //            j++;
    //            tempRecord = "";
    //            record = "";
    //            tempTotal = 0;
    //        }
    //        if (i == sourceList.Count - 1) records.Insert(j, record);
    //    }
    //}

    void ShowCounterRecords() 
    {
        record = "";
        int tempTotal = 0;
        foreach (GameObject contents in contents) Destroy(contents);
        bool newdayBegin = false;
        startFromIndex = 0;
        for (int i = 0; i < sourceList.Count; i++)
        {
            if (sourceList[i].StartTime.Date == DateTime.Today.AddDays(-initialContentNum).Date)
            {
                startFromIndex = i;
                break;
            }
        }
        for (int i = startFromIndex; i < sourceList.Count; i++)
        {
            if (i == 0 || sourceList[i - 1].StartTime.Date != sourceList[i].StartTime.Date)
            {
                newdayBegin = true;
                if (tempTotal != 0 && newdayBegin)
                {
                    record = "\n total amount : " + tempTotal + sourceButtonUnit;
                    tempTotal = 0;
                    AddTextSingleRecordPrefab();
                }
            }
            if (newdayBegin)
            {
                record = "\n ----------------------" + sourceList[i].StartTime.ToString("d")
                    + "---------------------- \n";
                AddTextSingleRecordPrefab();
                newdayBegin = false;
                record = "";
            }
            if (!newdayBegin)
            {
                record = sourceList[i].StartTime.ToShortTimeString() + "  " + sourceList[i].Number + sourceButton.GetComponent<Button_Entry>().unit;
                AddButtonSingleRecordPrefab(i);
                tempTotal += sourceList[i].Number;
            }
            //print date and total num for last record if date changes but no new record add 
            if (i == sourceList.Count - 1 && sourceList[i].EndTime.Date != DateTime.Now.Date)
            {
                record = "\n total amount : " + tempTotal + sourceButtonUnit ;
                tempTotal = 0;
                AddTextSingleRecordPrefab();
                record = "\n ----------------------" + DateTime.Now.ToString("d")
                    + "---------------------- \n";
                AddTextSingleRecordPrefab();
                record = "";
            }
        }
        HideButtonLoadMore();
    }

    void ShowNappyRecords()
    {
        string wee, poo;
        record = "";
        int tempWee = 0, tempPoo = 0;
        foreach (GameObject contents in contents) Destroy(contents);
        bool newdayBegin = false;
        startFromIndex = 0;
        for (int i = 0; i < sourceList.Count; i++)
        {
            if (sourceList[i].StartTime.Date == DateTime.Today.AddDays(-initialContentNum).Date)
            {
                startFromIndex = i;
                break;
            }
        }
        for (int i = startFromIndex; i < sourceList.Count; i++)
        {
            if (sourceList[i].Wee) wee = "Wee  ";
            else wee = "";

            if (sourceList[i].Poo) poo = "Poo";
            else poo = "";                      
            if (sourceList[i].Wee) tempWee++;
            if (sourceList[i].Poo) tempPoo++;

            if (i == 0 || sourceList[i - 1].StartTime.Date != sourceList[i].StartTime.Date)
            {
                newdayBegin = true;
                if (tempWee != 0 && tempPoo != 0 && newdayBegin) //show total amount when a new day begins
                {
                    record = String.Format("total  wee {0} Poo {1}", tempWee, tempPoo);
                    tempWee = 0; tempPoo = 0;
                    AddTextSingleRecordPrefab();
                }
            }
            if (newdayBegin) //show date line when new days begins
            {
                record = "\n ----------------------" + sourceList[i].StartTime.ToString("d") 
                    + "---------------------- \n";
                AddTextSingleRecordPrefab();
                newdayBegin = false;
                record = "";
            }
            if (!newdayBegin)// show every record detail
            {
                record = string.Format("{0}   {1}{2}", sourceList[i].EndTime.ToShortTimeString(), wee, poo);                
                AddButtonSingleRecordPrefab(i);
                if (sourceList[i].Wee) tempWee++;
                if (sourceList[i].Poo) tempPoo++;
            }
            //print date and total num for last record if date changes but no new record add 
            if (i == sourceList.Count - 1 && sourceList[i].EndTime.Date != DateTime.Now.Date)
            {
                record = String.Format("total  wee {0} Poo {1}", tempWee, tempPoo);
                tempWee = 0; tempPoo = 0;
                AddTextSingleRecordPrefab();
                record = "\n ----------------------" + DateTime.Now.ToString("d")
                    + "---------------------- \n";
                AddTextSingleRecordPrefab();
                record = "";
            }
        }
        HideButtonLoadMore();
    }

    //void ShowNappyRecords()
    //{
    //    tempRecord = "";
    //    record = "";
    //    records.Clear();
    //    string wee, poo;

    //    int j = 0;
    //    int tempTotal = 0;
    //    for (int i = 0; i < sourceList.Count; i++)
    //    {
    //        if (sourceList[i].Wee) wee = "Wee  ";
    //        else wee = "";

    //        if (sourceList[i].Poo) poo = "Poo";
    //        else poo = "";

    //        tempRecord = string.Format("{0}   {1}{2}\n", sourceList[i].EndTime.ToShortTimeString(), wee, poo);
    //        record += tempRecord;
    //        tempTotal += sourceList[i].Number;

    //        //將前一日記錄賦給records，records內的記錄從index = 0的元素起，日期依次增長index越大，日期越接近now
    //        if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
    //            && sourceList[i].EndTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].EndTime.Date)
    //        {
    //            record = record + "\n total amount : " + tempTotal + sourceButtonUnit + "\n"; //在輸出次日日期前輸入當日縂量
    //            record = record + "-------------------- " +
    //              sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
    //            records.Insert(j, record);
    //            print("records[j]" + j + records[j]);
    //            j++;
    //            tempRecord = "";
    //            record = "";
    //            tempTotal = 0;
    //        }
    //        if (i == sourceList.Count - 1) records.Insert(j, record);
    //    }
    //}

    //first time show records in 7 days,and every click at load more show records of 7(everyClickAddContentNum) more days 
    public void LoadMoreRecord() 
    {
        contentsNumTotal = initialContentNum + everyClickAddContentNum;
        initialContentNum += everyClickAddContentNum;
        ShowRecords();
    }

    public void HideButtonLoadMore()//when contain record last for 7 days,show Load More button 
    {         
        if (startFromIndex == 0) buttonLoadMore.SetActive(false);
        else buttonLoadMore.SetActive(true);
    }

    public void ShowRecords()
    {
        switch (buttonType)
        {
            case 0:
                {
                    ShowTimerRecords();
                }
                break;
            case 1:
                {
                    ShowCounterRecords();
                }
                break;
            case 2:
                {
                    ShowNappyRecords();
                }
                break;
        }
    }

    public void OpenPanelDeleteRecord()
    {
        panelRecordDeleteCheck.SetActive(true);
    }

    public void ClosePanelDeleteRecord()
    {
        panelRecordDeleteCheck.SetActive(false);
        textChosenRecordToDelete.text = "";
    }

    public void GetDeleteIndex(int index)
    {
        deleteIndex = index;
    }

    public void DeleteRecord()
    {
        switch (buttonType)
        {
            case 0:
                {
                    sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(-sourceList[deleteIndex].CalculateDuration());
                }
                break;
            case 1:
                {
                    sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(-sourceList[deleteIndex].Number);
                }
                break;
            case 2:
                {
                    sourceButton.GetComponent<Button_Entry>().SubtractTodayAmount(sourceList[deleteIndex].Wee, sourceList[deleteIndex].Poo);

                }
                break;
        }        
        sourceList.RemoveAt(deleteIndex);
        Main_Menu.menu.Save();
        ClosePanelDeleteRecord();
        ShowRecords();
    }
}    

