using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Record : MonoBehaviour {

    public GameObject recordsPanel, sourceButton;

    //to move out into a new .cs file
    public void OnClick()
    {
        List<Entry> sourceList = new List<Entry>();
        string records = "";

        sourceList = Main_Menu.menu.entryLists[sourceButton.name];

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

        recordsPanel.SetActive(true);
        recordsPanel.GetComponentInChildren<Text>().text = records;
    }
}
