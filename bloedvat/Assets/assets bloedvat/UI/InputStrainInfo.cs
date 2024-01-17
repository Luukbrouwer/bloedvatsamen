using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;



public class InputStrainInfo : MonoBehaviour
{
    /////////////////////////////////START READING EXCEL FILE//////////////////////////////////////////
    /*public TextAsset textAssetData;

    [System.Serializable]
    public class PositionCoordinates    //Creates an object containing the following data(types)
    {
        public int time;
        public int PosX;
        public int PosY;
        public int PosZ;
        public float curPressure;
        public float maxPressure;
    }
    [System.Serializable]

    public class TimePath   //Creates lists in which all the data from the Excel file can be stored
    {
        public PositionCoordinates[] positioncoordinates;
    }

    public TimePath myTimePath = new TimePath();    //Command to create the empty list which can store all the data from the Excel file

    private string[] AllData;     // Stores all the data
    private int duration;        // Gets the value of the amount of data entries (rows in Excel)

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);      //Creates a long string with all the data
        AllData = data;     //Assigns the string with all the data to an object that can be accessed outside of this Void()

        int tableSize = data.Length / 6 - 1;    //Determines total rows with data based on amount of columns (it excludes the first row with informaiton data)
        duration = tableSize;       //Assigns the amount of row with data to an objext that can be accessed outside of this void()
        myTimePath.positioncoordinates = new PositionCoordinates[tableSize];    // Creates an array with the length of the amount of data rows in Excel

        for (int i = 0; i < tableSize; i++)
        {
            myTimePath.positioncoordinates[i] = new PositionCoordinates     //Assigns associated values to the right time
            {
                time = int.Parse(data[6 * (i + 1)]),
                PosX = int.Parse(data[6 * (i + 1) + 1]),
                PosY = int.Parse(data[6 * (i + 1) + 2]),
                PosZ = int.Parse(data[6 * (i + 1) + 3]),
                curPressure = float.Parse(data[6 * (i + 1) + 4]),
                maxPressure = float.Parse(data[6 * (i + 1) + 5])
            };
        }
        return;
    }*/
    /////////////////////////////////DONE READING EXCEL FILE//////////////////////////////////////////

    UIDocument StrainInfoDocument;
    public GroupBox uiGroupBox;
    public Label uiInfoLabel;
    UnityEngine.UIElements.Button uiButton;
    FloatField uiFloFieldCur;
    FloatField uiFloFieldMax;
    Label uiLabel;
    ProgressBar uiDistancePB;
    Label uiScanWarning;
    public UnityEngine.UIElements.Button uiBackButton;

    public UnityEngine.UI.Image horzProgressBar;
    public UnityEngine.UI.Image[] healthBarPoints;

    void OnEnable()     //This void gets the UI document and all the different components of the UI for further referencing
    {
        StrainInfoDocument = GetComponent<UIDocument>();

        //Elements of the first UI window
        uiScanWarning = StrainInfoDocument.rootVisualElement.Q("ScanWarning") as Label;

        //Elements of the second UI window
        uiGroupBox = StrainInfoDocument.rootVisualElement.Q("InfoSurgeon") as GroupBox;
        uiInfoLabel = StrainInfoDocument.rootVisualElement.Q("PatientInfo") as Label;
        uiButton = StrainInfoDocument.rootVisualElement.Q("StartButton") as UnityEngine.UIElements.Button;
        uiFloFieldCur = StrainInfoDocument.rootVisualElement.Q("CurrentPressureValue") as FloatField;
        uiFloFieldMax = StrainInfoDocument.rootVisualElement.Q("MaximalPressureValue") as FloatField;
        uiLabel = StrainInfoDocument.rootVisualElement.Q("RelativePressureLabel") as Label;
        uiDistancePB = StrainInfoDocument.rootVisualElement.Q("DistanceProgressbar") as ProgressBar;
        uiBackButton = StrainInfoDocument.rootVisualElement.Q("BackButton") as UnityEngine.UIElements.Button;


        //Checks if elements of the UI are present
        if (StrainInfoDocument == null)
        {
            Debug.LogError("No button document found"); //Checks if UI document is found
        }
        if (uiGroupBox == null)
        {
            Debug.Log("Group box NOT found"); //Checks if group box is found
        }
        if (uiFloFieldCur == null)
        {
            Debug.Log("Current pressure float field NOT found"); //Checks if current pressure float field is found
        }
        if (uiFloFieldMax == null)
        {
            Debug.Log("Maximal pressure float field NOT found"); //Checks if maximal pressure float field is found
        }
        if (uiButton == null)
        {
            Debug.Log("Button NOT found"); //Checks if button is found
        }
        if (uiLabel == null)
        {
            Debug.Log("Label NOT found"); //Checks if label is found
        }
        if (uiScanWarning == null)
        {
            Debug.Log("Scan text NOT found"); //Checks if scan warning is found
        }

        uiButton.RegisterCallback<ClickEvent>(OnButtonClick);           //Runs OnButtonClick funtion when button is clicked
        uiBackButton.RegisterCallback<ClickEvent>(OnBackButtonClick);   //Runs OnBackButtonClick funtion when button is clicked
    }

    // Start is called before the first frame update
    void Start()
    {
        //ReadCSV();                                        //Start reading Excel file
        uiGroupBox.style.borderBottomWidth = 3;             //Layout of the second UI window
        uiGroupBox.style.borderBottomColor = Color.grey;
        uiGroupBox.style.borderRightWidth = 3;
        uiGroupBox.style.borderRightColor = Color.grey;
        uiGroupBox.style.borderTopWidth = 3;
        uiGroupBox.style.borderTopColor = Color.grey;
        uiGroupBox.style.borderLeftWidth = 3;
        uiGroupBox.style.borderLeftColor = Color.grey;
        uiBackButton.visible = false;

        areaSensor = Mathf.PI * Mathf.Pow(radiusSensor, 2);             //Calculate area of the sensor according to pi*radius^2
        Debug.Log("Area of the sensor is: " + areaSensor.ToString());
    }

    public float TimeStep = 1.00f;          //Time step with which the data is being gathered
    public ArduinoInput ArduinoScript;      //Get access to variables in ArduinoInput script
    public InputBloodVesselInfo BVscript;   //Get access to variables in InputBloodVesselInfo script

    private bool running = false;   //Bool that says whether the data from the Arduino is transfered to the UI

    public void OnButtonClick(ClickEvent evt)
    {
        if (running == false)
        {
            //InvokeRepeating("UpdateValues", 0, TimeStep);    //Calls function UpdateProgressValue every TimeStep seconds
            uiButton.text = "Running...";
            running = true;
        }

        else
        {
            uiButton.text = "Stopped, press to start";
            Debug.Log("Quit");
            running = false;
        }
    }

    public void OnBackButtonClick(ClickEvent evt)
    {
        if (running == false)
        {
            uiGroupBox.visible = false;
            BVscript.uiGroupBox.visible = true;
            uiInfoLabel.visible = false;
            uiBackButton.visible = false;
            BVscript.progressBar.enabled = false;
            BVscript.progressBarBackground.enabled = false;
        }

        if (running == true)
        {
            uiButton.text = "Stopped";
            uiGroupBox.visible = false;
            BVscript.uiGroupBox.visible = true;
            uiInfoLabel.visible = false;
            uiBackButton.visible = false;
            BVscript.progressBar.enabled = false;
            BVscript.progressBarBackground.enabled = false;
            running = false;
        }
    }

    private int progressValue = 0;          //Initialize start value of progressbar
    public float distance;               //Distance the guidewire is in the bloodvessel
    public float CurrentPressure;        //Variable that displays the current pressure of the GW on the BV
    private float MaximalPressure = 0.00f;  //Variable that displays the maximum pressure of the GW on the BV
    private float RelativePressure = 0.00f; //Variable that displays the relative pressure of the GW on the BV
    private string[] ArduinoData;           //Data from Arduino
    private float r = 0.0f;                 //RGB-value to set color of the progressbar
    private float g = 0.0f;                 //RGB-value to set color of the progressbar
    private float b = 0.0f;                 //RGB-value to set color of the progressbar
    private float a = 1.0f;                 //RGB-value to set color of the progressbar
    private float relativeDistance;          //Distance of guidewire in bloodvessel relative to distance to vasoconstrictions


    // Update is called once per frame
    void Update()
    {
        if (uiGroupBox.visible == true)
        {
            uiDistancePB.highValue = BVscript.uiLocationVC.value;
        }

        if (running == true)
        {
            UpdateValues();
        }
    }

    public float radiusSensor = 0.0051f;    //Radius of the pressure sensor in m
    private float areaSensor;               //Area of the pressure sensor in m^2

    void UpdateValues()
    {
        //From force (N) to pressure (Pa)
        CurrentPressure = (float)(CurrentPressure * (9.8 / 100));   //Scale force back from 0-100N, to 0.3-9.8N
        CurrentPressure /= areaSensor;                              //Force from pressure sensor is divided by the area to gain the pressure on the area
        CurrentPressure = (float)(CurrentPressure * 0.001);          //Scale pressure from Pa to hPa
        
        //Debug.Log(CurrentPressure);
        //Debug.Log(distance);
        if (distance < 0)
        {
            distance = 0;
        }

        relativeDistance = distance / BVscript.VClocation * 100;

        if (relativeDistance >= 0 & relativeDistance <= 25)  //To change value of the maximum pressure according to distance in blood vessel
        {
            MaximalPressure = 60.1f;
        }
        if (relativeDistance > 25 & relativeDistance <= 50)  //To change value of the maximum pressure according to distance in blood vessel
        {
            MaximalPressure = 69.5f;
        }
        if (relativeDistance > 50 & relativeDistance <= 75)  //To change value of the maximum pressure according to distance in blood vessel
        {
            MaximalPressure = 58.7f;
        }
        if (relativeDistance > 75 & relativeDistance <= 100)  //To change value of the maximum pressure according to distance in blood vessel
        {
            MaximalPressure = 55.2f;
        }

        if (BVscript.BVtype == "Aorta")
        {
            MaximalPressure += 3;
        }
        if (BVscript.BVtype == "Coronary artery")
        {
            MaximalPressure -= 4;
        }
        if (BVscript.BVtype == "Femoral artery")
        {
            MaximalPressure += 2;
        }

        uiFloFieldCur.value = Mathf.Round(CurrentPressure * 10.0f)/10.0f;
        uiFloFieldMax.value = Mathf.Round(MaximalPressure * 10.0f)/10.0f;

        uiLabel.text = "Relative pressure: " + Convert.ToInt32(RelativePressure).ToString() + "%";
        uiDistancePB.value = distance;
        BVscript.uiDistanceText.text = "Distance to vasoconstriction is: " + (Mathf.Round((uiDistancePB.highValue - distance) * 10.0f)/10.0f).ToString() + " cm";

        RelativePressure = CurrentPressure / MaximalPressure * 100;

        if (RelativePressure <= 50)
        {
            r = 1.0f * (RelativePressure / 50);
            g = 1.0f;

            horzProgressBar.fillAmount = RelativePressure / 100;
            horzProgressBar.color = new Color(r, g, b, a);
        }

        //Checks if the relative pressure is greater than 50 and smaller than or equal to 100 to be able to visualize in the
        //progressbar with color (from yellow/orange to red)
        if (RelativePressure > 50 & RelativePressure <= 100)
        {
            r = 1.0f;
            g = 1.0f * (1 - (RelativePressure - 50.0f) / 50);

            horzProgressBar.fillAmount = RelativePressure / 100;
            horzProgressBar.color = new Color(r, g, b, a);
        }

        //If the relative pressure is larger than 100, the progressbar will display 100
        if (RelativePressure > 100)
        {
            horzProgressBar.fillAmount = 1;
            horzProgressBar.color = Color.red;
        }
        progressValue += 1;     //Increase progressValue by 1 to index the next timestamp data next time

        for (int i = 0; i < healthBarPoints.Length; i++)
        {
            healthBarPoints[i].enabled = !DisplayHealthPoint(RelativePressure, i);
        }

        /*if (progressValue < duration)
        {
            distance = myTimePath.positioncoordinates[progressValue].PosX;
            CurrentPressure = myTimePath.positioncoordinates[progressValue].curPressure;
            MaximalPressure = myTimePath.positioncoordinates[progressValue].maxPressure;
            RelativePressure = CurrentPressure / MaximalPressure * 100;

            uiFloFieldCur.value = CurrentPressure;    //Writes value from Excel file to float field
            uiFloFieldMax.value = MaximalPressure;    //Writes value from Excel file to float field

            uiLabel.text = "Relative pressure: " + Convert.ToInt32(RelativePressure).ToString() + "%";

            uiDistancePB.value = distance;

            //Checks if the relative pressure is smaller than or equal to 50 to be able to visualize in the progressbar with color
            //(from green to yellow/orange)
            if (RelativePressure <= 50)
            {
                r = 1.0f * (RelativePressure / 50);
                g = 1.0f;

                horzProgressBar.fillAmount = RelativePressure / 100;
                horzProgressBar.color = new Color(r, g, b, a);
            }

            //Checks if the relative pressure is greater than 50 and smaller than or equal to 100 to be able to visualize in the
            //progressbar with color (from yellow/orange to red)
            if (RelativePressure > 50 & RelativePressure <= 100)
            {
                r = 1.0f;
                g = 1.0f * (1 - (RelativePressure - 50.0f) / 50);

                horzProgressBar.fillAmount = RelativePressure / 100;
                horzProgressBar.color = new Color(r, g, b, a);
            }

            //If the relative pressure is larger than 100, the progressbar will display 100
            if (RelativePressure > 100)
            {
                horzProgressBar.fillAmount = 1;
                horzProgressBar.color = Color.red;
            }
            progressValue += 1;     //Increase progressValue by 1 to index the next timestamp data next time

            for (int i = 0; i < healthBarPoints.Length; i++)
            {
                healthBarPoints[i].enabled = !DisplayHealthPoint(RelativePressure, i);
            }
        }

        else
        {
            uiButton.text = "Finished";
        }*/
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }
}
