using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Calendar : MonoBehaviour {
    //variable of panel_Canlendar
    public Text month;
    public List<GameObject> days,days1;
    public GameObject calendarDayPrefab, gameObjectDays;
    public GameObject previousMonthButton, nextMonthButton, backToTodayButton;

    private DateTime today, dateTime_babyBirth;
    private int  monthTemp, dayTemp, daysInMonths, yearTemp, firstDayIndex;
    private string babyBirth, weekOfFirstDay;
    private GameObject gob;
    //variable of panel_AddNote
    public GameObject panelAddNote;

    

    public void OnEnable()
    {        
        today = DateTime.Now;
        babyBirth = PlayerPrefs.GetString("babyBirth");
        //conver birthday formate from string to datetime
        dateTime_babyBirth = DateTime.ParseExact(babyBirth, "ddMMyyyy HHmm",
            CultureInfo.InvariantCulture, DateTimeStyles.None);
        BackToToday();
        
        //SetDays(today.Year,today.Month);
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

    //public void SetCalender(int thisYear, int thisMonth)
    //{
    //    int daysInThisMonths, firstDayIndex, lastMonth, lastYear,nextMonth, nextYear; //lastMonth, lastYear為日曆中上個月部分的入參，nextMonth, nextYear為日曆下個月部分的入參
    //    daysInThisMonths = DateTime.DaysInMonth(thisYear, thisMonth);
    //    weekOfFirstDay = new DateTime(thisYear, thisMonth, 1).DayOfWeek.ToString();
    //    firstDayIndex = ChangeWeekIntoNum(weekOfFirstDay);
    //    //foreach(GameObject days in days)
    //    //{
    //    //    days.transform.Find("Text_SpecialAge").gameObject.SetActive(false);
    //    //    //days.transform.Find("Text_SpecialAge").GetComponent<Text>().text = "SpecialAge";
    //    //    //days.transform.Find("Text_SpecialAge").GetComponent<Text>().color = Color.clear;
    //    //}                
    //    if (thisYear!=DateTime.Now.Year) month.text = new DateTime(thisYear, thisMonth, 1).ToString("MMMM", new CultureInfo("en-us")) //如不是當年月份，同時在月份後顯示年份
    //            + " " + thisYear;
    //    else month.text = new DateTime(thisYear, thisMonth, 1).ToString("MMMM", new CultureInfo("en-us")); //月份轉爲字母顯示
    //    if (thisMonth == 1)  //賦值日曆中上個月部分的入參，本月為1月時，上個月為12月，年份-1
    //    {
    //        lastYear = thisYear - 1;
    //        lastMonth = 12;
    //    }
    //    else
    //    {
    //        lastMonth = thisMonth - 1;
    //        lastYear = thisYear;
    //    }
    //    if (thisMonth == 12) //賦值日曆下個月部分的入參，本月為12月時，下個月為1月，年份+1
    //    {
    //        nextYear = thisYear + 1;
    //        nextMonth = 1;
    //    }
    //    else
    //    {
    //        nextMonth = thisMonth + 1;
    //        nextYear = thisYear;
    //    } 
    //    //dayTemp = DateTime_babyBirth.Day;             
    //    int j = DateTime.DaysInMonth(lastYear, lastMonth);        
    //    for (int i = firstDayIndex - 1; i >= 0; i--) //填充上月末幾日
    //    {
    //        days[i].transform.Find("Text_Day").GetComponent<Text>().text = j.ToString();
    //        //if (yearTemp == DateTime_babyBirth.Year && monthTemp == DateTime_babyBirth.Month && dayTemp == DateTime_babyBirth.Day) weekOfBirth = i;
    //        days[i].transform.Find("Text_Day").GetComponent<Text>().color = Color.gray; //非當前月字體為灰色            
    //        SetSpecialAge(days[i], j, lastYear, lastMonth, DateTime_babyBirth);
    //        j--;
    //    }
    //    j = 1;
    //    for (int i = firstDayIndex; i < daysInThisMonths + firstDayIndex; i++) //填充當月日期,j為日期
    //    {
    //        days[i].transform.Find("Text_Day").GetComponent<Text>().text = j.ToString();
    //        //if (yearTemp == DateTime_babyBirth.Year && monthTemp == DateTime_babyBirth.Month && j == DateTime_babyBirth.Day) weekOfBirth = i;
    //        //if (new DateTime(yearTemp, monthTemp, j) == DateTime.Today) days[i].gameObject.GetComponent<Image>().color = Color.gray;
    //        days[i].transform.Find("Text_Day").GetComponent<Text>().color = Color.black; //當前月字體為黑色
    //        SetSpecialAge(days[i], j, thisYear, thisMonth, DateTime_babyBirth);
    //        j++;
    //    } 
    //    j = 1;
    //    for (int i = daysInThisMonths + firstDayIndex; i < days.Count; i++) //填充下月初幾日
    //    {
    //        days[i].transform.Find("Text_Day").GetComponent<Text>().text = j.ToString();
    //        //if (yearTemp == DateTime_babyBirth.Year && monthTemp == DateTime_babyBirth.Month && dayTemp == DateTime_babyBirth.Day) weekOfBirth = i;
    //        days[i].transform.Find("Text_Day").GetComponent<Text>().color = Color.gray; //非當前月字體為灰色            
    //        SetSpecialAge(days[i], j, nextYear, nextMonth, DateTime_babyBirth);
    //        j++;
    //    }
    //}

    //public void SetSpecialAge(GameObject gob, int day, int thisYear, int thisMonth, DateTime DateTime_babyBirth)
    //{
    //    gob.transform.Find("Text_SpecialAge").gameObject.SetActive(false);        
    //    DateTime tempDT = new DateTime(thisYear, thisMonth, day);
    //    if ((thisYear - DateTime_babyBirth.Year) * 12 + thisMonth - DateTime_babyBirth.Month >= 0 && day == DateTime_babyBirth.Day) //根據年齡顯示特殊日期，大於一個月顯示月，一年内按整月顯示，大於一年顯示年和周
    //    {
    //        gob.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
    //        //gob.transform.Find("Text_SpecialAge").GetComponent<Text>().color = Color.HSVToRGB(157, 154, 214);
    //        if ((thisYear - DateTime_babyBirth.Year) * 12 + thisMonth - DateTime_babyBirth.Month < 12) //age小於1嵗，
    //        {
    //            gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (thisYear - DateTime_babyBirth.Year) * 12 + thisMonth - DateTime_babyBirth.Month + " Month";                
    //        }
    //        //if (thisYear == DateTime_babyBirth.Year && thisMonth == DateTime_babyBirth.Month)gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = "Birthday"; //生日當天
    //        if (thisYear == DateTime_babyBirth.Year && thisMonth == DateTime_babyBirth.Month)
    //        {
    //            gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = "Birthday";
    //            //gob.transform.Find("Text_SpecialAge").GetComponent<Text>().color = Color.HSVToRGB(0, 116, 251);
    //        }
    //        if ((thisYear - DateTime_babyBirth.Year) * 12 + thisMonth - DateTime_babyBirth.Month >= 12) //age大於等於1嵗時，顯示整年和整月，月為0時衹顯示年
    //        {
    //            if (thisMonth == DateTime_babyBirth.Month) gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (thisYear - DateTime_babyBirth.Year) + " Year ";
    //            else gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = (((thisYear - DateTime_babyBirth.Year) * 12 + thisMonth - DateTime_babyBirth.Month)) / 12 + " Y "
    //            + (((thisYear - DateTime_babyBirth.Year) * 12 + thisMonth - DateTime_babyBirth.Month)) % 12 + " M";
    //        }
    //    }
    //    if (tempDT.Subtract(DateTime_babyBirth.Date).Days < 91 && tempDT > DateTime_babyBirth)// && gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text == "SpecialAge") //小於三個月顯示周
    //    {
    //        if ((tempDT.Subtract(DateTime_babyBirth.Date).Days) % 7 == 0 && (tempDT.Subtract(DateTime_babyBirth.Date).Days) / 7 > 0)
    //        {
    //            print(tempDT);
    //            gob.transform.Find("Text_SpecialAge").gameObject.SetActive(true);
    //            gob.transform.Find("Text_SpecialAge").GetComponent<Text>().text = tempDT.Subtract(DateTime_babyBirth.Date).Days / 7 + " Week";
    //        }
    //    }
    //}

    public void ToNextMonth() //button click ToNextMonth
    {
        if (monthTemp == 12)
        {
            yearTemp++;
            monthTemp = 1;
        }
        else monthTemp++;
        //SetCalender(yearTemp, monthTemp);
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
        //SetCalender(yearTemp, monthTemp);
        SetDays(yearTemp, monthTemp);
    }    

    public void BackToToday() //button BackToToday
    {        
        dayTemp = DateTime.Now.Day;
        monthTemp = DateTime.Now.Month;
        yearTemp = DateTime.Now.Year;
        //SetCalender(yearTemp, monthTemp);
        SetDays(yearTemp, monthTemp);
    }

    //public void ClickOpenPanelAddNote(Transform gob)
    //{
    //    panelAddNote.gameObject.SetActive(true);
    //    //print(gob.transform.Find("Text_Day").GetComponent<Text>().text);
    //    //print(month.text);
    //}

    void SetDays(int thisYear, int thisMonth)
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
        if (days1 != null)
        {            
            foreach (GameObject days1 in days1) Destroy(days1);
            days1.Clear();
        }
        for (int i = 0; i < 42; i++)
        {
            gob = Instantiate(calendarDayPrefab, gameObjectDays.transform);
            days1.Add(gob);
            //dayTemp = DateTime_babyBirth.Day;
            if (i <= firstDayIndex - 1) //填充上月末幾日
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(lastYear, lastMonth, j, thisMonth, dateTime_babyBirth, panelAddNote);
                //if (yearTemp == DateTime_babyBirth.Year && monthTemp == DateTime_babyBirth.Month && dayTemp == DateTime_babyBirth.Day) weekOfBirth = i;
                j++;
            }
            if (i >= firstDayIndex && i < daysInThisMonths + firstDayIndex) //填充當月日期,j為日期
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(thisYear, thisMonth, j2, thisMonth, dateTime_babyBirth, panelAddNote);
                //if (yearTemp == DateTime_babyBirth.Year && monthTemp == DateTime_babyBirth.Month && j == DateTime_babyBirth.Day) weekOfBirth = i;
                if (new DateTime(yearTemp, monthTemp, j2) == DateTime.Today) days[i].gameObject.GetComponent<Image>().color = Color.gray;
                j2++;
            }
            if (i >= daysInThisMonths + firstDayIndex) //填充下月初幾日
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(nextYear, nextMonth, j3, thisMonth, dateTime_babyBirth, panelAddNote);
                //if (yearTemp == DateTime_babyBirth.Year && monthTemp == DateTime_babyBirth.Month && dayTemp == DateTime_babyBirth.Day) weekOfBirth = i;
                j3++;
            }
        }
    }
}
