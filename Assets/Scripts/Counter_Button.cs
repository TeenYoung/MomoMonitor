using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Counter_Button : MonoBehaviour {

    public Text titleText, totalNumText, recordsText;
    public InputField inputNumField;
    public GameObject recordsPanel,inputPanel;

    public string title;
    public string unit;

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

        totalNumText.text = totalNum + " " + unit;

        //show daily feeding total at bottle buttom
        DailyTotalText.text = " / " + Convert.ToDecimal(PlayerPrefs.GetString("babyWeight")) * 140
            + "~" + Convert.ToDecimal(PlayerPrefs.GetString("babyWeight")) * 160 + unit;        
    }

    public void EnterToInput()
    {
        inputPanel.SetActive(true);
        inputNumField.Select();
    }

    public void EnterToShowTotal()
    {
        inputPanel.SetActive(false);
        number = Convert.ToInt32(inputNumField.text);
        totalNum += number;
        totalNumText.text = totalNum + " " + unit;

        //show daily feeding total at bottle buttom
        //DailyTotalText.text = " / " + PlayerPrefs.GetInt("babyWeight") * 140
        //   + " ~ " + PlayerPrefs.GetInt("babyWeight") * 160 + unit;

        counter = new Counter()
        {
            Number = number,
            Time = DateTime.Now
        };

        //save data
        PlayerPrefs.SetInt(title, totalNum);
        AddData();
        Main_Menu.menu.Save();

        // ??find a way to rest to default 
        inputNumField.text = "";
        
    }

    public void EnterToCancel() {
        inputPanel.SetActive(false);

        // ??find a way to rest to default 
        inputNumField.text = "";
    }

    void AddData()
    {

        // if add another countr button, have to manuly add a case here
        switch (gameObject.name)
        {
            case "Bottle_Button":
                Main_Menu.menu.bottleCounterList.Add(counter);
                break;
            case "Pump_Button":
                Main_Menu.menu.pumpCounterList.Add(counter);
                break;
        }
    }

    public void RecordOnClick()
    {
        List<Counter> sourceCounterList = new List<Counter>();
        string records = "";

        //if add another timer button, have to manuly add a case here
        switch (gameObject.name)
        {
            case "Bottle_Button":
                sourceCounterList = Main_Menu.menu.bottleCounterList;
                break;
            case "Pump_Button":
                sourceCounterList = Main_Menu.menu.pumpCounterList;
                break;

        }


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

