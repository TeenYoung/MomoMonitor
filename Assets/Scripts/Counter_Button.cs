using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Counter_Button : MonoBehaviour {

    public Text titleText, totalNumText, recordsText;
    public GameObject recordsPanel, panel_Input;

    public string title;
    public string unit;

    private string saveTime;
    Panel_Input pI;
    private int number, totalNum;
    private Counter counter;

    //use for daily feeding estimate
    //private int dailyTotal;
    public Text DailyTotalText;
    
    // Use this for initialization
    void Start() {
        titleText.text = title;     
       

        //load saved data
        if (PlayerPrefs.HasKey(title))
            totalNum = PlayerPrefs.GetInt(title);

        //Grab the last start time from the player prefs as a long
        long temp = Convert.ToInt64(PlayerPrefs.GetString(saveTime));
        //Convert the last start time from binary to a DataTime variable
        counter = new Counter()
        {
            Time = DateTime.FromBinary(temp)
        };

        totalNumText.text = totalNum + " " + unit;

        //show daily feeding total at bottle buttom
        if (gameObject.name == "Bottle_Button")
        {            
            if(PlayerPrefs.HasKey("babyWeight"))              
            DailyTotalText.text = " / " + Convert.ToDecimal(PlayerPrefs.GetString("babyWeight")) * 140
            + "~" + Convert.ToDecimal(PlayerPrefs.GetString("babyWeight")) * 160 + unit;
            else DailyTotalText.text = "daily feeding base on weight";                
        }

        pI = panel_Input.GetComponent<Panel_Input>();

    }

    public void OnClick(Button button)
    {
        panel_Input.SetActive(true);

        pI.sourceButton = button;
        pI.manualInputDateTime = false;

        pI.inputField_2.Select();

        pI.text_Title_2.text = "Input volume";
        pI.text_Placeholder_2.text = "unit: ml";

        pI.inputField_2.GetComponent<InputField>().characterLimit = 3;

    }

    public void ConfirmInput()
    {
        panel_Input.SetActive(false);
        if(pI.inputField_2.text.Length > 0)
        {
            number = Convert.ToInt32(pI.inputField_2.text);
            totalNum += number;
            totalNumText.text = totalNum + " " + unit;

            counter = new Counter()
            {
                Number = number,
                Time = DateTime.Now
            };

            //save data
            PlayerPrefs.SetInt(title, totalNum);
            //Save the start time as a string in the player prefs class
            PlayerPrefs.SetString(saveTime, counter.Time.ToBinary().ToString());


            AddData();
            Main_Menu.menu.Save();
        }

        // ??find a way to rest to default 
        pI.inputField_2.text = "";

    }


    void AddData()
    {
        Main_Menu.menu.counterLists[gameObject.name].Add(counter);
    }


    public void ManualInputOnClick(Button button)
    {
        panel_Input.SetActive(true);

        pI.sourceButton = button;
        pI.manualInputDateTime = true;

        pI.inputField_1.gameObject.SetActive(true);
        pI.inputField_1.Select();

        pI.text_Title_1.text = "Input time";
        pI.text_Title_2.text = "Input volume";
        pI.text_Placeholder_1.text = "hhmm e.g. 1800 for 6pm";
        pI.text_Placeholder_2.text = "unit: ml";

        pI.inputField_1.GetComponent<InputField>().characterLimit = 4;
        pI.inputField_2.GetComponent<InputField>().characterLimit = 3;
    }

    public Counter ManualAddCounterRecord(string time, string volume)
    {
        if (time.Length == 4 && volume.Length >0)
        {
            int Hr = Int32.Parse(time.Substring(0, time.Length - 2));
            int Min = Int32.Parse(time.Substring(time.Length - 2));
            Counter counter = new Counter();

            if (Hr < 24 && Min < 60 )
            {
                DateTime now = DateTime.Now;
                counter.Time = new DateTime(now.Year, now.Month, now.Day, Hr, Min, 0);
                counter.Number = Int32.Parse(volume);

                return counter;
            }
            else return null;
        }
        else return null;
    }


    public void RecordOnClick()
    {
        List<Counter> sourceCounterList = new List<Counter>();
        string records = "";

        sourceCounterList = Main_Menu.menu.counterLists[gameObject.name];

        foreach (Counter counter in sourceCounterList)
        {

            string record = counter.Time.ToShortTimeString() + "    "
                + counter.Number + " " + unit + "\n";

            records = records + record;
        }

        recordsPanel.SetActive(true);
        recordsText.text = records;
    }

}

[Serializable]
public class Counter
{

    public int Number { get; set; }
    public DateTime Time { get; set; }

}

public class CounterComp : IComparer<Counter>
{
    public int Compare(Counter x, Counter y)
    {
        return x.Time.CompareTo(y.Time);
    }
}

