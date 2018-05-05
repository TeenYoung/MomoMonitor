using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Panel_LogEditor : MonoBehaviour {

    public InputField inputFieldLog;
    public Text textLogDate;
    public GameObject panelCalendar;
    public Log singleLog;


    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) panelCalendar.SetActive(true) ;
	}   

    public void GetLog(Log log)
    {
        singleLog = log;
        textLogDate.text = singleLog.Date.Day + " " +
            singleLog.Date.ToString("MMM", new CultureInfo("en-us"))
            + " " + singleLog.Date.Year;
    }
}
