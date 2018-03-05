using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Button : MonoBehaviour
{

    public Button NewDay_Button, Reset_Button;
    public GameObject Setting_Panel;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))Cancel();
    }

    public void SettingOnClick()
    {
        Setting_Panel.gameObject.SetActive(true);
    }

    public void Cancel()
    {
        Setting_Panel.gameObject.SetActive(false);       
    }

}
