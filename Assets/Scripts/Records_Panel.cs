using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Records_Panel : MonoBehaviour
{

    public List<Text> contents;
    public GameObject sourceButton; //由button record傳入
    public int buttonType;
    public Text text;
    public string sourceButtonUnit;
    public GameObject scrollPanel;

    private List<Entry> sourceList;
    private List<string> records ;//every member keep the records in the same day
    private string record;//
    private string tempRecord;    

    //private string records;

    //public Button buttonAddRecord;

    // Use this for initialization at every time when this panel be called
    void OnEnable ()
    {
        records = new List<string>();
        sourceList = Main_Menu.menu.entryLists[name];
        print(buttonType);      
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
        //text.text = records;
        for (int j = 0; j < contents.Count; j++)
        {
            for (int i = 0; i < records.Count; i++)
            {
                contents[i].gameObject.GetComponent<Text>().text = records[i].ToString();
            }
            contents[j].gameObject.GetComponent<Text>().text = "";
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
        for (int i = 0; i < records.Count; i++)
        {
            contents[i].gameObject.GetComponent<Text>().text = "";
        }
        tempRecord = "";
        record = "";
        records.Clear();
    }

    void ShowTimerRecords()
    {
        //foreach (Entry entry in sourceList)
        //{
        //    string record = entry.StartTime.ToShortTimeString() + " ~ ";
        //    if (entry.EndTime != new DateTime())
        //    {
        //        record += entry.EndTime.ToShortTimeString() + "     Duration:"
        //            + Main_Menu.menu.FormatTimeSpan(entry.CalculateDuration()) + "\n";
        //    }

        //    records = records + record;
        //    //records.Add(record);

        //    if (sourceList.IndexOf(entry) != sourceList.Count - 1
        //        && entry.StartTime.Date != sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date)
        //    {
        //        records = records + "-------------------- " +
        //           sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
        //        //records.Add("-------------------- " + 
        //        //    sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date.ToShortDateString() + " --------------------\n");
        //    }
        //} 
        //records = new List<string>();
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
                //print(tempRecord);
                //print(i);
            }
            record += tempRecord;
            //print(record);

            //// if date changes
            if (sourceList.IndexOf(sourceList[i]) != sourceList.Count - 1
                && sourceList[i].StartTime.Date != sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date)
            {
                record = record + "-------------------- " +
                   sourceList[sourceList.IndexOf(sourceList[i]) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";                                                      
                records.Insert(j, record);
                //print("records[j]" + j + records[j]);
                //print(records.Capacity);
                j++;
                tempRecord = "";
                record = "";
                //print("j=" + j);                
            }
            if(i == sourceList.Count-1) records.Insert(j, record);
        }
    }

    void ShowCounterRecords()
    {
        tempRecord = "";
        record = "";
        records.Clear();

        int j = 0;        
        for (int i = 0; i < sourceList.Count; i++)
        {
            tempRecord = sourceList[i].EndTime.ToShortTimeString() + "  " + sourceList[i].Number + sourceButton.GetComponent<Button_Entry>().unit + "\n";
            record += tempRecord;
            //print(record);

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

            //foreach (Entry entry in sourceList)
            //{
            //    string record = entry.EndTime.ToShortTimeString() + "  " + entry.Number + sourceButtonUnit + "\n";

            //    records = records + record;
            //    //records.Add(record);

            //    if (sourceList.IndexOf(entry) != sourceList.Count - 1
            //        && entry.EndTime.Date != sourceList[sourceList.IndexOf(entry) + 1].EndTime.Date)
            //    {
            //        records = records + "-------------------- " +
            //          sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
            //        //records.Add("-------------------- " +
            //        //    sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date.ToShortDateString() +
            //        //    " --------------------\n");
            //    }

            //}

            //   

        }
    }
}    

