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
    public bool timing;
    public int babyAgeNum;
    public GameObject imageNotTiming, imageTiming;
    public Entry entry;

    private Entry lastEntry;
    private List<Entry> entrys; 
    private TimeSpan duration;
    private TimeSpan todayDuration, timeSpanFromLastTime;
    private int number, todayAmount, todayWee, todayPoo;
    private int deleteIndex;



    // Use this for initialization
    void Start () {

        //load last entry
        entrys = Main_Menu.menu.entryLists[gameObject.name];

        RefreshTexts();

        //determing and change the status of timer
        if (entry.StartTime != new DateTime() && entry.EndTime == new DateTime()) timing = true;
        else timing = false;

        if (timing)
        {
            text_Title.text = title + "ing";
            imageNotTiming.SetActive(false);
            imageTiming.SetActive(true);
        }

        if (gameObject.name == "Button_Bottle")
        {
            if (PlayerPrefs.HasKey("babyWeight"))
            {
                Decimal babyWeightTemp = Convert.ToDecimal(PlayerPrefs.GetString("babyWeight"));
                int babyAgeTemp = Convert.ToInt32(PlayerPrefs.GetString("babyAge"));
                Decimal babyFeedingTotalTemp = babyWeightTemp * (50 + 50 * babyAgeTemp);
                if (babyFeedingTotalTemp / babyWeightTemp > 150)
                    dailyTotalText.text = " / " + babyWeightTemp * 140 + unit;
                else dailyTotalText.text = " / " + babyFeedingTotalTemp.ToString() + unit;
            }

            else dailyTotalText.text = "";
        }

    }

    public void RefreshTexts()
    {
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
    }

    void CalculateAndShowSUM(int buttonType)
    {
        //lastEntry = entrys[entrys.Count - 1];
        todayDuration = new TimeSpan();
        todayAmount = 0; todayPoo = 0; todayWee = 0;

        switch (buttonType)
        {
            case 0:
                {
                    foreach (Entry entry in entrys)
                    {
                        if (entry.StartTime.Date == DateTime.Now.Date && entry.EndTime != new DateTime())
                        {
                            TimeSpan dr = entry.CalculateDuration();
                            todayDuration += dr;
                        }
                    }

                    if (todayDuration != new TimeSpan()) ShowTodayAmount(new TimeSpan());
                    else text_Title.text = title;
                }
                break;
                
            case 1:
                {
                    foreach (Entry entry in entrys)
                    {
                        if (entry.EndTime.Date == DateTime.Now.Date)
                        {
                            int n = entry.Number;
                            todayAmount += n;
                        }                            
                    }

                    if (todayAmount != 0) ShowTodayAmount(0);
                    else text_Title.text = title;
                }
                break;

            case 2:
                {
                    foreach (Entry entry in entrys)
                    {
                        if (entry.EndTime.Date == DateTime.Now.Date)
                        {
                            if (entry.Wee) todayWee++;
                            if (entry.Poo) todayPoo++;
                        }
                    }

                    if (todayPoo!=0 || todayWee !=0) ShowTodayAmount(false, false);
                    else text_Title.text = title;

                }
                break;
        }
    }

    public void OnClick()
    {
        panel_Input.gameObject.GetComponent<Panel_Input>().sourceButtonType = buttonType;
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
                    NappyOnClick();
                }
                break;
        }
        panel_Input.GetComponent<Panel_Input>().sourceButton = gameObject;
    }


    public void TimerOnClick()
    {
        if (timing)
        {
            timing = false;

            imageTiming.SetActive(false);
            imageNotTiming.SetActive(true);

            entry.EndTime = DateTime.Now;            

            entrys.RemoveAt(entrys.Count - 1);

            if (entry.StartTime.Date == entry.EndTime.Date)
            {
                entrys.Add(entry);
                Main_Menu.menu.Save();
            }

            else
            {
                Entry entry1 = new Entry()
                {
                    StartTime = entry.StartTime,
                    EndTime = new DateTime(entry.StartTime.Year, entry.StartTime.Month, entry.StartTime.Day, 23, 59, 59)
                };
                entrys.Add(entry1);

                Entry entry2 = new Entry()
                {
                    StartTime = new DateTime(entry.EndTime.Year, entry.EndTime.Month, entry.EndTime.Day, 0, 0, 0),
                    EndTime = entry.EndTime
                };
                entrys.Add(entry2);

                Main_Menu.menu.Save();
            }

            //calculate and show total duration
            ShowTodayAmount(duration);
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

            SetTimingTitle();
        }
    }

    public void SetTimingTitle()
    {
        imageTiming.SetActive(true);
        imageNotTiming.SetActive(false);
        text_Title.text = title + "ing";
    }

    public void CounterOnClick()
    {
        panel_Input.SetActive(true);
        //imageNotTiming.SetActive(true);

        Panel_Input pI = panel_Input.GetComponent<Panel_Input>();

        pI.sourceButton = gameObject;
        pI.manualInputDateTime = false;

        pI.inputField_2.Select();

        pI.text_Title_2.text = "Input number";
        pI.text_Placeholder_2.text = "unit: " + unit;

        pI.inputField_2.GetComponent<InputField>().characterLimit = 3;
    }

    public void NappyOnClick()
    {
        panel_Input.SetActive(true);
        panel_Input.transform.Find("Wee_Button").gameObject.SetActive(true);
        panel_Input.transform.Find("Poo_Button").gameObject.SetActive(true);
        panel_Input.transform.Find("Both_Button").gameObject.SetActive(true);

        Panel_Input pI = panel_Input.GetComponent<Panel_Input>();

        pI.sourceButton = gameObject;
        pI.inputField_2.gameObject.SetActive(false);
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
                lastEntry = entrys[entrys.Count - 1];

                    timeSpanFromLastTime = DateTime.Now.Subtract(lastEntry.EndTime);


                text_Property.text = Main_Menu.menu.FormatTimeSpan(timeSpanFromLastTime) + " ago";
                
            }
            else
            {
                text_Property.text = "No Record";
            }
        }
    }

    public void ShowTodayAmount(TimeSpan duration)
    {
        todayDuration += duration;
        text_Title.text = String.Format("{0} {1}", titlePast, Main_Menu.menu.FormatTimeSpan(todayDuration));
    }

    public void ShowTodayAmount(int amount)
    {
        todayAmount += amount;
        text_Title.text = String.Format("{0} {1}", titlePast, todayAmount);
    }

    public void ShowTodayAmount(bool isWee, bool isPoo)
    {
        if (isWee) todayWee++;
        if (isPoo) todayPoo++;

        text_Title.text = String.Format("Wee {0}   Poo {1}", todayWee, todayPoo);
    }
}

// intergrate timer, counter and nappy into this class
[Serializable]
public class Entry
{

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Number { get; set; }
    public bool Wee { get; set; }
    public bool Poo { get; set; }

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