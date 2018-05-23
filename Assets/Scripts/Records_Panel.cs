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

    //variables of records prefab to show records
    public GameObject textSingleRecordPrefab, buttonSingleRecordPrefab, recordsView, buttonLoadMore;

    private int startFromIndex = 0;
    private int initialContentNum = 6; //change num to N to see N+1 days of records when the first time records panel be loaded 
    private int everyClickAddContentNum = 7, contentsNumTotal; //when every click LoadMore Button, change num to N to see N more days of records 
    private GameObject gob;// to hold prefab when they created
    private string record;//用于計算每日記錄，并寫入到prefab里

    //variables use for delete record
    public GameObject panelRecordDeleteCheck;// buttonRDDelete, buttonRDCancelDelete;
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
        contents = new List<GameObject>();
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
            if (i == 0 || sourceList[i - 1].EndTime.Date != sourceList[i].StartTime.Date)
            {
                newdayBegin = true;
                if (tempDailyTS != "")
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
   
    void ShowCounterRecords() 
    {
        record = "";
        int tempTotal = 0;
        foreach (GameObject contents in contents) Destroy(contents);
        contents = new List<GameObject>();
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
                if (tempTotal != 0)
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

            if (i == 0 || sourceList[i - 1].StartTime.Date != sourceList[i].StartTime.Date)
            {
                newdayBegin = true;
                if (tempWee != 0 || tempPoo != 0) //show total amount when a new day begins
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

    void ShowFoodRecords()
    {
        record = "";
        int tempTotal = 0;
        foreach (GameObject contents in contents) Destroy(contents);
        contents = new List<GameObject>();
        bool newdayBegin = false;
        startFromIndex = 0;
        print(sourceList.Count);
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
                    record = "\n total amount : " + tempTotal;
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
                record = sourceList[i].StartTime.ToShortTimeString() + "  " + sourceList[i].Food;
                AddButtonSingleRecordPrefab(i);
                tempTotal += 1;
            }
            //print date and total num for last record if date changes but no new record add 
            if (i == sourceList.Count - 1 && sourceList[i].EndTime.Date != DateTime.Now.Date)
            {
                record = "\n total amount : " + tempTotal ;
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

    //first time show records in 7 days,and every click at load more show records of 7(everyClickAddContentNum) more days 
    public void LoadMoreRecord() 
    {
        //contentsNumTotal = initialContentNum + everyClickAddContentNum;
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

            case 3:
                {
                    ShowFoodRecords();
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
        //if timing, finish it before delete
        if (buttonType == 0 && sourceList[deleteIndex].EndTime == new DateTime())
            sourceButton.GetComponent<Button_Entry>().TimerOnClick();

        sourceList.RemoveAt(deleteIndex);
        Main_Menu.menu.Save();
        ClosePanelDeleteRecord();
        ShowRecords();

        //if not timing, refresh title
        if (!(buttonType == 0 && sourceList[deleteIndex].EndTime == new DateTime()))
            sourceButton.GetComponent<Button_Entry>().RefreshTexts();

    }
}    

