using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class Menu_BabyInfo : MonoBehaviour {

    public GameObject Panel_BabyInfoInitialization,Panel_BabyInfo;
    public Text Text_Name, Text_BirthNum, Text_GenderInfo, Text_Weight, Text_Height;
    public DateTime DateTime_babyBirth;
    public Sprite boyPortrait, girlPortrait;
    public GameObject buttonAge;
    public InputField inputFieldGrowth;
    public GameObject buttonHeight, buttonWeight;

    public GameObject buttonSetting, panelSetting, panelSettingCheck;

    private GameObject buttonGrowth;
    private string babyBirth;
    private TimeSpan TimeSpan_babyAge;
    private string status;

    private List<Log> weightList = new List<Log>();
    private List<Log> heightList = new List<Log>();

    //Use this for initialization
    void Start() {
        if (PlayerPrefs.HasKey("babyName"))
        {
            ShowPanel_BabyInfo();
        }
        else ShowPanel_BabyInfoInitialization();
    }

    private void OnEnable()
    {
        weightList = Main_Menu.menu.weightList;
        heightList = Main_Menu.menu.heightList;
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelSettingCheck.gameObject.activeInHierarchy)
            {
                panelSettingCheck.SetActive(false);
                panelSetting.SetActive(false);
            }
            if(panelSetting.gameObject.activeInHierarchy) panelSetting.SetActive(false);
            else if(!panelSettingCheck.gameObject.activeInHierarchy & !panelSetting.gameObject.activeInHierarchy)Application.Quit();
        } 
    }

    public void DeliverOnClick() {
        ShowPanel_BabyInfo();
    }

    public void ShowPanel_BabyInfo()
    {
        Text_Name.text = PlayerPrefs.GetString("babyName");        
                
        //read birth saving
        babyBirth = PlayerPrefs.GetString("babyBirth");
        
        //conver birthday formate from string to datetime
        DateTime_babyBirth = DateTime.ParseExact(babyBirth, 
            "ddMMyyyy HHmm",
            CultureInfo.InvariantCulture, DateTimeStyles.None);

        //output birth in format "dd/MM/yyyy HH:mm", and MM show in character
        Text_BirthNum.text = DateTime_babyBirth.ToString("dd ")
            + DateTime_babyBirth.ToString("MMM", new CultureInfo("en-us")).Substring(0, 3)
            + DateTime_babyBirth.ToString(" yyyy ") + "\n"
            + DateTime_babyBirth.ToString("HH:mm");

        //count and output baby's age
        TimeSpan_babyAge = DateTime.Now.Subtract(DateTime_babyBirth);
        //age less than 1 year,default show in days
        if (DateTime_babyBirth.AddYears(1) > DateTime.Now)
        {            
            buttonAge.GetComponentInChildren<Text>().text = TimeSpan_babyAge.Days.ToString() + " days";
            status = "days";
        }

        //age more than 1 year,default show in year
        if (DateTime_babyBirth.AddYears(1) < DateTime.Now)
        {
            buttonAge.GetComponentInChildren<Text>().text
                 = ((DateTime.Now.Year - DateTime_babyBirth.Year) * 12 + (DateTime.Now.Month - DateTime_babyBirth.Month)) / 12 + " Y " +
                 ((DateTime.Now.Year - DateTime_babyBirth.Year) * 12 + (DateTime.Now.Month - DateTime_babyBirth.Month)) % 12 + " M";
            status = "MY";
        }        

        PlayerPrefs.SetString("babyAge", TimeSpan_babyAge.Days.ToString());

        string babyGender = PlayerPrefs.GetString("babyGender");
        Text_GenderInfo.text = babyGender;
        if (babyGender == "boy")
        {
            Panel_BabyInfo.transform.Find("Image").GetComponent<Image>().sprite = boyPortrait;
            //Panel_BabyInfo.GetComponent<Image>().color = new Color(0.141f,0.349f,0.537f,1);
        }
        
        else Panel_BabyInfo.transform.Find("Image").GetComponent<Image>().sprite = girlPortrait;

        Panel_BabyInfo.gameObject.SetActive(true);
        Panel_BabyInfoInitialization.gameObject.SetActive(false);

        //load weight and height saving
        Text_Weight.text = weightList[weightList.Count - 1].Detail;
        Text_Height.text = heightList[heightList.Count - 1].Detail;
    }

    public void ShowPanel_BabyInfoInitialization()
    {
        Panel_BabyInfo.gameObject.SetActive(false);
        Panel_BabyInfoInitialization.gameObject.SetActive(true);
    }

    public void AddGrowth(GameObject gobButton)
    {   
        inputFieldGrowth.gameObject.SetActive(true);
        inputFieldGrowth.Select();
        buttonGrowth = gobButton;
        if(buttonGrowth == buttonWeight)inputFieldGrowth.placeholder.GetComponent<Text>().text = "Update weight ( Unit: kg):";
        if(buttonGrowth == buttonHeight) inputFieldGrowth.placeholder.GetComponent<Text>().text = "Update height ( Unit: cm):";
    } 

    public void SaveGrowth()
    {
        Log logTemp = new Log();
        if (buttonGrowth == buttonWeight)
        {
            logTemp.Type = "weight";
            Text_Weight.text = inputFieldGrowth.text;
            //PlayerPrefs.SetString("babyWeight", Text_Weight.text);
        }
        if (buttonGrowth == buttonHeight)
        {
            logTemp.Type = "height";
            Text_Height.text = inputFieldGrowth.text;
            //PlayerPrefs.SetString("babyHeight", Text_Height.text);
        }
        logTemp.Date = DateTime.Today;
        logTemp.Detail = inputFieldGrowth.text;
        Main_Menu.menu.LogsAdd(logTemp);
        Main_Menu.menu.LogSave();
        inputFieldGrowth.gameObject.SetActive(false);
        inputFieldGrowth.text = "";
    }

    public void CancelGrowth()
    {
        inputFieldGrowth.gameObject.SetActive(false);
        inputFieldGrowth.text = "";
    }

    public void ChangeAgeUnit()
    {
        if (status == "days")// && TimeSpan_babyAge.Days >=7) //show Days and Weeks
        {
            buttonAge.GetComponentInChildren<Text>().text = TimeSpan_babyAge.Days / 7 + " W " + TimeSpan_babyAge.Days % 7 + " D";
            if (TimeSpan_babyAge.Days % 7 == 0) buttonAge.GetComponentInChildren<Text>().text = TimeSpan_babyAge.Days / 7 + " W"; // if day=0, only show week
            status = "DW";
        }
        else if(status == "DW")//show Weeks and Month
        {
            int monthTemp = (DateTime.Now.Year - DateTime_babyBirth.Year) *12 + DateTime.Now.Month - DateTime_babyBirth.Month;
            buttonAge.GetComponentInChildren<Text>().text = monthTemp + " M " +
               DateTime.Now.Subtract(DateTime_babyBirth.AddMonths(monthTemp)).Days/7 + " W";
            if (DateTime.Now.Subtract(DateTime_babyBirth.AddMonths(monthTemp)).Days / 7 == 0)// if week = 0, only show month
                buttonAge.GetComponentInChildren<Text>().text = monthTemp + " M";
            status = "WM";
        }
        else if (status == "WM")//show Months and Years 
        {
            buttonAge.GetComponentInChildren<Text>().text = ((DateTime.Now.Year - DateTime_babyBirth.Year) * 12 + (DateTime.Now.Month - DateTime_babyBirth.Month)) / 12 + " Y " +
                ((DateTime.Now.Year - DateTime_babyBirth.Year) * 12 + (DateTime.Now.Month - DateTime_babyBirth.Month)) % 12 + " M";
            if (((DateTime.Now.Year - DateTime_babyBirth.Year) * 12 + (DateTime.Now.Month - DateTime_babyBirth.Month)) % 12 == 0) // if month=0, only show year
                buttonAge.GetComponentInChildren<Text>().text = ((DateTime.Now.Year - DateTime_babyBirth.Year) * 12 + (DateTime.Now.Month - DateTime_babyBirth.Month)) / 12 + " Y";
            status = "MY";
        }
        else if (status == "MY")//show Days
        {
            buttonAge.GetComponentInChildren<Text>().text = TimeSpan_babyAge.Days.ToString() + " days";
            status = "days";
        }
    }

    public void OpenSettingPanel()
    {
        panelSetting.SetActive(true);
    }

    public void OpenPanelSettingCheck()
    {
        panelSettingCheck.SetActive(true);
    }

    public void ResetCancel()
    {
        panelSetting.SetActive(false);
        panelSettingCheck.SetActive(false);
    }

    public void ResetConfirm()
    {
        panelSetting.SetActive(false);
        panelSettingCheck.SetActive(false);
        PlayerPrefs.DeleteAll();
        Start();
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
