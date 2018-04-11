using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Button_CalendarDay : MonoBehaviour {

    public GameObject panelAddNote;
    public DateTime date;
    public Text textSpecialAge, textNote, textDay;
     

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickOpenPanelAddNote()
    {
        panelAddNote.SetActive(true);
        panelAddNote.GetComponent<Panel_AddNote>().GetNoteDate(date);
    }

    public void ShowDate(int year, int month, int day, int mainMonth, DateTime dateTime_babyBirth, GameObject panelAddNoteTemp)
    {
        textDay.text = day.ToString();
        if (month == mainMonth) textDay.color = Color.black; //非當前月字體為黑色
        else textDay.color = Color.gray;//非當前月字體為灰色
        SetSpecialAge(gameObject,day,year,month, dateTime_babyBirth);
        panelAddNote = panelAddNoteTemp;
        date = new DateTime(year, month, day);
        //print(date);
    }

    public void SetSpecialAge(GameObject gob, int day, int year, int month, DateTime dateTime_babyBirth)
    {
        gob.transform.Find("Text_SpecialAge").gameObject.SetActive(false);
        DateTime tempDT = new DateTime(year, month, day);
        if ((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month >= 0 && day == dateTime_babyBirth.Day) //根據年齡顯示特殊日期，大於一個月顯示月，一年内按整月顯示，大於一年顯示年和周
        {
            gob.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
            //gob.transform.Find("Text_SpecialAge").GetComponent<Text>().color = Color.HSVToRGB(157, 154, 214);
            if ((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month < 12) //age小於1嵗，
            {
                gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month + " Month";
            }
            //if (thisYear == DateTime_babyBirth.Year && thisMonth == DateTime_babyBirth.Month)gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = "Birthday"; //生日當天
            if (year == dateTime_babyBirth.Year && month == dateTime_babyBirth.Month)
            {
                gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = "Birthday";
                gob.transform.Find("Text_SpecialAge").GetComponent<Text>().color = Color.HSVToRGB(0, 116, 251);
            }
            if ((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month >= 12) //age大於等於1嵗時，顯示整年和整月，月為0時衹顯示年
            {
                if (month == dateTime_babyBirth.Month) gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (year - dateTime_babyBirth.Year) + " Year ";
                else gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month)) / 12 + " Y "
                + (((year - dateTime_babyBirth.Year) * 12 + month - dateTime_babyBirth.Month)) % 12 + " M";
            }
        }
        if (tempDT.Subtract(dateTime_babyBirth.Date).Days < 91 && tempDT > dateTime_babyBirth)// && gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text == "SpecialAge") //小於三個月顯示周
        {
            if ((tempDT.Subtract(dateTime_babyBirth.Date).Days) % 7 == 0 && (tempDT.Subtract(dateTime_babyBirth.Date).Days) / 7 > 0)
            {
                //print(tempDT);
                gob.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
                gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = tempDT.Subtract(dateTime_babyBirth.Date).Days / 7 + " Week";
            }
        }
    }
}
