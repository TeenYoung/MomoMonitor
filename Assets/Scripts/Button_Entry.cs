using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Button_Entry : MonoBehaviour {

    public Text recordsText, text_Title, text_Property;
    public string title, titlePast, unit;
    public GameObject recordsPanel, panel_Input;
    public int buttonType; // 0 : timer   1 : counter   2 : nappy
    public Text dailyTotalText;

    private bool timing;
    private Entry entry;
    private List<Entry> entrys;
    private TimeSpan duration;
    private TimeSpan totalDuration, timeSpanFromLastTime;
    private int number, totalNum;


    // Use this for initialization
    void Start () {

        //load last entry
        entrys = Main_Menu.menu.entryLists[gameObject.name];

        //load total duration and update it
        if (entrys.Count != 0)
        {
            entry = entrys[entrys.Count - 1];

            CalculateAndShowSUM(buttonType);
        }
        else
        {
            entry = new Entry();
            text_Title.text = title;
        }

        //a switch case needed here
        //determing and change the status of timer
        if (entry.StartTime != new DateTime() && entry.EndTime == new DateTime()) timing = true;
        else timing = false;

        if (timing)
        {
            //maybe need another public string
            text_Title.text = title + "ing";
        }

        if (gameObject.name == "Button_Bottle")
        {
            if (PlayerPrefs.HasKey("babyWeight"))
            {
                Decimal babyWeightTemp = Convert.ToDecimal(PlayerPrefs.GetString("babyWeight"));
                int babyAgeTemp = Convert.ToInt16(PlayerPrefs.GetInt("babyAge"));
                Decimal babyFeedingTotalTemp = babyWeightTemp * (50 + 50 * babyAgeTemp);
                if (babyFeedingTotalTemp / babyWeightTemp >= 150)
                    dailyTotalText.text = " / " + babyWeightTemp * 140 + unit;
                else dailyTotalText.text = " / " + babyFeedingTotalTemp.ToString() + unit;
            }

            else dailyTotalText.text = "daily feeding base on weight";
        }

    }

    void CalculateAndShowSUM(int buttonType)
    {
        switch (buttonType)
        {
            case 0:
                {
                    foreach (Entry entry in entrys)
                    {
                        if (entry.EndTime != new DateTime())
                        {
                            TimeSpan dr = entry.CalculateDuration();
                            totalDuration += dr;
                        }
                    }

                    UpdateTotalDuration();
                }
                break;

            case 1:
                {
                    foreach (Entry entry in entrys)
                    {
                        int n = entry.Number;
                        totalNum += n;
                    }

                    UpdateTotalNumber();
                }
                break;

            case 2:
                {

                }
                break;
        }
    }

    public void OnClick()
    {
        //call different fuction depend on button type
        switch (buttonType)
        {
            case 0:
                {
                    TimerOnClick();
                }
                break;

            case 1:
                {
                    CounterOnClick();
                }
                break;

            case 2:
                {

                }
                break;
        }
    }


    public void TimerOnClick()
    {
        if (timing)
        {

            timing = false;

            entry.EndTime = DateTime.Now;

            //add data into log and save
            entrys.RemoveAt(entrys.Count - 1);
            entrys.Add(entry);
            Main_Menu.menu.Save();

            //calculate and show total duration
            UpdateTotalDuration(duration);
        }
        else
        {
            timing = true;
            entry = new Entry
            {
                StartTime = DateTime.Now
            };

            //add data into log and save
            entrys.Add(entry);
            Main_Menu.menu.Save();

            text_Title.text = title + "ing";

        }
    }

    public void CounterOnClick()
    {
        panel_Input.SetActive(true);

        Panel_Input pI = panel_Input.GetComponent<Panel_Input>();

        pI.sourceButton = gameObject.GetComponent<Button>();
        pI.manualInputDateTime = false;

        pI.inputField_2.Select();

        pI.text_Title_2.text = "Input number";
        pI.text_Placeholder_2.text = "unit: " + unit;

        pI.inputField_2.GetComponent<InputField>().characterLimit = 3;
    }
    
    // Update is called once per frame
    void Update()
    {
        // to add a switch case 
        if (buttonType == 0 && timing)
        {
            duration = DateTime.Now.Subtract(entry.StartTime);
            text_Property.text = Main_Menu.menu.FormatTimeSpan(duration);
        }
        else
        {
            if (entrys.Count != 0)
            {
                Entry lastEntry = entrys[entrys.Count - 1];

                if (entry.EndTime != new DateTime() && entry.EndTime >= lastEntry.EndTime)
                {
                    timeSpanFromLastTime = DateTime.Now.Subtract(entry.EndTime);
                }
                else
                {
                    timeSpanFromLastTime = DateTime.Now.Subtract(lastEntry.EndTime);
                }

                text_Property.text = Main_Menu.menu.FormatTimeSpan(timeSpanFromLastTime) + " ago";
                
            }
            else if (buttonType == 0)
            {
                text_Property.text = "Tap to Start";
            }
            else
            {
                text_Property.text = "Tap to Input";
            }
        }
    }

    public void UpdateTotalDuration()
    {
        text_Title.text = titlePast + " " + Main_Menu.menu.FormatTimeSpan(totalDuration);
    }

    public void UpdateTotalDuration(TimeSpan addTimeSpan)
    {
        totalDuration += addTimeSpan;
        text_Title.text = titlePast + ": " + Main_Menu.menu.FormatTimeSpan(totalDuration);
    }

    public void UpdateTotalNumber()
    {
        text_Title.text = titlePast + " " + totalNum + " " + unit;
    }

    public void UpdateTotalNumber(int addNumber)
    {
        totalNum += addNumber;
        text_Title.text = titlePast + " " + totalNum + " " + unit;
    }

}

// intergrate timer, counter and nappy into this class
[Serializable]
public class Entry
{

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Number { get; set; }

    public TimeSpan CalculateDuration()
    {
        TimeSpan duration = EndTime.Subtract(StartTime);
        return duration;
    }
}

public class EntryComp : IComparer<Entry>
{
    public int Compare(Entry x, Entry y)
    {
        if (x.StartTime == y.StartTime)
        {
            return x.EndTime.CompareTo(y.EndTime);
        }
        else return x.StartTime.CompareTo(y.StartTime);
    }
}