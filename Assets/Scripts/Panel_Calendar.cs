using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Calendar : MonoBehaviour {
    //variable of panel_Canlendar
    public Text month;
    public List<GameObject> days;
    public GameObject calendarDayPrefab, gameObjectDays, panelLogs;
    public GameObject buttonBackToToday, buttonSeeAllLogs;

    private DateTime dateTime_babyBirth;
    private int  monthTemp, daysInMonths, yearTemp, firstDayIndex;
    private string babyBirth, weekOfFirstDay;

    List<Log> logs = new List<Log>();
    //public GameObject panelAddNote;    

    public void Start()
    {
        babyBirth = PlayerPrefs.GetString("babyBirth");

        //conver birthday formate from string to datetime
        if (babyBirth != "")
            dateTime_babyBirth = DateTime.ParseExact(babyBirth,
                "ddMMyyyy HHmm",
                CultureInfo.InvariantCulture, DateTimeStyles.None);
        else dateTime_babyBirth = new DateTime(1900, 02, 02);//若無生日輸入，設生日年份為1900，且之后不顯示special age
        BackToToday();
    }

    public void BackToCertainDay(DateTime date)
    {
        babyBirth = PlayerPrefs.GetString("babyBirth");
        //print(babyBirth);

        //conver birthday formate from string to datetime
        if (babyBirth != "")
            dateTime_babyBirth = DateTime.ParseExact(babyBirth,
                "ddMMyyyy HHmm",
                CultureInfo.InvariantCulture, DateTimeStyles.None);
        else dateTime_babyBirth = new DateTime(1900, 02, 02);//若無生日輸入，設生日年份為1900，且之后不顯示special age
        SetDays(date.Year,date.Month);
        monthTemp = date.Month;
        yearTemp = date.Year;
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
  
    public void ToNextMonth() //button click ToNextMonth
    {
        if (monthTemp == 12)
        {
            yearTemp++;
            monthTemp = 1;
        }
        else monthTemp++;
        SetDays(yearTemp, monthTemp);
    }

    public void ToPreviousMonth() //button click ToPreviousMonth
    {
        if (monthTemp == 1)
        {
            yearTemp--;
            monthTemp = 12;
        }
        else monthTemp--;
        SetDays(yearTemp, monthTemp);
    }

    public void ToNextYear()
    {
        yearTemp++;
        SetDays(yearTemp, monthTemp);
    }

    public void ToPreviousYear()
    {
        yearTemp--;
        SetDays(yearTemp, monthTemp);
    }

    public void BackToToday() //button BackToToday
    {
        monthTemp = DateTime.Now.Month;
        yearTemp = DateTime.Now.Year;
        SetDays(yearTemp, monthTemp);
    }

    public void GetLogs(List<Log> logListTemp)
    {
        logs = logListTemp;
    }

    public void SetDays(int thisYear, int thisMonth)
    {
        int daysInThisMonths, firstDayIndex, lastMonth, lastYear, nextMonth, nextYear; //lastMonth, lastYear為日曆中上個月部分的入參，nextMonth, nextYear為日曆下個月部分的入參
        daysInThisMonths = DateTime.DaysInMonth(thisYear, thisMonth);
        weekOfFirstDay = new DateTime(thisYear, thisMonth, 1).DayOfWeek.ToString();
        firstDayIndex = ChangeWeekIntoNum(weekOfFirstDay);
        if (thisYear != DateTime.Now.Year) month.text = new DateTime(thisYear, thisMonth, 1).ToString("MMMM", new CultureInfo("en-us")) //如不是當年月份，同時在月份後顯示年份
                  + " " + thisYear;
        else month.text = new DateTime(thisYear, thisMonth, 1).ToString("MMMM", new CultureInfo("en-us")); //月份轉爲字母顯示
        if (thisMonth == 1)  //賦值日曆中上個月部分的入參，本月為1月時，上個月為12月，年份-1
        {
            lastYear = thisYear - 1;
            lastMonth = 12;
        }
        else
        {
            lastMonth = thisMonth - 1;
            lastYear = thisYear;
        }
        if (thisMonth == 12) //賦值日曆下個月部分的入參，本月為12月時，下個月為1月，年份+1
        {
            nextYear = thisYear + 1;
            nextMonth = 1;
        }
        else
        {
            nextMonth = thisMonth + 1;
            nextYear = thisYear;
        }
        int j = DateTime.DaysInMonth(lastYear, lastMonth) - firstDayIndex + 1;
        int j2 = 1;
        int j3 = 1;
        if (days != null)
        {            
            foreach (GameObject days in days) Destroy(days);
            days.Clear();
        }
        GameObject gob;
        for (int i = 0; i < 42; i++)
        {
            gob = Instantiate(calendarDayPrefab, gameObjectDays.transform);
            days.Add(gob);
            if (i <= firstDayIndex - 1) //填充上月末幾日
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(lastYear, lastMonth, j, thisMonth, dateTime_babyBirth, panelLogs);//, SetLogTag(new DateTime(lastYear, lastMonth, j)));
                ShowLogTag(lastYear, lastMonth, j, gob);
                j++;
            }
            
            if (i >= firstDayIndex && i < daysInThisMonths + firstDayIndex) //填充當月日期,j為日期
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(thisYear, thisMonth, j2, thisMonth, dateTime_babyBirth, panelLogs);//, SetLogTag(new DateTime(lastYear, lastMonth, j2)));
                ShowLogTag(thisYear, thisMonth, j2, gob);
                if (new DateTime(yearTemp, monthTemp, j2) == DateTime.Today) days[i].gameObject.GetComponent<Image>().color = new  Color(0.784f, 0.784f, 0.784f);
                j2++;
            }
            if (i >= daysInThisMonths + firstDayIndex) //填充下月初幾日
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(nextYear, nextMonth, j3, thisMonth, dateTime_babyBirth, panelLogs);//, SetLogTag(new DateTime(lastYear, lastMonth, j3)));
                ShowLogTag(nextYear, nextMonth, j3, gob);                
                j3++;
            }
        }
    }

    public void ShowLogTag(int year, int month, int day,GameObject buttonDay)
    {
        for (int i = 0; i < logs.Count; i++)
        {
            if (logs[i].Date.Date == new DateTime(year,month, day).Date)
            {
                buttonDay.GetComponent<Button_CalendarDay>().SetLogTag();
            }
        }
    }
}
