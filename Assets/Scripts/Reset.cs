using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick() {
        PlayerPrefs.DeleteAll();
        File.Delete(Application.persistentDataPath + "/record.dat");
    }
}
