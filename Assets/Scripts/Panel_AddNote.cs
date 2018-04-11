using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Panel_AddNote : MonoBehaviour {

    public GameObject buttonAddNote, panelNoteEditor;
    public DateTime date;

    private void OnEnable()
    {
        panelNoteEditor.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {             
            if (gameObject.activeInHierarchy) gameObject.SetActive(false);
        }
    }
        
    public void ClickAddNote()
    {
        //gameObject.SetActive(false);
        panelNoteEditor.gameObject.SetActive(true);
        //GetNoteDate(date);
        print(date.Day+ "/" +date.Month + "/" + date.Year);
    }

    public void ClickClosePanelAddNote()
    {
        gameObject.SetActive(false);
    }

    public void GetNoteDate(DateTime dateOfNote)
    {
        date = dateOfNote;
    }
}
