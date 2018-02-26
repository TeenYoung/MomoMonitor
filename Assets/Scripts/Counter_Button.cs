using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Counter_Button : MonoBehaviour {

    public Button inputEnter;
    public Text totalNumText, singleNumText;
    public InputField inputNumField;

    public string unit;

    private int number, totalNum; 
    
    // Use this for initialization
    void Start() {
        inputEnter.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void EnterToInput()
    {
        inputEnter.gameObject.SetActive(true);
    }

    public void EnterToShowTotal()
    {
        inputEnter.gameObject.SetActive(false);
        //number = Convert.ToInt32(singleNumText.text);
        number = Convert.ToInt32(inputNumField.text);
        totalNum += number;
        totalNumText.text = totalNum + " " + unit;

        // ??find a way to rest to default 
        inputNumField.text = "";
        
    }
    
}

