using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Records_Panel : MonoBehaviour
{

    public List<GameObject> contents;
    public GameObject sourceButton; //由button record傳入
    public int buttonType; //由button record傳入
    //public Text text;
    public string sourceButtonUnit;  //由button record傳入
    public GameObject scrollPanel;

    private List<Entry> sourceList;
    private List<string> records ;//every member keep the records in the same day
    private string record;//用于計算每日記錄，并寫入到records list里
    private string tempRecord;   // 暫時存儲每條記錄
    private string tempDailyTS; // 暫時存儲每日縂時長

    
    public GameObject recordTextPrefab, recordsView;
    private int initialContentNum = 7, everyClickAddContentNum = 7, contentsNumTotal;

    // Use this for initialization at every time when this panel be called
    void OnEnable ()
    {
        records = new List<string>();
        sourceList = Main_Menu.menu.entryLists[name];        
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

        //判斷records的天數和contents的大小，并顯示將同一日的records顯示在一個content內

        //若record天數小于contents，將無records的contents清空
        //if (records.Count < contents.Count)
        //{
        //    for (int j = 0; j < contents.Count; j++)
        //    {
        //        for (int i = 0; i < records.Count; i++)
        //        {
        //            contents[i].gameObject.GetComponent<Text>().text = records[i].ToString();
        //        }
        //        contents[j].gameObject.GetComponent<Text>().text = "";
        //    }
        //}

        ////records天數等于contents
        //else if (records.Count == contents.Count)
        //{
        //    for (int i = 0; i < records.Count; i++)
        //    {
        //        contents[i].gameObject.GetComponent<Text>().text = records[i].ToString();
        //    }
        //}

        ////records天數大于contents，輸入最后contetnts.count天的記錄
        //else if (records.Count > contents.Count)
        //{
        //    int j = 0;
        //    for (int i = records.Count - contents.Count; i < records.Count; i++)
        //    {
        //        contents[j].gameObject.GetComponent<Text>().text = records[i].ToString();
        //        j++;
        //    }
        //}  
        SetRecordsText(initialContentNum);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))CloseRecord();
    }
    
    public void CloseRecord()
    {
        gameObject.SetActive(false);
        //foreach(GameObject content in contents) 
        //{
        //    //content.text = "";
        //    //content.gameObject.GetComponent<Text>().text = "";
        //}
        initialContentNum = 7;
    } 

    public void SetRecordsText(int num)
    {
        foreach (GameObject contents in contents) Destroy(contents);
        GameObject gob;
        if (records.Count <= num)
        {
            for (int i = 0; i < records.Count; i++)
            {
                gob = Instantiate(recordTextPrefab, recordsView.transform);
                gob.gameObject.GetComponent<Text>().text = records[i].ToString();
                contents.Add(gob);
            }
        }
        else if(records.Count > num)
        {
            int j = records.Count - num;
            for (int i = 0; i < num; i++)
            {
                gob = Instantiate(recordTextPrefab, recordsView.transform);
                gob.gameObject.GetComponent<Text>().text = records[j].ToString();
                contents.Add(gob);
                j++;
            }
        }
    }

    void ShowTimerRecords() //records里的每一元素為每一天的記錄
    {
        tempRecord = "";
        record = "";
        //record = "\n --------------------  --------------------\n";
        records.Clear();
        bool newdayBegin = false;
        int j = 0;
        TimeSpan dr = new TimeSpan(); //傳遞計算縂時長        
        for (int i = 0; i < sourceList.Count; i++)
        {        
            //if(newdayBegin)record = " --------------------" + sourceList[i].StartTime.Date.ToShortDateString() + "  --------------------\n";
            tempRecord = sourceList[i].StartTime.ToShortTimeString() + " ~ ";
            if (sourceList[i].EndTime != new DateTime())
            {
                tempRecord += sourceList[i].EndTime.ToShortTimeString() + "     Duration:"
                    + Main_Menu.menu.FormatTimeSpan(sourceList[i].CalculateDuration()) + "\n";
                dr += sourceList[i].CalculateDuration();
            }
            record += tempRecord;
            tempDailyTS = Main_Menu.menu.FormatTimeSpan(dr); //記錄當日縂時長
            
            // if date changes 將前一日記錄賦給records，records內的記錄從index=0的元素起，日期依次增長index越大，日期越接近now
            if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
                && sourceList[i].StartTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date)
            {
                record = record + "\n total duration : " + tempDailyTS + "\n"; //在輸出次日日期前輸入當日縂時長
                record = record + "\n -------------------- " +
                   sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
                records.Insert(j, record);
                j++;
                tempRecord = "";
                record = "";
                dr = new TimeSpan();
            }
            if (i == sourceList.Count - 1 && !newdayBegin)
            {
                tempRecord = sourceList[i].StartTime.ToShortTimeString() + " ~ ";
                records.Insert(j, record); //將未結束的當天記錄計入records
            }
        }
    }

    void ShowCounterRecords() //records里的每一元素為每一天的記錄
    {
        tempRecord = "";
        record = "";
        records.Clear();

        int j = 0;
        int tempTotal = 0;
        for (int i = 0; i < sourceList.Count; i++)
        {
            tempRecord = sourceList[i].EndTime.ToShortTimeString() + "  " + sourceList[i].Number + sourceButton.GetComponent<Button_Entry>().unit + "\n";
            record += tempRecord;
            tempTotal += sourceList[i].Number;

            //將前一日記錄賦給records，records內的記錄從index = 0的元素起，日期依次增長index越大，日期越接近now
            if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
                && sourceList[i].EndTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].EndTime.Date)
            {
                record = record + "\n total amount : " + tempTotal + sourceButtonUnit + "\n"; //在輸出次日日期前輸入當日縂量
                record = record + "-------------------- " +
                  sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
                records.Insert(j, record);
                print("records[j]" + j + records[j]);
                j++;
                tempRecord = "";
                record = "";
                tempTotal = 0;
            }
            if (i == sourceList.Count - 1) records.Insert(j, record); 
        }
    }

    void ShowNappyRecords()
    {
        tempRecord = "";
        record = "";
        records.Clear();
        string wee, poo;

        int j = 0;
        int tempTotal = 0;
        for (int i = 0; i < sourceList.Count; i++)
        {
            if (sourceList[i].Wee) wee = "Wee  ";
            else wee = "";

            if (sourceList[i].Poo) poo = "Poo";
            else poo = "";

            tempRecord = string.Format("{0}   {1}{2}\n", sourceList[i].EndTime.ToShortTimeString(), wee, poo);
            record += tempRecord;
            tempTotal += sourceList[i].Number;

            //將前一日記錄賦給records，records內的記錄從index = 0的元素起，日期依次增長index越大，日期越接近now
            if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
                && sourceList[i].EndTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].EndTime.Date)
            {
                record = record + "\n total amount : " + tempTotal + sourceButtonUnit + "\n"; //在輸出次日日期前輸入當日縂量
                record = record + "-------------------- " +
                  sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
                records.Insert(j, record);
                print("records[j]" + j + records[j]);
                j++;
                tempRecord = "";
                record = "";
                tempTotal = 0;
            }
            if (i == sourceList.Count - 1) records.Insert(j, record);
        }
    }

    public void LoadMoreRecord()
    {
        contentsNumTotal = initialContentNum + everyClickAddContentNum;
        initialContentNum += everyClickAddContentNum;
        SetRecordsText(contentsNumTotal);
    }
}    

