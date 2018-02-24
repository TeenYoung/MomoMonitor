using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour {

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public TimeSpan CalculateDuration()
    {
        TimeSpan duration = EndTime.Subtract(StartTime);
        return duration;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
