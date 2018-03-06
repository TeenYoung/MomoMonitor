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

    public void NextInputField()
    {
        inputField_2.Select();
    }

    public void InputFieldOnEndEdit()
    {
        inputString_1 = inputField_1.text;
        inputString_2 = inputField_2.text;

        if (sourceButton.GetComponent<Timer_Button>() != null)
        {
            Timer timer = sourceButton.GetComponent<Timer_Button>().ManualAddTimerRecord(inputString_1, inputString_2);
            
            if (timer != null)
            {
                Main_Menu.menu.timerLists[sourceButton.name].Add(timer);
                Main_Menu.menu.timerLists[sourceButton.name].Sort(new TimerComp());
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

        if (sourceButton.GetComponent<Counter_Button>() != null)
        {
            if (manualInputDateTime)
            {
                Counter counter = sourceButton.GetComponent<Counter_Button>().ManualAddCounterRecord(inputString_1, inputString_2);

                if(counter != null)
                {
                    Main_Menu.menu.counterLists[sourceButton.name].Add(counter);
                    Main_Menu.menu.counterLists[sourceButton.name].Sort(new CounterComp());

                    Main_Menu.menu.Save();
                    ClosePanel();
                }
                else if (inputString_2 == "")
                {
                    
                }
                else
                {
                    text_Warning.gameObject.SetActive(true);
                }

            }
            else
            {
                sourceButton.GetComponent<Counter_Button>().ConfirmInput();
            }
        }


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
