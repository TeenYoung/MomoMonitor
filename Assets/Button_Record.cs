using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Record : MonoBehaviour {

    public GameObject recordsPanel, sourceButton;

    private List<Entry> sourceList;
    private string records;

    //to move out into a new .cs file
    public void OnClick()
    {
        sourceList = new List<Entry>();
        records = "";

        sourceList = Main_Menu.menu.entryLists[sourceButton.name];

        switch (sourceButton.GetComponent<Button_Entry>().buttonType)
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

        recordsPanel.SetActive(true);
        recordsPanel.GetComponentInChildren<Text>().text = records;
    }

    void ShowTimerRecords()
    {
        foreach (Entry entry in sourceList)
        {
            string record = entry.StartTime.ToShortTimeString() + " ~ ";
            if (entry.EndTime != new DateTime())
            {
                record += entry.EndTime.ToShortTimeString() + "     Duration:"
                    + Main_Menu.menu.FormatTimeSpan(entry.CalculateDuration()) + "\n";
            }

            records = records + record;

            if (sourceList.IndexOf(entry) != sourceList.Count - 1
                && entry.StartTime.Date != sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date)
            {
                records = records + "-------------------- " +
                    sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
            }

        }
    }

    void ShowCounterRecords()
    {
        foreach (Entry entry in sourceList)
        {
            string record = entry.EndTime.ToShortTimeString() + "  " + entry.Number + sourceButton.GetComponent<Button_Entry>().unit + "\n";

            records = records + record;

            if (sourceList.IndexOf(entry) != sourceList.Count - 1
                && entry.EndTime.Date != sourceList[sourceList.IndexOf(entry) + 1].EndTime.Date)
            {
                records = records + "-------------------- " +
                    sourceList[sourceList.IndexOf(entry) + 1].StartTime.Date.ToShortDateString() + " --------------------\n";
            }

        }
    }
}
