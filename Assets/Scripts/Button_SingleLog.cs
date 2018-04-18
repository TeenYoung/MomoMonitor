using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_SingleLog : MonoBehaviour {

    public GameObject panelDeleteCheck, panelLogs;
    public Text textSingleLog;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape)) CloseDeleteCheckPanel();
    }

    public void GetPanelDeleteCheck(GameObject gobPanel,GameObject gobPanelLog)
    {
        panelDeleteCheck = gobPanel;
        panelLogs = gobPanelLog;
    }

    public void OpenDeleteCheckPanel()
    {
        panelDeleteCheck.SetActive(true);
        panelLogs.GetComponent<Panel_Logs>().GetDeleteLogDetail(textSingleLog.text);
    }

    //public void CloseDeleteCheckPanel()
    //{
    //    panelDeleteCheck.SetActive(false);
    //}
}
