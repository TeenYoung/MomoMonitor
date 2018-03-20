using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Records_Panel : MonoBehaviour
{

    public List<Text> contents;
    public GameObject sourceButton; //由button record傳入
    public int buttonType; //由button record傳入
    public Text text;
    public string sourceButtonUnit;  //由button record傳入
    public GameObject scrollPanel;

    private List<Entry> sourceList;
    private List<string> records ;//every member keep the records in the same day
    private string record;//用于計算每日記錄，并寫入到records list里
    private string tempRecord;   // 暫時存儲每條記錄

    //private string records;

    //public Button buttonAddRecord;

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
        }
        
     //判斷records的天數和contents的大小，并顯示將同一日的records顯示在一個content內

        //若record天數小于contents，將無records的contents清空
        if (records.Count < contents.Count)
        {
            for (int j = 0; j < contents.Count; j++)
            {
                for (int i = 0; i < records.Count; i++)
                {
                    contents[i].gameObject.GetComponent<Text>().text = records[i].ToString();
                }
                contents[j].gameObject.GetComponent<Text>().text = "";
            }
        }

        //records天數等于contents
        else if (records.Count == contents.Count)
        {
            for (int i = 0; i < records.Count; i++)
            {
                contents[i].gameObject.GetComponent<Text>().text = records[i].ToString();
            }
        }

        //records天數大于contents，輸入最后contetnts.count天的記錄
        else if (records.Count > contents.Count)
        {
            int j = 0;
            for (int i = records.Count - contents.Count; i < records.Count; i++)
            {
                print("i =" + i + ":  " + records[i]);
                contents[j].gameObject.GetComponent<Text>().text = records[i].ToString();
                j++;
            }
        }               
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseRecord();
    }
    
    public void CloseRecord()
    {
        gameObject.SetActive(false);

        //reset all records
        //for (int i = 0; i < records.Count; i++)
        //{
        //    contents[i].gameObject.GetComponent<Text>().text = "";
        //}
        //tempRecord = "";
        //record = "";
        //records.Clear();
    }

    void ShowTimerRecords() //records里的每一元素為每一天的記錄
    {        
        tempRecord = "";
        record = "";
        records.Clear();

        int j = 0;
        for (int i = 0; i < sourceList.Count; i++)
        {
            tempRecord = sourceList[i].StartTime.ToShortTimeString() + " ~ ";
            if (sourceList[i].EndTime != new DateTime())
            {
                tempRecord += sourceList[i].EndTime.ToShortTimeString() + "     Duration:"
                    + Main_Menu.menu.FormatTimeSpan(sourceList[i].CalculateDuration()) + "\n";
            }
            record += tempRecord;

            // if date changes 將前一日記錄賦給records，records內的記錄從index=0的元素起，日期依次增長index越大，日期越接近now
            if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
                && sourceList[i].StartTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date)
            {
                record = record + "-------------------- " +
                   sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";                                                      
                records.Insert(j, record);
                j++;
                tempRecord = "";
                record = "";                
            }
            if(i == sourceList.Count-1) records.Insert(j, record);            
        }
    }

    void ShowCounterRecords() //records里的每一元素為每一天的記錄
    {
        tempRecord = "";
        record = "";
        records.Clear();

        int j = 0;        
        for (int i = 0; i < sourceList.Count; i++)
        {
            tempRecord = sourceList[i].EndTime.ToShortTimeString() + "  " + sourceList[i].Number + sourceButton.GetComponent<Button_Entry>().unit + "\n";
            record += tempRecord;

            //將前一日記錄賦給records，records內的記錄從index = 0的元素起，日期依次增長index越大，日期越接近now
            if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
                && sourceList[i].EndTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].EndTime.Date)
            {
                record = record + "-------------------- " +
                  sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
                records.Insert(j, record);
                print("records[j]" + j + records[j]);
                j++;
                tempRecord = "";
                record = "";
            }
            if (i == sourceList.Count - 1) records.Insert(j, record);
        }
    }
}    

