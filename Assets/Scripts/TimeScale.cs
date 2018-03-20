using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScale : MonoBehaviour {

    public GameObject Number_TimeScale;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 25; i++)
        {
            GameObject numberTS = Instantiate(Number_TimeScale, gameObject.transform);
            numberTS.GetComponent<RectTransform>().localPosition = new Vector3(0, i * -60.0F, 0);
            numberTS.GetComponent<Text>().text = i.ToString();

            if (i%3 == 0)
            {
                numberTS.GetComponent<Text>().fontSize = 30;
                numberTS.GetComponent<Text>().fontStyle = FontStyle.Bold;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
