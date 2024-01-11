using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InputBloodVesselInfo : MonoBehaviour
{
    UIDocument StrainInfoDocument;
    GroupBox uiGroupBox;
    UnityEngine.UIElements.Button uiButton;
    Label uiScanText;
    FloatField uiLengthBV;
    FloatField uiLocationVC;
    DropdownField uiDropdownField;

    public UnityEngine.UI.Image progressBar;
    public UnityEngine.UI.Image progressBarBackground;

    void OnEnable()     //This void gets the UI document and all the different components of the UI for further referencing
    {
        StrainInfoDocument = GetComponent<UIDocument>();

        if (StrainInfoDocument == null)
        {
            Debug.LogError("No button document found"); //Checks if UI document is found
        }

        uiButton = StrainInfoDocument.rootVisualElement.Q("TestButton") as UnityEngine.UIElements.Button;

        if (uiButton == null)
        {
            Debug.Log("Button NOT found"); //Checks if button is found
        }

        uiScanText = StrainInfoDocument.rootVisualElement.Q("ScanText") as Label;

        if (uiScanText == null)
        {
            Debug.Log("Scan text NOT found"); //Checks if scan text is found
        }

        uiGroupBox = StrainInfoDocument.rootVisualElement.Q("InfoScan") as GroupBox;

        if (uiGroupBox == null)
        {
            Debug.Log("Group box NOT found"); //Checks if group box is found
        }

        uiLengthBV = StrainInfoDocument.rootVisualElement.Q("LengthBloodVessel") as FloatField;

        if (uiLengthBV == null)
        {
            Debug.Log("Length blood vessel NOT found"); //Checks if length blood vessel is found
        }

        uiLocationVC = StrainInfoDocument.rootVisualElement.Q("LocationVasoconstriction") as FloatField;

        if (uiLocationVC == null)
        {
            Debug.Log("Location vasoconstriction NOT found"); //Checks if location vasoconstriction is found
        }

        uiDropdownField = StrainInfoDocument.rootVisualElement.Q("DropdownField") as DropdownField;

        if (uiDropdownField == null)
        {
            Debug.Log("Dropdown menu NOT found"); //Checks if dropdown menu is found
        }

        //uiButton.RegisterCallback<ClickEvent>(OnButtonClick);
    }



    // Start is called before the first frame update
    void Start()
    {
        uiButton.visible = false;
        progressBar.enabled = false;
        progressBarBackground.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (uiLocationVC.value > uiLengthBV.value)
        {
            uiScanText.text = "Location vasoconstriction outside of blood vessel";
        }

        if (uiLocationVC.value <= uiLengthBV.value)
        {
            uiScanText.text = "";
        }

        if (uiLocationVC.value <= uiLengthBV.value & uiDropdownField.value != null & uiLengthBV.value != 0 & uiLocationVC.value != 0)
        {
            uiButton.visible = true;
        }

        if (uiButton.text == "Running...")
        {
            OnButtonClick();
        }
    }

    public void OnButtonClick()
    {
        uiDropdownField.visible = false;
        uiLengthBV.visible = false;
        uiLocationVC.visible = false;
        progressBar.enabled = true;
        progressBarBackground.enabled = true;
    }
}
