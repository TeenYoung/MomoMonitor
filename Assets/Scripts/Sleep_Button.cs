using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Sleep_Button : MonoBehaviour {

    public Text sleepDurationText;
    public Text sleepingText;

    private  bool sleeping = false;
    private Timer sleepTimer = new Timer();
    private TimeSpan sleepDuration;
    private TimeSpan totalSleepDuration;

    // Use this for initialization
    void Start () {
    }

    public void Click()
    {

        if (sleeping)
        {
            sleepTimer.EndTime = DateTime.Now;
            sleepingText.text = "total sleep";
            totalSleepDuration += sleepDuration;
            sleepDurationText.text = FormatTimeSpan(totalSleepDuration);
            sleeping = false;
        }
        else
        {
            sleepTimer.StartTime = DateTime.Now;
            sleepingText.text = "sleeping";
            sleeping = true;
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

        if (sleeping)
        {
            sleepDuration = DateTime.Now.Subtract(sleepTimer.StartTime);
            sleepDurationText.text = FormatTimeSpan (sleepDuration);
        }
    }
}
