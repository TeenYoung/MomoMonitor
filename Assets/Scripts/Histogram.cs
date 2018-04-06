﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Histogram : MonoBehaviour {

    public GameObject number_TimeScalePrefab, gridlinePrefab, barPrefab, iconPrefab;
    public Transform content;

    Transform lineNow;
    float yNow;

	// Use this for initialization
	void Start () {
        //locate the line of now
        lineNow = transform.Find("Timeline");
        yNow = DateTime.Now.Hour * -60 - DateTime.Now.Minute;
        lineNow.GetComponent<RectTransform>().localPosition = new Vector3(460, yNow, 0);


        LayoutTimeScale();
        LayoutBarContents(Main_Menu.menu.sleepList, 120, Main_Menu.menu.colors[3]);//foreach list later
        LayoutIconContents(Main_Menu.menu.bottlefeedList, 60);//foreach list later
        LayoutBarContents(Main_Menu.menu.breastfeedList, 60, Main_Menu.menu.colors[2]);//foreach list later

    }


    void LayoutTimeScale()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject numberTS = Instantiate(number_TimeScalePrefab, gameObject.transform);
            numberTS.GetComponent<RectTransform>().localPosition = new Vector3(0, i * -60.0F, 0);
            numberTS.GetComponent<Text>().text = i.ToString();

            if (i % 3 == 0)
            {
                numberTS.GetComponent<Text>().fontSize = 30;
                numberTS.GetComponent<Text>().fontStyle = FontStyle.Bold;
                GameObject gridline = Instantiate(gridlinePrefab, gameObject.transform);
                gridline.GetComponent<RectTransform>().localPosition = new Vector3(470, i * -60.0F, 0);
            }
        }

        for (int j = 0; j < 7; j++)
        {
            GameObject day = Instantiate(number_TimeScalePrefab, content);
            day.GetComponent<RectTransform>().localPosition = new Vector3(2000 - 150f * j, -1500, 0);
            day.GetComponent<Text>().text = (DateTime.Now - new TimeSpan(j,0,0,0)).DayOfWeek.ToString();
        }
    }

    void LayoutBarContents(List<Entry> sourceList, float posX, Color color)
    {
        for (int i = 0; i < 7; i++)
        {
            foreach (Entry entry in sourceList)
            {
                TimeSpan delta = DateTime.Now.Date - entry.StartTime.Date;
                if (delta.Days == i)
                {
                    GameObject bar = Instantiate(barPrefab, content);
                    bar.GetComponent<Image>().color = color;
                    float yStart = entry.StartTime.Hour * -60 - entry.StartTime.Minute;
                    float yDuration = (float)entry.CalculateDuration().TotalMinutes;
                    if (yDuration < 0 && entry.StartTime.Date == DateTime.Now.Date)
                    {
                        yDuration = (float)DateTime.Now.Subtract(entry.StartTime).TotalMinutes;
                    }
                    else if (yDuration < 0) yDuration = 1440 + yStart;// !! incomplate

                    bar.GetComponent<RectTransform>().sizeDelta = new Vector2(50, yDuration);
                    bar.GetComponent<RectTransform>().localPosition = new Vector3(1900 - 150f * i + posX, yStart, 0);

                }
            }
        }
    }

    void LayoutIconContents(List<Entry> sourceList, float posX)
    {
        for (int i = 0; i < 7; i++)
        {
            foreach (Entry entry in sourceList)
            {
                TimeSpan delta = DateTime.Now.Date - entry.StartTime.Date;
                if (delta.Days == i)
                {
                    GameObject icon = Instantiate(iconPrefab, content);
                    float yStart = entry.StartTime.Hour * -60 - entry.StartTime.Minute;
                    float size = entry.Number;
                    if (size > 150) size = 150;
                    else if (size < 50) size = 50;

                    icon.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                    icon.GetComponent<RectTransform>().localPosition = new Vector3(1900 - 150f * i + posX, yStart, 0);

                }
            }
        }

    }
}