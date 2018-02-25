using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer_Button : MonoBehaviour {

    public Text textTitle, textTime;
    public string TextTitleTotal, TextTitleTiming;

    private  bool timing = false;
    private Timer timer = new Timer();
    private TimeSpan duration;
    private TimeSpan totalDuration;

    // Use this for initialization
    void Start () {
    }

    public void Click()
    {

        if (timing)
        {
            timer.EndTime = DateTime.Now;
            textTitle.text = TextTitleTotal;
            totalDuration += duration;
            textTime.text = FormatTimeSpan(totalDuration);
            timing = false;
        }
        else
        {
            timer.StartTime = DateTime.Now;
            textTitle.text = TextTitleTiming;
            timing = true;
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
