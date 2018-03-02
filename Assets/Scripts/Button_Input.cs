using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Button_Input : MonoBehaviour {

    public InputField inputNumField;
    public Text Text_Num;

    //give title a name for saving input
    public string unit, titleInSaving;
    
    private string number;

    //use for data saving log
    private DateTime InputTime;

    // Use this for initialization
    void Start () {

        inputNumField.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenInputField()
    {
        //clean inputfield
        inputNumField.text = "";
        
        inputNumField.gameObject.SetActive(true);
        inputNumField.Select();
    }

    public void InputConfirm()
    {
        inputNumField.gameObject.SetActive(false);

        //number = Convert.ToString(inputNumField.text);
        
        Text_Num.text = Convert.ToString((inputNumField.text)) + " " + unit;     

        //save update time
        InputTime = DateTime.Now;

        //add code to saving update time in log

        //save data
        PlayerPrefs.SetString(titleInSaving, Text_Num.text);        

        //inputNumField.text = "";
    }

    public void InputCancel()
    {
        //inputNumField.text = "";
        inputNumField.gameObject.SetActive(false);        
    }

}
