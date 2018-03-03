using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Nappy_Button : MonoBehaviour {

    public GameObject panel_Input;
    public Text text_wetNum, text_pooNum;

    private int wetNum, pooNum;

    // Use this for initialization
    void Start() {

        //load saved data
        wetNum = PlayerPrefs.GetInt("wet: ");
        text_wetNum.text = "wet: " + wetNum;

        pooNum = PlayerPrefs.GetInt("poo: ");
        text_pooNum.text = "poo: " + pooNum;
        
    }

    public void WetButtonOnClick() {
        wetNum += 1;

        //save data
        PlayerPrefs.SetInt("wet: ", wetNum);

        text_wetNum.text = "wet: " + wetNum;
        CloseInputPanel();
    }

    public void PooButtonOnClick()
    {
        pooNum += 1;

        //save data
        PlayerPrefs.SetInt("poo: ", pooNum);

        text_pooNum.text = "poo: " + pooNum;

        CloseInputPanel();
    }

    public void BothButtonOnClick()
    {
        wetNum += 1;
        //save data
        PlayerPrefs.SetInt("wet: ", wetNum);

        text_wetNum.text = "wet: " + wetNum;

        pooNum += 1;

        //save data
        PlayerPrefs.SetInt("poo: ", pooNum);

        text_pooNum.text = "poo: " + pooNum;

        CloseInputPanel();
    }

    public void OpenInputPanel() {
        panel_Input.SetActive(true);
    }

    public void CloseInputPanel() {
        panel_Input.SetActive(false);
    }

    public void RecordOnClick()
    {

    }
}
