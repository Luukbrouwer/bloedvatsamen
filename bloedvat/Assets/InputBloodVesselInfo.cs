using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

public class InputBloodVesselInfo : MonoBehaviour
{
    UIDocument StrainInfoDocument;
    GroupBox uiInfoSurgeon;
    UnityEngine.UIElements.Label uiDistanceText;
    ProgressBar uiDistancePB;
    GroupBox uiGroupBox;
    UnityEngine.UIElements.Button uiButton;
    UnityEngine.UIElements.Label uiLabel;
    public UnityEngine.UIElements.Label uiScanText;
    public UnityEngine.UIElements.Label uiScanWarning;
    public FloatField uiLengthBV;
    public FloatField uiLocationVC;
    public DropdownField uiDropdownField;

    public UnityEngine.UI.Image progressBar;
    public UnityEngine.UI.Image progressBarBackground;

    void OnEnable()     //This void gets the UI document and all the different components of the UI for further referencing
    {
        StrainInfoDocument = GetComponent<UIDocument>();

        if (StrainInfoDocument == null)
        {
            Debug.LogError("No button document found"); //Checks if UI document is found
        }

        uiInfoSurgeon = StrainInfoDocument.rootVisualElement.Q("InfoSurgeon") as GroupBox;

        if (uiInfoSurgeon == null)
        {
            Debug.Log("Group box NOT found"); //Checks if group box is found
        }

        uiButton = StrainInfoDocument.rootVisualElement.Q("NextButton") as UnityEngine.UIElements.Button;

        if (uiButton == null)
        {
            Debug.Log("Button NOT found"); //Checks if button is found
        }

        uiScanText = StrainInfoDocument.rootVisualElement.Q("ScanText") as UnityEngine.UIElements.Label;

        if (uiScanText == null)
        {
            Debug.Log("Scan text NOT found"); //Checks if scan text is found
        }

        uiScanWarning = StrainInfoDocument.rootVisualElement.Q("ScanWarning") as UnityEngine.UIElements.Label;

        if (uiScanWarning == null)
        {
            Debug.Log("Scan text NOT found"); //Checks if scan warning is found
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

        uiButton.RegisterCallback<ClickEvent>(OnButtonClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        uiInfoSurgeon.visible = false;
        progressBar.enabled = false;
        progressBarBackground.enabled = false;
    }

    public float BVlength;         //Total blood vessel length
    public float VClocation;       //Vasoconstriction location within blood vessel
    public string BVtype;          //Blood vessel type

    // Update is called once per frame
    void Update()
    {
        BVlength = uiLengthBV.value;
        VClocation = uiLocationVC.value;
        BVtype = uiDropdownField.value;
        
        if (VClocation > BVlength)
        {
            uiScanWarning.text = "Location vasoconstriction outside of blood vessel";
            uiScanWarning.style.color = Color.red;
        }

        if (VClocation <= BVlength & uiScanWarning.text == "Location vasoconstriction outside of blood vessel")
        {
            uiScanWarning.text = "";
        }

        if (BVlength != 0 & VClocation != 0 & BVtype != null & BVlength >= VClocation)
        {
            uiButton.text = "Next!";
            uiButton.style.color = Color.black;
        }
    }

    public ArduinoInput ArduinoScript;
    public InputStrainInfo StrainScript;

    public void OnButtonClick(ClickEvent evt)
    {
        if (BVlength == 0 || VClocation == 0 || VClocation > BVlength || BVtype == null)
        {
/*            uiScanWarning.text = "Not all fields have been corretly filled in";
            uiScanWarning.style.color = Color.red;*/
            uiButton.text = "Not all fields have been correctly filled in.";
            uiButton.style.color = Color.red;
        }

        else
        {
            uiGroupBox.visible = false;
            StrainScript.uiGroupBox.visible = true;

            //progressBar.transform.position = uiLabel.worldTransform.GetPosition() + new Vector3(-27, -84, 0);
            //progressBarBackground.transform.position = uiLabel.worldTransform.GetPosition() + new Vector3(-25, -88, 0);
            progressBar.enabled = true;
            progressBarBackground.enabled = true;
        }
    }
}
