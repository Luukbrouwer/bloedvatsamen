using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Reflection.Emit;

public class InputBloodVesselInfo : MonoBehaviour
{
    UIDocument StrainInfoDocument;
    UnityEngine.UIElements.Label uiDistanceText;
    ProgressBar uiDistancePB;
    GroupBox uiGroupBox;
    UnityEngine.UIElements.Button uiButton;
    UnityEngine.UIElements.Label uiLabel;
    UnityEngine.UIElements.Label uiScanText;
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

        uiScanText = StrainInfoDocument.rootVisualElement.Q("ScanText") as UnityEngine.UIElements.Label;

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

        uiLabel = StrainInfoDocument.rootVisualElement.Q("RelativePressureLabel") as UnityEngine.UIElements.Label;
        uiDistanceText = StrainInfoDocument.rootVisualElement.Q("DistanceText") as UnityEngine.UIElements.Label;
        uiDistancePB = StrainInfoDocument.rootVisualElement.Q("DistanceProgressbar") as ProgressBar;

        //uiButton.RegisterCallback<ClickEvent>(OnButtonClick);
    }



    // Start is called before the first frame update
    void Start()
    {
        uiLabel.visible = false;
        uiButton.visible = false;
        progressBar.enabled = false;
        progressBarBackground.enabled = false;
        uiDistanceText.visible = false;
        uiDistancePB.visible = false;
    }

    private float BVlength;
    private float VClocation;
    private string BVtype;
    private float GWlength;
    private bool infoReady = false;

    // Update is called once per frame
    void Update()
    {
        BVlength = uiLengthBV.value;
        VClocation = uiLocationVC.value;
        BVtype = uiDropdownField.value;
        
        if (VClocation > BVlength || VClocation == 0)
        {
            uiScanText.text = "Location vasoconstriction outside of blood vessel";
            uiButton.visible = false;
        }

        if (VClocation <= BVlength)
        {
            uiScanText.text = "";
        }

        if (VClocation <= BVlength & BVtype != null & BVlength != 0 & VClocation != 0)
        {
            uiButton.visible = true;
        }

        if (uiButton.text == "Running..." & infoReady == false)
        {
            OnButtonClick();
            DistanceProgressBar();
            infoReady = true;
        }
    }

    public void OnButtonClick()
    {
        uiGroupBox.visible = false;
        uiLabel.visible = true;
        progressBar.transform.position = uiLabel.worldTransform.GetPosition() + new Vector3(-27, -84, 0);
        progressBarBackground.transform.position = uiLabel.worldTransform.GetPosition() + new Vector3(-25, -88, 0);
        progressBar.enabled = true;
        progressBarBackground.enabled = true;
    }

    public void DistanceProgressBar()
    {
        uiDistanceText.visible = true;
        uiDistancePB.visible = true;
        uiDistancePB.highValue = VClocation;

        //uiDistancePB.value = GWlength;
    }
}
