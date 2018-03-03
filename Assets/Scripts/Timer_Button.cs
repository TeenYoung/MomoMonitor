using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer_Button : MonoBehaviour {

    public Text titleText, timeText, recordsText, text_StatusTitle, text_LastTime;
    public string title;
    public GameObject recordsPanel; 
    public TimeSpan timeSpanFromLastTime;

    private string titleTiming, saveStartTime, saveEndTime;
    private bool timing = false;
    private int intTiming;
    private Timer timer;
    private TimeSpan duration;
    private TimeSpan totalDuration;

    // Use this for initialization
    void Start () {
        titleTiming = title + "ing";
        saveStartTime = "Start " + title;
        saveEndTime = "End" + title;

        if (PlayerPrefs.HasKey(title))
        totalDuration = TimeSpan.Parse(PlayerPrefs.GetString(title));

        if (PlayerPrefs.HasKey(titleTiming))
        {
            timing = Convert.ToBoolean(PlayerPrefs.GetInt(titleTiming));

            if (timing)
            {
                //when timing, show timing info
                titleText.gameObject.SetActive(true);
                timeText.gameObject.SetActive(true);
                text_StatusTitle.gameObject.SetActive(false);
                text_LastTime.gameObject.SetActive(false);

                //Grab the last start time from the player prefs as a long
                long temp = Convert.ToInt64(PlayerPrefs.GetString(saveStartTime));
                //Convert the last start time from binary to a DataTime variable
                timer = new Timer()
                {
                    StartTime = DateTime.FromBinary(temp)
                };
                titleText.text = titleTiming;
            }
            else
            {
                //when not timing, show total and last time info
                titleText.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
                text_StatusTitle.gameObject.SetActive(true);
                text_LastTime.gameObject.SetActive(true);

                //text_StatusTitle.text = FormatTimeSpan(totalDuration);
                text_LastTime.text = FormatTimeSpan(timeSpanFromLastTime) + " ago";
            }
        }

    }

    public void OnClick()
    {

        if (timing)
        {
            timer.EndTime = DateTime.Now;
            timeSpanFromLastTime = DateTime.Now.Subtract(timer.EndTime);

            titleText.text = title;
            totalDuration += duration;
            //save total duration into PlayerPrefs
            PlayerPrefs.SetString(title, totalDuration.ToString());


            timeText.text = FormatTimeSpan(totalDuration);
            timing = false;
            //save is timing or not into PlayerPrefs
            intTiming = Convert.ToInt32(timing);
            PlayerPrefs.SetInt(titleTiming, intTiming);

            //add data into log and save
            AddData();
            Main_Menu.menu.Save();

            //when not timing, show total and last time info
            titleText.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            text_StatusTitle.gameObject.SetActive(true);
            text_LastTime.gameObject.SetActive(true);

            text_StatusTitle.text = FormatTimeSpan(totalDuration);
            text_LastTime.text = FormatTimeSpan(timeSpanFromLastTime) + " ago";
        }
        else
        {
            timer = new Timer
            {
                StartTime = DateTime.Now
            };

            //Save the start time as a string in the player prefs class
            PlayerPrefs.SetString(saveStartTime, timer.StartTime.ToBinary().ToString());

            titleText.text = titleTiming;
            timing = true;
            //save is timing or not into PlayerPrefs
            intTiming = Convert.ToInt32(timing);
            PlayerPrefs.SetInt(titleTiming, intTiming);

            //when timing, show timing info
            titleText.gameObject.SetActive(true);
            timeText.gameObject.SetActive(true);
            text_StatusTitle.gameObject.SetActive(false);
            text_LastTime.gameObject.SetActive(false);
            
        }
    }

    public void RecordOnClick()
    {
        List<Timer> sourceTimerList = new List<Timer>();
        string records = "";

        //if add another timer button, have to manuly add a case here
        switch (gameObject.name)
        {
            case "Breastfeed_Button":
                sourceTimerList = Main_Menu.menu.bfTimerList;
                break;
            case "Sleep_Button":
                sourceTimerList = Main_Menu.menu.sleepTimerList;
                break;
            case "Play_Button":
                sourceTimerList = Main_Menu.menu.playTimerList;
                break;
        }


        foreach (Timer timer in sourceTimerList)
        {
            //print (timer.StartTime.ToLongTimeString() + " " 
            //    + timer.EndTime.ToLongTimeString() + " "
            //    + FormatTimeSpan(timer.CalculateDuration()));

            string record = timer.StartTime.ToShortTimeString() + " ~ "
                + timer.EndTime.ToShortTimeString() + "     Duration:"
                + FormatTimeSpan(timer.CalculateDuration()) + "\n";

            records = records + record;
        }

        recordsPanel.SetActive(true);
        recordsText.text = records;
    }

    public void CloseRecordOnClick()
    {
        recordsPanel.SetActive(false);
    }

    string FormatTimeSpan(TimeSpan timeSpan)
    {
        string d, h, m, s;

        if (timeSpan.Days == 0)
        {
            d = "";
        }
        else
        {
            d = timeSpan.Days + "d";
        }

        if (timeSpan.Hours == 0)
        {
            h = "";
            s = timeSpan.Seconds + "s";
        }
        else
        {
            h = timeSpan.Hours + "h";
            s = "";
        }


        if (timeSpan.Minutes == 0)
        {
            m = "";
            s = timeSpan.Seconds + "s";
        }
        else
            m = timeSpan.Minutes + "m";

        if (timeSpan.Seconds == 0)
            s = "";

        string dhms = d + h + m + s;
        return dhms;
    }


    // Update is called once per frame
    void Update()
    {

        if (timing)
        {
            duration = DateTime.Now.Subtract(timer.StartTime);
            timeText.text = FormatTimeSpan (duration);
        }
        else
        {            
            text_LastTime.text = FormatTimeSpan(timeSpanFromLastTime) + " ago";
        }
    }

    void AddData()
    {
        
        //if add another timer button, have to manuly add a case here
        switch (gameObject.name)
        {
            case "Breastfeed_Button":
                Main_Menu.menu.bfTimerList.Add(timer);
                break;
            case "Sleep_Button":
                Main_Menu.menu.sleepTimerList.Add(timer);
                break;
            case "Play_Button":
                Main_Menu.menu.playTimerList.Add(timer);
                break;
        }
    }
}

[Serializable]
public class Timer
{

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public TimeSpan CalculateDuration()
    {
        TimeSpan duration = EndTime.Subtract(StartTime);
        return duration;
    }
}
