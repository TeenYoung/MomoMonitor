using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Breast_Button : MonoBehaviour {

    public Text breasrDurationText;
    public Text breastFeedText;

    private  bool breastFeeding = false;
    private Timer breastTimer = new Timer();
    private TimeSpan breastDuration;
    private TimeSpan totalBreastDuration;

    // Use this for initialization
    void Start () {
    }

    public void Click()
    {

        if (breastFeeding)
        {
            breastTimer.EndTime = DateTime.Now;
            breastFeedText.text = "total breastfeed";
            totalBreastDuration += breastDuration;
            breasrDurationText.text = FormatTimeSpan(totalBreastDuration);
            breastFeeding = false;
        }
        else
        {
            breastTimer.StartTime = DateTime.Now;
            breastFeedText.text = "breastfeeding";
            breastFeeding = true;
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

        if (breastFeeding)
        {
            breastDuration = DateTime.Now.Subtract(breastTimer.StartTime);
            breasrDurationText.text = FormatTimeSpan (breastDuration);
        }
    }
}
