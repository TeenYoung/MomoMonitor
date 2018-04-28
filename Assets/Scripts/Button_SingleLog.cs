using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_SingleLog : MonoBehaviour {

    public GameObject panelDeleteCheck, panelLogs;
    public Text textSingleLog;
    public int logIndex;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void GetPanelDeleteCheck(GameObject gobPanel,GameObject gobPanelLog, int index)
    {
        panelDeleteCheck = gobPanel;
        panelLogs = gobPanelLog;
        logIndex = index;
    }

    public void OpenDeleteCheckPanel()
    {
        panelLogs.GetComponent<Panel_Logs>().GetDeleteLogIndex(logIndex);
        panelDeleteCheck.SetActive(true);
        print(logIndex);
    }
}
