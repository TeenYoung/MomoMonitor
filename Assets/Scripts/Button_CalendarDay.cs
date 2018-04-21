using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Button_CalendarDay : MonoBehaviour {

    public GameObject panelLogs;
    public DateTime date;
    public Text  textDay, textSpecialAge, textNote;
    public Image imageLogTag;
    public List<String> notes,reminder,growth;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowDate(int year, int month, int day, int mainMonth, 
        DateTime dateTime_babyBirth, GameObject panelLogsTemp)
    {
        textDay.text = day.ToString();
        if (month == mainMonth) textDay.color = Color.black; //非當前月字體為黑色
        else textDay.color = Color.gray;//非當前月字體為灰色
        if (new DateTime(year, month, day) == DateTime.Today) gameObject.GetComponent<Image>().color = new Color(0.784f, 0.784f, 0.784f, 0.5f);
        SetSpecialAge(gameObject, day, year, month, dateTime_babyBirth);//,hasLog);        
        panelLogs = panelLogsTemp;
        date = new DateTime(year, month, day);
    }

    public void ClickOpenPanelLogs()
    {
        //panelLogs.GetComponent<Panel_Logs>().GetSourceButton(gameObject);
        panelLogs.GetComponent<Panel_Logs>().GetButtonDate(date);
        panelLogs.SetActive(true);
    }

    public void SetSpecialAge(GameObject buttonCalendarDay, int day, int year, int month, DateTime dateTime_babyBirth)
    {
        buttonCalendarDay.transform.Find("Text_SpecialAge").gameObject.SetActive(false);
        if (dateTime_babyBirth.Date.Year != 1900)
        {
            DateTime tempDT = new DateTime(year, month, day);
            if ((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month >= 0 && day == dateTime_babyBirth.Day) //根據年齡顯示特殊日期，大於一個月顯示月，一年内按整月顯示，大於一年顯示年和周
            {
                buttonCalendarDay.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
                if ((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month < 12) //age小於1嵗，
                {
                    buttonCalendarDay.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month + " Month";
                }
                if (year == dateTime_babyBirth.Year && month == dateTime_babyBirth.Month)//生日當天
                {
                    buttonCalendarDay.transform.Find("Text_SpecialAge").GetComponent<Text>().text = "Birthday";
                    buttonCalendarDay.transform.Find("Text_SpecialAge").GetComponent<Text>().color = new Color(0.984f,0.537f, 0.537f);
                }
                if ((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month >= 12) //age大於等於1嵗時，顯示整年和整月，月為0時衹顯示年
                {
                    if (month == dateTime_babyBirth.Month) buttonCalendarDay.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (year - dateTime_babyBirth.Year) + " Year ";
                    else buttonCalendarDay.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month)) / 12 + " Y "
                    + (((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month)) % 12 + " M";
                }
            }
            if (tempDT.Subtract(dateTime_babyBirth.Date).Days < 91 && tempDT > dateTime_babyBirth)//小於三個月顯示周
            {
                if ((tempDT.Subtract(dateTime_babyBirth.Date).Days) % 7 == 0 && (tempDT.Subtract(dateTime_babyBirth.Date).Days) / 7 > 0)
                {
                    buttonCalendarDay.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
                    buttonCalendarDay.transform.Find("Text_SpecialAge").GetComponent<Text>().text = tempDT.Subtract(dateTime_babyBirth.Date).Days / 7 + " Week";
                }
            }
        }           
    }

    public void SetLogTag()
    {
        imageLogTag.gameObject.SetActive(true);
    }
}
