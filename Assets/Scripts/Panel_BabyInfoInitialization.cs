using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Panel_BabyInfoInitialization : MonoBehaviour {

    public InputField InputField_Name, InputField_Birth;
    public Text Text_Name, Text_Birth;
    public Toggle Toggle_Gender;

    private string genderInfo;    

    // Use this for initialization
    void Start () {
        InputField_Name.Select();
        Toggle_Gender.isOn = false;
    }
	
	// Update is called once per frame
	void Update () {
    }
    

    // save info 
    public void SaveInfo() {
        
        //get gender
        if (Toggle_Gender.isOn) genderInfo = "boy";
        else genderInfo = "girl";

        PlayerPrefs.SetString("babyName", Text_Name.text);
        PlayerPrefs.SetString("babyBirth", Text_Birth.text.ToString());
        //print(Text_Birth.text);
        PlayerPrefs.SetString("babyGender", genderInfo);

        InputField_Name.Select();
        InputField_Name.text = "";
        InputField_Birth.text = "";
        Toggle_Gender.isOn = false;
    }


}
