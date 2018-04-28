using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Input : MonoBehaviour {

    public InputField inputField_1, inputField_2;
    public GameObject sourceButton; //對應的sourceButton，從button manualinput 傳入或button entry傳入
    public Text text_Placeholder_1, text_Placeholder_2, text_Title_1, text_Title_2, text_Warning;
    public bool manualInputDateTime;
    public int sourceButtonType; //對應的sourceButtonType，從button manualinput傳入或button entry傳入
    //public GameObject recordsPanel;
    public GameObject button_Ongoing;

    private string inputString_1, inputString_2;
    private Entry entry;
    //private Button_Entry sourceButtonEntry;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }


    public void NextInputField()
    {
        inputField_2.Select();
    }

    public void InputFieldOnEndEdit()
    {
        inputString_1 = inputField_1.text;
        inputString_2 = inputField_2.text;

        switch (sourceButtonType)
        {
            case 0:
                {
                    InputTimer(false);
                }
                break;

            case 1:
                {
                    InputCounter();
                }
                break;

            case 2:
                {
                    InputNappy();
                }
                break;
        }
    }

    public void ButtonOngoingOnClick()
    {
        inputString_1 = inputField_1.text;
        InputTimer(true);

        
    }

    void InputTimer(bool isOngoing)
    {
        if (isOngoing)
        {
            entry = ManualAddEntry(inputString_1, true);

            if (entry != null)
            {
                //TimeSpan dr = entry.CalculateDuration();
                //sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(dr);

                sourceButton.GetComponent<Button_Entry>().entry = entry;
                sourceButton.GetComponent<Button_Entry>().SetTimingTitle();
                sourceButton.GetComponent<Button_Entry>().timing = true;

                SaveEntry();
                ClosePanel();

            }
            else
            {
                text_Warning.gameObject.SetActive(true);
            }

        }
        else
        {
            entry = ManualAddEntry(inputString_1, inputString_2);

            if (entry != null)
            {
                TimeSpan dr = entry.CalculateDuration();
                sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(dr);

                SaveEntry();
                ClosePanel();

            }
            else
            {
                text_Warning.gameObject.SetActive(true);
            }

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
            //sourceButton = 
            sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(number);

            SaveEntry();
            ClosePanel();
        }
        else
        {
            text_Warning.gameObject.SetActive(true);
        }
    }

    void InputNappy()
    {
        entry = ManualAddEntry(inputString_2);

        if (entry != null)
        {
            transform.Find("Wee_Button").gameObject.SetActive(true);
            transform.Find("Poo_Button").gameObject.SetActive(true);
            transform.Find("Both_Button").gameObject.SetActive(true);

            Main_Menu.menu.entryLists[sourceButton.name].Add(entry);
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

    public Entry ManualAddEntry(string startTime, bool isOngoing)
    {
        if (startTime.Length == 4)
        {
            int statHr = Int32.Parse(startTime.Substring(0, 2));
            int statMin = Int32.Parse(startTime.Substring(2));
            Entry entry = new Entry();

            if (statHr < 24 && statMin < 60 )
            {
                DateTime now = DateTime.Now;
                entry.StartTime = new DateTime(now.Year, now.Month, now.Day, statHr, statMin, 0);

                int count = Main_Menu.menu.entryLists[sourceButton.name].Count;
                Entry lastEntry = Main_Menu.menu.entryLists[sourceButton.name][count - 1];
                if (entry.StartTime < now && entry.StartTime > lastEntry.EndTime) return entry;
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

                //待修完，儅正在計時時，增加的補充記錄，若其結束時間晚於計時的開始時間，輸入無效
                //if (entry.EndTime == new DateTime() && entry.EndTime > lastEntry.StartTime) ;
                else return null;
            }
            else return null;
        }
        else return null;
    }

    public Entry ManualAddEntry(string endTime)
    {
        if (endTime.Length == 4)
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
        inputField_2.gameObject.SetActive(true);
        text_Warning.gameObject.SetActive(false);
        button_Ongoing.SetActive(false);

        transform.Find("Wee_Button").gameObject.SetActive(false);
        transform.Find("Poo_Button").gameObject.SetActive(false);
        transform.Find("Both_Button").gameObject.SetActive(false);

        gameObject.SetActive(false);
        manualInputDateTime = false;

    }

    public void Wee()
    {

        if (manualInputDateTime)
        {
            int count = Main_Menu.menu.entryLists[sourceButton.name].Count;
            entry = Main_Menu.menu.entryLists[sourceButton.name][count - 1];
            Main_Menu.menu.entryLists[sourceButton.name].RemoveAt(count - 1);
        }
        else
        {
            entry = new Entry
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };
        }

        entry.Wee = true; entry.Poo = false;

        SaveEntry();
        ClosePanel();

        sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(entry.Wee, entry.Poo);

    }

    public void Poo()
    {
        if (manualInputDateTime)
        {
            int count = Main_Menu.menu.entryLists[sourceButton.name].Count;
            entry = Main_Menu.menu.entryLists[sourceButton.name][count - 1];
            Main_Menu.menu.entryLists[sourceButton.name].RemoveAt(count - 1);
        }
        else
        {
            entry = new Entry
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
            };
        }

        entry.Wee = false; entry.Poo = true;

        SaveEntry();
        ClosePanel();

        sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(entry.Wee, entry.Poo);

    }

    public void Both()
    {
        if (manualInputDateTime)
        {
            int count = Main_Menu.menu.entryLists[sourceButton.name].Count;
            entry = Main_Menu.menu.entryLists[sourceButton.name][count - 1];
            Main_Menu.menu.entryLists[sourceButton.name].RemoveAt(count - 1);
        }
        else
        {
            entry = new Entry
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
            };
        }

        entry.Wee = true; entry.Poo = true;

        SaveEntry();
        ClosePanel();

        sourceButton.GetComponent<Button_Entry>().ShowTodayAmount(entry.Wee,entry.Poo);
    }

}
