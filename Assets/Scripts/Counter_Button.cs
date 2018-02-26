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
        //inputNumField.gameObject.SetActive(false);
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

        // ??find a way to rest to default 
        inputNumField.text = "";
        
    }

    public void EnterToCancel() {
        inputNumField.gameObject.SetActive(false);

        // ??find a way to rest to default 
        inputNumField.text = "";
    }
    
}

