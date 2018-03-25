using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Record : MonoBehaviour {

    public GameObject recordsPanel, sourceButton;
    public Text text_Title, text_Property;
    
    public Button buttonManualInput;

    public GameObject scrollView;

    //public GameObject contents;

    private Button_Entry button_Entry;

    private List<Entry> sourceList;
    private string records;


    private void Start()
    {
        button_Entry = sourceButton.GetComponent<Button_Entry>();
        text_Title.text = button_Entry.text_Title.text;
        text_Property.text = button_Entry.text_Property.text;
    }


    private void Update()
    {
        //update status at title, update last time at property
        text_Title.text = button_Entry.text_Title.text;
        text_Property.text = button_Entry.text_Property.text;      

        buttonManualInput.enabled = true;
    }
    
    //to move out into a new .cs file
    public void OnClick()
    {
        sourceList = Main_Menu.menu.entryLists[sourceButton.name];
        recordsPanel.name = button_Entry.name;
        buttonManualInput.gameObject.GetComponent<Image>().color = gameObject.GetComponent<Image>().color; //使打開manual Input button與本button同色

        sourceList = new List<Entry>();
        
        sourceList = Main_Menu.menu.entryLists[sourceButton.name];
        recordsPanel.name = button_Entry.name;

        //如果recordsPanel未打開則打開，且把打開panel的buttontype,name, unit傳給records_Panel,同時顯示mannualInput
        if (!recordsPanel.activeInHierarchy)
        {
            recordsPanel.gameObject.GetComponent<Records_Panel>().sourceButton = sourceButton; //把source button傳給records panel
            recordsPanel.gameObject.GetComponent<Records_Panel>().buttonType = button_Entry.buttonType; //為recordsPanel 傳入打開其面板的button type
            recordsPanel.gameObject.GetComponent<Records_Panel>().sourceButtonUnit = button_Entry.gameObject.GetComponent<Button_Entry>().unit; //為recordsPanel 傳入打開其面板的button 的unit

            buttonManualInput.gameObject.GetComponent<Button_ManualInput>().sourceButton = button_Entry.gameObject; //為button manualinput 傳入打開其面板的button name
            buttonManualInput.gameObject.GetComponent<Button_ManualInput>().sourceButtonType = button_Entry.buttonType; //為button manualinput 傳入打開其面板的button type
            buttonManualInput.gameObject.GetComponent<Button_ManualInput>().sourceButtonUnit = button_Entry.gameObject.GetComponent<Button_Entry>().unit;
            recordsPanel.SetActive(true);
        }
            
        //如果recordsPanel打開著，再次點擊關閉recordsPanel，同時關閉buttonManualInput
        else if (recordsPanel.activeInHierarchy) recordsPanel.GetComponent<Records_Panel>().CloseRecord();  
    }       
}
