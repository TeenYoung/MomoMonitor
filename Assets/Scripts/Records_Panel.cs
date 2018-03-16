using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Records_Panel : MonoBehaviour {

    public List<Text> contents;
    //public Button buttonAddRecord;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseRecord();

    }

    public void AddRecord()
    {
        
    }

    public void CloseRecord()
    {
        gameObject.SetActive(false);
    }
}
