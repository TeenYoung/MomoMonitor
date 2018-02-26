using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Nappy_Button : MonoBehaviour {

    public Button Wet_Button, Poo_Button, BothWetAndPoo_Button;
    public Text Text_wetNum, Text_pooNum;

    private int wetNum, pooNum;

    // Use this for initialization
    void Start() {

        //load saved data
        wetNum = PlayerPrefs.GetInt("wet: ");
        Text_wetNum.text = "wet: " + wetNum;

        pooNum = PlayerPrefs.GetInt("poo: ");
        Text_pooNum.text = "poo: " + pooNum;
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void NappyButtonOnClick() {
        OpenNappyTypeButton();
    }

    public void WetButtonOnClick() {
        wetNum += 1;

        //save data
        PlayerPrefs.SetInt("wet: ", wetNum);

        Text_wetNum.text = "wet: " + wetNum;
        CloseNappyTypeButton();
    }

    public void PooButtonOnClick()
    {
        pooNum += 1;

        //save data
        PlayerPrefs.SetInt("poo: ", pooNum);

        Text_pooNum.text = "poo: " + pooNum;

        CloseNappyTypeButton();
    }

    public void BothButtonOnClick()
    {
        wetNum += 1;
        //save data
        PlayerPrefs.SetInt("wet: ", wetNum);

        Text_wetNum.text = "wet: " + wetNum;

        pooNum += 1;

        //save data
        PlayerPrefs.SetInt("poo: ", pooNum);

        Text_pooNum.text = "poo: " + pooNum;

        CloseNappyTypeButton();
    }

    public void OpenNappyTypeButton() {
        Wet_Button.gameObject.SetActive(true);
        Poo_Button.gameObject.SetActive(true);
        BothWetAndPoo_Button.gameObject.SetActive(true);
    }

    public void CloseNappyTypeButton() {
        Wet_Button.gameObject.SetActive(false);
        Poo_Button.gameObject.SetActive(false);
        BothWetAndPoo_Button.gameObject.SetActive(false);
    }
}
