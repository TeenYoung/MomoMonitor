using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDay : MonoBehaviour {

public void OnClick()
    {
        PlayerPrefs.DeleteAll();
    }

}
