using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer_Button : MonoBehaviour {

    public Text textTitle, textTime;
    public string title;
    public List<Timer> timerList = new List<Timer>();

    private string titleTiming, saveStartTime;
    private bool timing = false;
    private int intTiming;
    private Timer timer;
    private TimeSpan duration;
    private TimeSpan totalDuration;

    // Use this for initialization
    void Start () {
        titleTiming = title + "ing";
        saveStartTime = "Start " + title;

        if (PlayerPrefs.HasKey(titleTiming))
        {
            timing = Convert.ToBoolean(PlayerPrefs.GetInt(titleTiming));

            if (timing)
            {
                //Grab the last start time from the player prefs as a long
                long temp = Convert.ToInt64(PlayerPrefs.GetString(saveStartTime));
                //Convert the last start time from binary to a DataTime variable
                timer = new Timer()
                {
                    StartTime = DateTime.FromBinary(temp)
                };

            }
        }

        if (PlayerPrefs.HasKey(title))
        totalDuration = TimeSpan.Parse(PlayerPrefs.GetString(title));

        textTitle.text = title;
        textTime.text = FormatTimeSpan(totalDuration);
    }

    public void OnClick()
    {

        if (timing)
        {
            timer.EndTime = DateTime.Now;

            textTitle.text = title;
            totalDuration += duration;
            //save total duration into PlayerPrefs
            PlayerPrefs.SetString(title, totalDuration.ToString());


            textTime.text = FormatTimeSpan(totalDuration);
            timing = false;
            //save is timing or not into PlayerPrefs
            intTiming = Convert.ToInt32(timing);
            PlayerPrefs.SetInt(titleTiming, intTiming);

            //add data into log and save
            timerList.Add(timer);

            Main_Menu.menu.Save();
        }
        else
        {
            timer = new Timer
            {
                StartTime = DateTime.Now
            };

            //Save the start time as a string in the player prefs class
            PlayerPrefs.SetString(saveStartTime, timer.StartTime.ToBinary().ToString());

            textTitle.text = titleTiming;
            timing = true;
            //save is timing or not into PlayerPrefs
            intTiming = Convert.ToInt32(timing);
            PlayerPrefs.SetInt(titleTiming, intTiming);
        }
    }

    public void RecordOnClick()
    {
        foreach (Timer timer in timerList)
        {
            print (timer.StartTime.ToLongTimeString() + " " 
                + timer.EndTime.ToLongTimeString() + " "
                + FormatTimeSpan(timer.CalculateDuration()));
        }
    }

    string FormatTimeSpan(TimeSpan timeSpan)
    {
        string h, m, s;


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

        string hms = h + m + s;
        return hms;
    }


    // Update is called once per frame
    void Update()
    {

        if (timing)
        {
            duration = DateTime.Now.Subtract(timer.StartTime);
            textTime.text = FormatTimeSpan (duration);
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
