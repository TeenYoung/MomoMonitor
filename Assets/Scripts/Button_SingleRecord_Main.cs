using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_SingleRecord_Main : MonoBehaviour {

    public GameObject panelRecordDeleteCheck, panelRecords;
    public Text textChosenRecordToDelete;
    private string deleteRecordText;
    private int recordIndex;

    // Use this for initialization
    void Start () {		
	}
	
	// Update is called once per frame
	void Update () {		
	}

    public void GetPanelRecordDeleteCheck(GameObject gobPanelRecordDeleteCheck, Text textObj, string text, Color color,GameObject gobRecordsPanel, int index)
    {
        panelRecordDeleteCheck = gobPanelRecordDeleteCheck;
        textChosenRecordToDelete = textObj;
        deleteRecordText = text;
        panelRecordDeleteCheck.GetComponent<Image>().color = color;
        recordIndex = index;
        panelRecords = gobRecordsPanel;        
    }

    public void OpenPanelRecordDeleteCheck()
    {
        panelRecordDeleteCheck.SetActive(true);        
        textChosenRecordToDelete.text = deleteRecordText;
        panelRecords.GetComponent<Records_Panel>().GetDeleteIndex(recordIndex);
    }
}
