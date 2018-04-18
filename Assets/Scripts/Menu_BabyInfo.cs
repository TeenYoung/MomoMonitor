using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class Menu_BabyInfo : MonoBehaviour {

    public GameObject Panel_BabyInfoInitialization,
        Panel_BabyInfo;
    public Text Text_Name, Text_BirthNum, Text_GenderInfo, Text_AgeNum, Text_Weight, Text_Height;
    public Button Button_ResetBabyInfo;
    public DateTime DateTime_babyBirth;
    public Sprite boyPortrait, girlPortrait;

    private string babyName, babyBirth, babyGender;
    private TimeSpan TimeSpan_babyAge;
    //private Text Text_BirthHour, Text_BirthMinute, Text_Birth,
    //   Text_BirthDay, Text_BirthMonth, Text_BirthYear;


    //Use this for initialization
    void Start() {
        if (PlayerPrefs.HasKey("babyName"))
        {
            ShowPanel_BabyInfo();
        }
        else ShowPanel_BabyInfoInitialization();
    }

    // Update is called once per frame
    void Update() {
    }

    public void DeliverOnClick() {
        ShowPanel_BabyInfo();
    }

    public void ShowPanel_BabyInfo()
    {
        Text_Name.text = PlayerPrefs.GetString("babyName");        
                
        //read birth saving
        babyBirth = PlayerPrefs.GetString("babyBirth");
        //print(babyBirth);
        
        //conver birthday formate from string to datetime
        DateTime_babyBirth = DateTime.ParseExact(babyBirth, 
            "ddMMyyyy HHmm",
            CultureInfo.InvariantCulture, DateTimeStyles.None);
        //print(DateTime_babyBirth.ToString());

        //output birth in format "dd/MM/yyyy HH:mm", and MM show in character
        Text_BirthNum.text = DateTime_babyBirth.ToString("dd ")
            + DateTime_babyBirth.ToString("MMM", new CultureInfo("en-us")).Substring(0, 3)
            + DateTime_babyBirth.ToString(" yyyy ")
            + DateTime_babyBirth.ToString("HH:mm");

        //count and output baby's age
        TimeSpan_babyAge = DateTime.Now.Subtract(DateTime_babyBirth);
        //print(TimeSpan_babyAge.Days.ToString());
        Text_AgeNum.text = TimeSpan_babyAge.Days.ToString() + " days";
        
        PlayerPrefs.SetString("babyAge", TimeSpan_babyAge.Days.ToString());

        string babyGender = PlayerPrefs.GetString("babyGender");
        Text_GenderInfo.text = babyGender;
        if(babyGender == "boy")
        Panel_BabyInfo.transform.Find("Image").GetComponent<Image>().sprite = boyPortrait;
        else Panel_BabyInfo.transform.Find("Image").GetComponent<Image>().sprite = girlPortrait;



        Panel_BabyInfo.gameObject.SetActive(true);
        Panel_BabyInfoInitialization.gameObject.SetActive(false);

        //load weight and height saving
        Text_Weight.text = PlayerPrefs.GetString("babyWeight");
        Text_Height.text = PlayerPrefs.GetString("babyHeight");
    }

    public void ShowPanel_BabyInfoInitialization()
    {
        Panel_BabyInfo.gameObject.SetActive(false);
        Panel_BabyInfoInitialization.gameObject.SetActive(true);
    }

    //Reset saving
    public void Reset()
    {
        PlayerPrefs.DeleteKey("babyName");
        PlayerPrefs.DeleteKey("babyBirth");
        PlayerPrefs.DeleteKey("babyGender");
        PlayerPrefs.DeleteKey("babyWeight");
        PlayerPrefs.DeleteKey("babyHeight");
        ShowPanel_BabyInfoInitialization();
    }  
    
}
