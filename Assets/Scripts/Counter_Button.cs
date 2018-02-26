using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Counter_Button : MonoBehaviour {

    public Text titleText;
    public Text totalNumText;
    public InputField inputNumField;

    public string title;
    public string unit;

    private int number, totalNum;     
    
    // Use this for initialization
    void Start() {
        titleText.text = title;

        //load saved data
        if (PlayerPrefs.HasKey(title))
            totalNum = PlayerPrefs.GetInt(title);

        totalNumText.text = totalNum + " " + unit;
    }

    // Update is called once per frame
    void Update() {

    }

    public void EnterToInput()
    {
        inputNumField.gameObject.SetActive(true);
        inputNumField.Select();
    }

    public void EnterToShowTotal()
    {
        inputNumField.gameObject.SetActive(false);
        //number = Convert.ToInt32(singleNumText.text);
        number = Convert.ToInt32(inputNumField.text);
        totalNum += number;
        totalNumText.text = totalNum + " " + unit;

        //save data
        PlayerPrefs.SetInt(title, totalNum);

        // ??find a way to rest to default 
        inputNumField.text = "";
        
    }

    public void EnterToCancel() {
        inputNumField.gameObject.SetActive(false);

        // ??find a way to rest to default 
        inputNumField.text = "";
    }
    
}

