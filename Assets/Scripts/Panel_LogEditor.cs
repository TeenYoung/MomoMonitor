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
    //public DateTime logDate;
    public Log singleLog;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        
    }
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
