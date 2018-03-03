using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer_Button : MonoBehaviour {

    public Text titleText, timeText, recordsText, text_StatusTitle, text_LastTime;
    public string title,titlePast;
    public GameObject recordsPanel; 

    private string titleTiming, saveStartTime, saveEndTime;
    private bool timing = false;
    private int intTiming;
    private Timer timer;
    private TimeSpan duration;
    private TimeSpan totalDuration, timeSpanFromLastTime;

    // Use this for initialization
    void Start () {
        titleTiming = title + "ing";
        saveStartTime = "Start " + title;
        saveEndTime = "End" + title;

        if (PlayerPrefs.HasKey(title))
        {
            totalDuration = TimeSpan.Parse(PlayerPrefs.GetString(title));
            text_StatusTitle.text = titlePast + ": " + FormatTimeSpan(totalDuration);
        }
        else text_StatusTitle.text = title;
         


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
                //Grab the last start time from the player prefs as a long
                long temp = Convert.ToInt64(PlayerPrefs.GetString(saveEndTime));
                //Convert the last start time from binary to a DataTime variable
                timer = new Timer()
                {
                    EndTime = DateTime.FromBinary(temp)
                };

                //when not timing, show total and last time info
                titleText.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
                text_StatusTitle.gameObject.SetActive(true);
                text_LastTime.gameObject.SetActive(true);
            }



        }

    }

    public void OnClick()
    {

        if (timing)
        {
            timer.EndTime = DateTime.Now;
            PlayerPrefs.SetString(saveEndTime, timer.EndTime.ToBinary().ToString());


            //timeSpanFromLastTime = DateTime.Now.Subtract(timer.EndTime);

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

            text_StatusTitle.text = titlePast + ": " + FormatTimeSpan(totalDuration);
            //text_LastTime.text = FormatTimeSpan(timeSpanFromLastTime) + " ago";
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

        sourceTimerList = Main_Menu.menu.timerLists[gameObject.name];

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
        string  d, h, m, dhm;

        if (timeSpan > new TimeSpan(0,0,59))
        {
            if (timeSpan.Days == 0)
            {
                d = "";
                m = timeSpan.Minutes + "m";
            }
            else
            {
                d = timeSpan.Days + "d";
                m = "";
            }


            if (timeSpan.Hours == 0)
            {
                h = "";
                m = timeSpan.Minutes + "m";
            }
            else
                h = timeSpan.Hours + "h";

            if (timeSpan.Minutes == 0)
                m = "";

            dhm = d + h + m;
        }
        else dhm = "< 1min";


        return dhm;
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
            if (PlayerPrefs.HasKey(saveEndTime))
            {
                timeSpanFromLastTime = DateTime.Now.Subtract(timer.EndTime);
                text_LastTime.text = FormatTimeSpan(timeSpanFromLastTime) + " ago";
            }
            else
            {
                text_LastTime.text = "Never";
            }
        }
    }

    void AddData()
    {

        Main_Menu.menu.timerLists[gameObject.name].Add(timer);

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
