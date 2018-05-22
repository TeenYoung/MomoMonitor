using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GrowthChart : MonoBehaviour
{

    LineRenderer lrWeight, lrHeight;

    public Vector2 weightPathStart, heightPathStart;
    public float widthModifier, heightModifier;

    DateTime birthday;

    private void Awake()
    {
        lrWeight = transform.Find("WeightPath").GetComponent<LineRenderer>();
        lrHeight = transform.Find("HeightPath").GetComponent<LineRenderer>();

        //conver birthday formate from string to datetime
        birthday = DateTime.ParseExact(PlayerPrefs.GetString("babyBirth"),
            "ddMMyyyy HHmm",
            CultureInfo.InvariantCulture, DateTimeStyles.None);
    }


    // Use this for initialization
    void Start()
    {
        RenderGrowthPath(Main_Menu.menu.weightList, weightPathStart, lrWeight);
        RenderGrowthPath(Main_Menu.menu.heightList, heightPathStart, lrHeight);
    }

    void RenderGrowthPath(List<Log> logList, Vector2 startPoint, LineRenderer lr)
    {
        Vector3[] positionArray = CalculateArray(logList, startPoint);

        lr.positionCount = positionArray.Length;
        lr.SetPositions(positionArray);
    }

    Vector3[] CalculateArray(List<Log> logList, Vector2 startPoint)
    {
        Vector3[] positionArray = new Vector3[logList.Count];

        for (int i = 0; i < logList.Count; i++)
        {
            positionArray[i].x = logList[i].Date.Subtract(birthday).Days * widthModifier + startPoint.x;
            positionArray[i].y = float.Parse(logList[i].Detail) / 10 * heightModifier + startPoint.y;
        }

        return positionArray;
    }
}