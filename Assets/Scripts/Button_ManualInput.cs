using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ManualInput : MonoBehaviour {

    public GameObject panel_Input;
    public Button button;

    private Panel_Input pI;

    public void OnClick()
    {
        panel_Input.SetActive(true);

        pI = panel_Input.GetComponent<Panel_Input>();

        pI.sourceButton = button;

        pI.inputField_1.gameObject.SetActive(true);
        pI.inputField_1.Select();

        SetManualInputPanel(button.gameObject.GetComponent<Button_Entry>().buttonType);
    }

    void SetManualInputPanel(int buttonType)
    {
        switch (buttonType)
        {
            case 0:
                {
                    pI.text_Title_1.text = "Input start time";
                    pI.text_Title_2.text = "Input end time";
                    pI.text_Placeholder_1.text = "hhmm e.g. 1800 for 6pm";
                    pI.text_Placeholder_2.text = "hhmm e.g. 1900 for 7pm";

                    pI.inputField_1.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().characterLimit = 4;
                }
                break;

            case 1:
                {
                    pI.manualInputDateTime = true;

                    pI.text_Title_1.text = "Input end time";
                    pI.text_Title_2.text = "Input number";
                    pI.text_Placeholder_1.text = "hhmm e.g. 1800 for 6pm";
                    pI.text_Placeholder_2.text = "unit: " + button.GetComponent<Button_Entry>().unit;

                    pI.inputField_1.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().characterLimit = 4;
                }
                break;
        }
    }
}
