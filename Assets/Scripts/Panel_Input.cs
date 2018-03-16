using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Input : MonoBehaviour {

    public InputField inputField_1, inputField_2;
    public Button sourceButton;
    public Text text_Placeholder_1, text_Placeholder_2, text_Title_1, text_Title_2, text_Warning;
    public bool manualInputDateTime;

    private string inputString_1, inputString_2;
    private Entry entry;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ClosePanel();
    }


    public void NextInputField()
    {
        inputField_2.Select();
    }

    public void InputFieldOnEndEdit()
    {
        inputString_1 = inputField_1.text;
        inputString_2 = inputField_2.text;

        switch (sourceButton.GetComponent<Button_Entry>().buttonType)
        {
            case 0:
                {
                    InputTimer();
                }
                break;

            case 1:
                {
                    InputCounter();
                }
                break;

            case 2:
                {

                }
                break;
        }
    }

    void InputTimer()
    {
        entry = ManualAddEntry(inputString_1, inputString_2);

        if (entry != null)
        {
            TimeSpan dr = entry.CalculateDuration();
            sourceButton.GetComponent<Button_Entry>().UpdateTotalDuration(dr);

            SaveEntry();
            ClosePanel();
        }
        else
        {
            text_Warning.gameObject.SetActive(true);
        }
    }

    void InputCounter()
    {
        int n = 0;

        if (inputString_2 != "")
        {
            n = Convert.ToInt32(inputString_2);
        }

        if (manualInputDateTime)
        {
            entry = ManualAddEntry(inputString_1, n);
        }
        else
        {
            entry = AutoAddEntry(n);
        }

        if (entry != null)
        {
            int number = entry.Number;
            sourceButton.GetComponent<Button_Entry>().UpdateTotalNumber(number);

            SaveEntry();
            ClosePanel();
        }
        else
        {
            text_Warning.gameObject.SetActive(true);
        }
    }

    void SaveEntry()
    {
        Main_Menu.menu.entryLists[sourceButton.name].Add(entry);
        Main_Menu.menu.entryLists[sourceButton.name].Sort(new EntryComp());
        Main_Menu.menu.Save();
    }

    //need to move it
    public Entry ManualAddEntry(string startTime, string endTime)
    {
        if (startTime.Length == 4 && endTime.Length == 4)
        {
            int statHr = Int32.Parse(startTime.Substring(0, 2));
            int statMin = Int32.Parse(startTime.Substring(2));
            int endHr = Int32.Parse(endTime.Substring(0, 2));
            int endMin = Int32.Parse(endTime.Substring(2));
            Entry entry = new Entry();
            
            if (statHr < 24 && endHr < 24
                && statMin < 60 && endMin < 60
                && statHr * 100 + statMin < endHr * 100 + endMin)
            {
                DateTime now = DateTime.Now;
                entry.StartTime = new DateTime(now.Year, now.Month, now.Day, statHr, statMin, 0);
                entry.EndTime = new DateTime(now.Year, now.Month, now.Day, endHr, endMin, 0);

                if (entry.EndTime < now) return entry;
                else return null;
            }
            else return null;
        }
        else return null;
    }

    public Entry ManualAddEntry(string endTime, int number)
    {
        if (endTime.Length == 4 && number > 0)
        {
            int endHr = Int32.Parse(endTime.Substring(0, 2));
            int endMin = Int32.Parse(endTime.Substring(2));
            Entry entry = new Entry();

            if (endHr < 24
                && endMin < 60)
            {
                DateTime now = DateTime.Now;
                entry.EndTime = new DateTime(now.Year, now.Month, now.Day, endHr, endMin, 0);
                entry.StartTime = entry.EndTime;
                entry.Number = number;

                if (entry.EndTime < now) return entry;
                else return null;
            }
            else return null;
        }
        else return null;
    }

    public Entry AutoAddEntry(int number)
    {
        if (number > 0)
        {
            Entry entry = new Entry
            {
                EndTime = DateTime.Now,
                StartTime = DateTime.Now,
                Number = number
            };
            return entry;
        }
        else return null;
    }

    public void ClosePanel()
    {
        inputField_1.text = "";
        inputField_2.text = "";

        inputField_1.gameObject.SetActive(false);
        text_Warning.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
