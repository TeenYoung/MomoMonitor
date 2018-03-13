using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Input : MonoBehaviour {

    public InputField inputField_1, inputField_2;
    public Button sourceButton;
    public Text text_Placeholder_1, text_Placeholder_2, text_Title_1, text_Title_2, text_Warning;
    public bool manualInputDateTime;

    private string inputString_1, inputString_2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ClosePanel();
    }


    public void NextInputField()
    {
        inputField_2.Select();
    }

    public void InputFieldOnEndEdit()
    {
        inputString_1 = inputField_1.text;
        inputString_2 = inputField_2.text;

        if (sourceButton.GetComponent<Button_Entry>().buttonType == 0)
        {
            Entry entry = ManualAddTimerRecord(inputString_1, inputString_2);

            if (entry != null)
            {
                TimeSpan dr = entry.CalculateDuration();
                sourceButton.GetComponent<Button_Entry>().UpdateTotalDuration(dr);

                Main_Menu.menu.entryLists[sourceButton.name].Add(entry);
                Main_Menu.menu.entryLists[sourceButton.name].Sort(new EntryComp());
                Main_Menu.menu.Save();
                ClosePanel();
            }
            else if(inputString_2 == "")
            {
                
            }
            else
            {
                text_Warning.gameObject.SetActive(true);
            }
        }

        //if (sourceButton.GetComponent<Counter_Button>() != null)
        //{
        //    if (manualInputDateTime)
        //    {
        //        Counter counter = sourceButton.GetComponent<Counter_Button>().ManualAddCounterRecord(inputString_1, inputString_2);

        //        if(counter != null)
        //        {
        //            Main_Menu.menu.counterLists[sourceButton.name].Add(counter);
        //            Main_Menu.menu.counterLists[sourceButton.name].Sort(new CounterComp());

        //            Main_Menu.menu.Save();
        //            ClosePanel();
        //        }
        //        else if (inputString_2 == "")
        //        {
                    
        //        }
        //        else
        //        {
        //            text_Warning.gameObject.SetActive(true);
        //        }

        //    }
        //    else
        //    {
        //        sourceButton.GetComponent<Counter_Button>().ConfirmInput();
        //    }
        //}
    }

    //need to move it
    public Entry ManualAddTimerRecord(string startTime, string endTime)
    {
        if (startTime.Length == 4 && endTime.Length == 4)
        {
            int statHr = Int32.Parse(startTime.Substring(0, startTime.Length - 2));
            int statMin = Int32.Parse(startTime.Substring(startTime.Length - 2));
            int endHr = Int32.Parse(endTime.Substring(0, startTime.Length - 2));
            int endMin = Int32.Parse(endTime.Substring(startTime.Length - 2));
            Entry entry = new Entry();
            
            if (statHr < 24 && endHr < 24
                && statMin < 60 && endMin < 60
                && statHr * 100 + statMin < endHr * 100 + endMin)
            {
                DateTime now = DateTime.Now;
                entry.StartTime = new DateTime(now.Year, now.Month, now.Day, statHr, statMin, 0);
                entry.EndTime = new DateTime(now.Year, now.Month, now.Day, endHr, endMin, 0);

                if (entry.EndTime < now) return entry;
                else return null;
            }
            else return null;
        }
        else return null;
    }

    public void ClosePanel()
    {
        inputField_1.text = "";
        inputField_2.text = "";

        inputField_1.gameObject.SetActive(false);
        text_Warning.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
