using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

public class InputBloodVesselInfo : MonoBehaviour
{
    UIDocument StrainInfoDocument;
    GroupBox uiInfoSurgeon;
    public UnityEngine.UIElements.Label uiDistanceText;
    ProgressBar uiDistancePB;
    public GroupBox uiGroupBox;
    UnityEngine.UIElements.Button uiButton;
    UnityEngine.UIElements.Label uiLabel;
    public UnityEngine.UIElements.Label uiScanText;
    public UnityEngine.UIElements.Label uiScanWarning;
    public TextField uiPatientName;
    public FloatField uiLengthBV;
    public FloatField uiLocationVC;
    public DropdownField uiDropdownField;

    public UnityEngine.UI.Image progressBar;
    public UnityEngine.UI.Image progressBarBackground;

    void OnEnable()     //This void gets the UI document and all the different components of the UI for further referencing
    {
        StrainInfoDocument = GetComponent<UIDocument>();

        //Elements first UI window
        uiScanText = StrainInfoDocument.rootVisualElement.Q("ScanText") as UnityEngine.UIElements.Label;
        uiScanWarning = StrainInfoDocument.rootVisualElement.Q("ScanWarning") as UnityEngine.UIElements.Label;
        uiLengthBV = StrainInfoDocument.rootVisualElement.Q("LengthBloodVessel") as FloatField;
        uiLocationVC = StrainInfoDocument.rootVisualElement.Q("LocationVasoconstriction") as FloatField;
        uiDropdownField = StrainInfoDocument.rootVisualElement.Q("DropdownField") as DropdownField;
        uiPatientName = StrainInfoDocument.rootVisualElement.Q("PatientName") as TextField;
        //Elements second UI window
        uiInfoSurgeon = StrainInfoDocument.rootVisualElement.Q("InfoSurgeon") as GroupBox;
        uiButton = StrainInfoDocument.rootVisualElement.Q("NextButton") as UnityEngine.UIElements.Button;
        uiGroupBox = StrainInfoDocument.rootVisualElement.Q("InfoScan") as GroupBox;
        uiLabel = StrainInfoDocument.rootVisualElement.Q("RelativePressureLabel") as UnityEngine.UIElements.Label;
        uiDistanceText = StrainInfoDocument.rootVisualElement.Q("DistanceText") as UnityEngine.UIElements.Label;
        uiDistancePB = StrainInfoDocument.rootVisualElement.Q("DistanceProgressbar") as ProgressBar;

        //Checks if elements are present
        if (StrainInfoDocument == null)
        {
            Debug.LogError("No button document found"); //Checks if UI document is found
        }
        if (uiInfoSurgeon == null)
        {
            Debug.Log("Group box NOT found"); //Checks if group box is found
        }
        if (uiButton == null)
        {
            Debug.Log("Button NOT found"); //Checks if button is found
        }
        if (uiScanText == null)
        {
            Debug.Log("Scan text NOT found"); //Checks if scan text is found
        }
        if (uiScanWarning == null)
        {
            Debug.Log("Scan text NOT found"); //Checks if scan warning is found
        }
        if (uiGroupBox == null)
        {
            Debug.Log("Group box NOT found"); //Checks if group box is found
        }
        if (uiLengthBV == null)
        {
            Debug.Log("Length blood vessel NOT found"); //Checks if length blood vessel is found
        }
        if (uiLocationVC == null)
        {
            Debug.Log("Location vasoconstriction NOT found"); //Checks if location vasoconstriction is found
        }
        if (uiDropdownField == null)
        {
            Debug.Log("Dropdown menu NOT found"); //Checks if dropdown menu is found
        }

        uiButton.RegisterCallback<ClickEvent>(OnButtonClick);   //Runs the OnButtonClick function when the button is clicked
    }

    void Start()
    {
        StrainScript.uiInfoLabel.visible = false;
        uiInfoSurgeon.visible = false;                      //Hide second UI window with surgeon info
        progressBar.enabled = false;                        //Hide the self-made progressbar
        progressBarBackground.enabled = false;              //Hide background of self-made progressbar
        uiGroupBox.style.borderBottomWidth = 3;             //Layout of the first UI window
        uiGroupBox.style.borderBottomColor = Color.grey;
        uiGroupBox.style.borderRightWidth = 3;
        uiGroupBox.style.borderRightColor = Color.grey;
        uiGroupBox.style.borderTopWidth = 3;
        uiGroupBox.style.borderTopColor = Color.grey;
        uiGroupBox.style.borderLeftWidth = 3;
        uiGroupBox.style.borderLeftColor = Color.grey;
    }

    public string PatientName;      //Name of the patient
    public float BVlength;         //Total blood vessel length
    public float VClocation;       //Vasoconstriction location within blood vessel
    public string BVtype;          //Blood vessel type

    void Update()
    {
        PatientName = uiPatientName.value;
        BVlength = uiLengthBV.value;
        VClocation = uiLocationVC.value;
        BVtype = uiDropdownField.value;
        
        if (VClocation > BVlength)
        {
            uiScanWarning.text = "Location vasoconstriction outside of blood vessel.";
            uiScanWarning.style.color = Color.red;
        }
        if (VClocation <= BVlength & uiScanWarning.text == "Location vasoconstriction outside of blood vessel.")
        {
            uiScanWarning.text = "";
        }

        if (BVlength < 0 || VClocation < 0)
        {
            uiScanWarning.text = "Blood vessel length and location of vasoconstriction should both be positive numbers.";
        }
        if (BVlength >= 0 & VClocation >= 0 & uiScanWarning.text == "Blood vessel length and location of vasoconstriction should be positive numbers.")
        {
            uiScanWarning.text = "";
        }

        if (BVlength != 0 & VClocation != 0 & BVtype != null & BVlength >= VClocation & PatientName != "" & PatientName != "Type name" & BVlength >= 0 & VClocation >= 0)
        {
            uiButton.text = "Next!";
            uiButton.style.color = Color.black;
        }
    }

    public ArduinoInput ArduinoScript;
    public InputStrainInfo StrainScript;

    public void OnButtonClick(ClickEvent evt)
    {
        if (BVlength == 0 || VClocation == 0 || VClocation > BVlength || BVtype == null || PatientName == "" || PatientName == "Type name" || BVlength < 0 || VClocation < 0)
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
            StrainScript.uiBackButton.visible = true;

            //progressBar.transform.position = uiLabel.worldTransform.GetPosition() + new Vector3(-27, -84, 0);
            //progressBarBackground.transform.position = uiLabel.worldTransform.GetPosition() + new Vector3(-25, -88, 0);
            progressBar.enabled = true;
            progressBarBackground.enabled = true;

            StrainScript.uiInfoLabel.visible = true;
            StrainScript.uiInfoLabel.text = "Patient: " + PatientName.ToString() + "\r\n" +
                "Blood vessel type: " + BVtype.ToString() + "\r\n" +
                "Blood vessel length: " + BVlength.ToString() + " cm" + "\r\n" +
                "Distance to vasoconstriction: " + VClocation.ToString() + " cm";
        }
    }
}
