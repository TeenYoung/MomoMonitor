using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Calendar : MonoBehaviour {

    public Text month;
    public List<GameObject> days;

    public GameObject previousMonthButton, nextMonthButton, backToTodayButton;
    private DateTime today, DateTime_babyBirth;
    private int  monthTemp, dayTemp, daysInMonths, yearTemp, firstDayIndex;
    private string babyBirth, weekOfFirstDay; 

    private void OnEnable()
    {
        today = DateTime.Now;
        babyBirth = PlayerPrefs.GetString("babyBirth");
        //conver birthday formate from string to datetime
        DateTime_babyBirth = DateTime.ParseExact(babyBirth, "ddMMyyyy HHmm",
            CultureInfo.InvariantCulture, DateTimeStyles.None);
        BackToToday();       
    }

    public int ChangeWeekIntoNum(string week)
    {
        int num = 0;
        if(week == DayOfWeek.Sunday.ToString())num = 0;
        if (week == DayOfWeek.Monday.ToString()) num = 1;
        if (week == DayOfWeek.Tuesday.ToString()) num = 2;
        if (week == DayOfWeek.Wednesday.ToString()) num = 3;
        if (week == DayOfWeek.Thursday.ToString()) num = 4;
        if (week == DayOfWeek.Friday.ToString()) num = 5;
        else if(week == DayOfWeek.Saturday.ToString()) num = 6;
        return num;
    }      

    public void SetSpecialAge(GameObject gob, int day, int yearTemp, int monthTemp, int i)
    {
        gob.transform.Find("Text_SpecialAge").gameObject.SetActive(false);
        if ((yearTemp - DateTime_babyBirth.Year) * 12 + monthTemp - DateTime_babyBirth.Month >= 0 && day == DateTime_babyBirth.Day) //根據年齡顯示特殊日期，大於一個月顯示月，一年内按整月顯示，大於一年顯示年和周
        {
            gob.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
            if ((yearTemp - DateTime_babyBirth.Year) * 12 + monthTemp - DateTime_babyBirth.Month < 12) //age小於1嵗，
            {
                gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (yearTemp - DateTime_babyBirth.Year) * 12 + monthTemp - DateTime_babyBirth.Month + " Month";
            }
            if (yearTemp == DateTime_babyBirth.Year && monthTemp == DateTime_babyBirth.Month) gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = "Birthday";
            if ((yearTemp - DateTime_babyBirth.Year) * 12 + monthTemp - DateTime_babyBirth.Month >= 12) //age大於等於1嵗時，顯示整年和整月，月為0時衹顯示年
            {
                if (monthTemp == DateTime_babyBirth.Month) gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (yearTemp - DateTime_babyBirth.Year) + " Year ";
                else gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (((yearTemp - DateTime_babyBirth.Year) * 12 + monthTemp - DateTime_babyBirth.Month)) / 12 + " Y "
                + (((yearTemp - DateTime_babyBirth.Year) * 12 + monthTemp - DateTime_babyBirth.Month)) % 12 + " M";
            }            
        }
        //else if ((yearTemp - DateTime_babyBirth.Year) * 12 + monthTemp - DateTime_babyBirth.Month <= 3 && new DateTime(yearTemp, monthTemp, day) > DateTime_babyBirth) //小於三個月顯示周
        //{
        //    if ((new DateTime(yearTemp, monthTemp, day).Subtract(DateTime_babyBirth).Days + 1) % 7 == 0 && (new DateTime(yearTemp, monthTemp, day).Subtract(DateTime_babyBirth).Days) / 7 > 0)
        //    {
        //        gob.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
        //        gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (new DateTime(yearTemp, monthTemp, day).Subtract(DateTime_babyBirth).Days) / 7 + " Week";
        //    }
        //}
    }

    public void SetCalender(int yearTemp, int monthTemp)
    {
        int daysInMonths, firstDayIndex, dayTemp;         
        daysInMonths = DateTime.DaysInMonth(yearTemp, monthTemp);
        weekOfFirstDay = new DateTime(yearTemp, monthTemp, 1).DayOfWeek.ToString();
        firstDayIndex = ChangeWeekIntoNum(weekOfFirstDay);
        if(yearTemp!=DateTime.Now.Year) month.text = new DateTime(yearTemp, monthTemp, 1).ToString("MMMM", new CultureInfo("en-us")) //如不是當年月份，同時在月份後顯示年份
                + " " + yearTemp;
        else month.text = new DateTime(yearTemp, monthTemp, 1).ToString("MMMM", new CultureInfo("en-us")); //月份轉爲字母顯示
        int j = 1;
        dayTemp = DateTime_babyBirth.Day;
        for (int i = firstDayIndex; i <= daysInMonths + firstDayIndex ; i++) //填充當月日期,j為日期
        {
            days[i].transform.Find("Text_Day").GetComponent<Text>().text = j.ToString();
            //if (new DateTime(yearTemp, monthTemp, j) == DateTime.Today) days[i].gameObject.GetComponent<Image>().color = Color.gray;
            days[i].transform.Find("Text_Day").GetComponent<Text>().color = Color.black; //當前月字體為黑色
            SetSpecialAge(days[i], j, yearTemp,monthTemp, i);            
            j++;            
        }
        if (monthTemp == 1)
        {
            yearTemp--;
            monthTemp = 12;
        }
        else monthTemp--;
        j = DateTime.DaysInMonth(yearTemp, monthTemp);        
        for (int i = firstDayIndex - 1; i >= 0; i--) //填充上月末幾日
        {
            days[i].transform.Find("Text_Day").GetComponent<Text>().text = j.ToString();
            days[i].transform.Find("Text_Day").GetComponent<Text>().color = Color.gray; //非當前月字體為灰色            
            SetSpecialAge(days[i], j, yearTemp, monthTemp, i);
            j--;
        }
        j = 1;
        if (monthTemp == 12)
        {
            yearTemp++;
            monthTemp = 1;
        }
        else monthTemp++;
        for (int i = daysInMonths + firstDayIndex ; i < days.Count; i++) //填充下月初幾日
        {
            days[i].transform.Find("Text_Day").GetComponent<Text>().text = j.ToString();
            days[i].transform.Find("Text_Day").GetComponent<Text>().color = Color.gray; //非當前月字體為灰色            
            SetSpecialAge(days[i], j, yearTemp, monthTemp+1, i);
            j++;
        }        
    }
   
    public void ToNextMonth() //button click ToNextMonth
    {
        if (monthTemp == 12)
        {
            yearTemp++;
            monthTemp = 1;
        }
        else monthTemp++;
        //foreach (GameObject day in days)
        //{
        //    gameObject.GetComponent<Image>().color = Color.white;
        //}
        SetCalender(yearTemp, monthTemp);
    }

    public void ToPreviousMonth() //button click ToPreviousMonth
    {
        if (monthTemp == 1)
        {
            yearTemp--;
            monthTemp = 12;
        }
        else monthTemp--;
        SetCalender(yearTemp, monthTemp);
    }    

    public void BackToToday() //button BackToToday
    {        
        dayTemp = DateTime.Now.Day;
        monthTemp = DateTime.Now.Month;
        yearTemp = DateTime.Now.Year;
        SetCalender(yearTemp, monthTemp);
    }

    
}
