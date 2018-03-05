using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Nappy_Button : MonoBehaviour {

    public GameObject panel_Input;
    public Text text_wetNum, text_pooNum, recordsText;
    public GameObject recordsPanel;

    private int weesNum, poosNum;
    private Nappy nappy;

    // Use this for initialization
    void Start() {

        //load saved data
        weesNum = PlayerPrefs.GetInt("wee: ");
        text_wetNum.text = "wee: " + weesNum;

        poosNum = PlayerPrefs.GetInt("poo: ");
        text_pooNum.text = "poo: " + poosNum;
        
    }

    // Update is called once per frame
    void Update()
    {
        //when press backward on keyboard, cancel input
        if (Input.GetKeyDown(KeyCode.Escape)) CloseInputPanel();
    }

    public void WeeButtonOnClick() {
        weesNum ++;

        //save data
        PlayerPrefs.SetInt("wee: ", weesNum);

        text_wetNum.text = "wee: " + weesNum;
        CloseInputPanel();

        nappy = new Nappy
        {
            Time = DateTime.Now,
            Wee = true,
            Poo = false
        };

        AddData();
        Main_Menu.menu.Save();
    }

    public void PooButtonOnClick()
    {
        poosNum ++;

        //save data
        PlayerPrefs.SetInt("poo: ", poosNum);

        text_pooNum.text = "poo: " + poosNum;

        CloseInputPanel();

        nappy = new Nappy
        {
            Time = DateTime.Now,
            Wee = false,
            Poo = true
        };

        AddData();
        Main_Menu.menu.Save();

    }

    public void BothButtonOnClick()
    {
        weesNum ++;
        //save data
        PlayerPrefs.SetInt("wee: ", weesNum);

        text_wetNum.text = "wee: " + weesNum;

        poosNum ++;

        //save data
        PlayerPrefs.SetInt("poo: ", poosNum);

        text_pooNum.text = "poo: " + poosNum;

        CloseInputPanel();

        nappy = new Nappy
        {
            Time = DateTime.Now,
            Wee = true,
            Poo = true
        };

        AddData();
        Main_Menu.menu.Save();

    }

    public void OpenInputPanel() {
        panel_Input.SetActive(true);
    }

    public void CloseInputPanel() {
        panel_Input.SetActive(false);
    }

    public void RecordOnClick()
    {
        string wee, poo;
        string records = "";

        foreach (Nappy nappy in Main_Menu.menu.nappyList)
        {
            if (nappy.Wee) wee = "Wee  ";
            else wee = "";

            if (nappy.Poo) poo = "Poo";
            else poo = "";

            string record = string.Format("{0}   {1}{2}\n",nappy.Time.ToShortTimeString(),wee,poo);

            records = records + record;
        }

        recordsPanel.SetActive(true);
        recordsText.text = records;
    }

    void AddData()
    {

        Main_Menu.menu.nappyList.Add(nappy);

    }

}

[Serializable]
public class Nappy
{

    public DateTime Time { get; set; }
    public bool Wee { get; set; }
    public bool Poo { get; set; }

}

