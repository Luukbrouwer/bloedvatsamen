using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;

public class InputStrainInfo : MonoBehaviour
{
    /////////////////////////////////START READING EXCEL FILE//////////////////////////////////////////
    public TextAsset textAssetData;

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
    }
    /////////////////////////////////DONE READING EXCEL FILE//////////////////////////////////////////

    UIDocument StrainInfoDocument;
    GroupBox uiGroupBox;
    UnityEngine.UIElements.Button uiButton;
    FloatField uiFloFieldCur;
    FloatField uiFloFieldMax;
    Label uiLabel;

    public UnityEngine.UI.Image horzProgressBar;
    public UnityEngine.UI.Image[] healthBarPoints;

    void OnEnable()     //This void gets the UI document and all the different components of the UI for further referencing
    {
        StrainInfoDocument = GetComponent<UIDocument>();

        if (StrainInfoDocument == null)
        {
            Debug.LogError("No button document found"); //Checks if UI document is found
        }

        uiGroupBox = StrainInfoDocument.rootVisualElement.Q("GroupBox") as GroupBox;

        if (uiGroupBox == null)
        {
            Debug.Log("Group box NOT found"); //Checks if group box is found
        }

        uiFloFieldCur = StrainInfoDocument.rootVisualElement.Q("CurrentPressureValue") as FloatField;

        if (uiFloFieldCur == null)
        {
            Debug.Log("Current pressure float field NOT found"); //Checks if current pressure float field is found
        }

        uiFloFieldMax = StrainInfoDocument.rootVisualElement.Q("MaximalPressureValue") as FloatField;

        if (uiFloFieldMax == null)
        {
            Debug.Log("Maximal pressure float field NOT found"); //Checks if maximal pressure float field is found
        }

        uiButton = StrainInfoDocument.rootVisualElement.Q("TestButton") as UnityEngine.UIElements.Button;

        if (uiButton == null)
        {
            Debug.Log("Button NOT found"); //Checks if button is found
        }

        uiLabel = StrainInfoDocument.rootVisualElement.Q("RelativePressureLabel") as Label;

        if (uiLabel == null)
        {
            Debug.Log("Label NOT found"); //Checks if label is found
        }

        uiButton.RegisterCallback<ClickEvent>(OnButtonClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadCSV();
/*        uiGroupBox.style.backgroundColor = Color.white;
        uiGroupBox.style.borderBottomWidth = 3;
        uiGroupBox.style.borderBottomColor = Color.black;
        uiGroupBox.style.borderRightWidth = 3;
        uiGroupBox.style.borderRightColor = Color.black;
        uiGroupBox.style.borderTopWidth = 3;
        uiGroupBox.style.borderTopColor = Color.black;
        uiGroupBox.style.borderLeftWidth = 3;
        uiGroupBox.style.borderLeftColor = Color.black;*/
        //uiSlider.showInputField = false;
    }

    public float TimeStep = 1.00f;  //Time step with which the data is being gathered

    public void OnButtonClick(ClickEvent evt)
    {
        //InvokeRepeating("UpdateValues", 0, TimeStep);    //Calls function UpdateProgressValue every TimeStep seconds
        uiButton.text = "Running...";
    }

    // Update is called once per frame
    void Update()
    {
        if (uiButton.text == "Running...")
        {
            UpdateValues();
        }
    }

    private int progressValue = 0;    //Initialize start value of progressbar
    private float CurrentPressure = 0.00f;
    private float MaximalPressure = 0.00f;
    private float RelativePressure = 0.00f;
    private float r = 0.0f;
    private float g = 0.0f;
    private float b = 0.0f;
    private float a = 1.0f;
    private float lerpSpeed;

    void UpdateValues()
    {
        //lerpSpeed = 3f * Time.deltaTime;

        if (progressValue < duration)
        {
            CurrentPressure = myTimePath.positioncoordinates[progressValue].curPressure;
            MaximalPressure = myTimePath.positioncoordinates[progressValue].maxPressure;
            RelativePressure = CurrentPressure / MaximalPressure * 100;

            uiFloFieldCur.value = CurrentPressure;    //Writes value from Excel file to float field
            uiFloFieldMax.value = MaximalPressure;    //Writes value from Excel file to float field

            uiLabel.text = "Relative pressure: " + Convert.ToInt32(RelativePressure).ToString() + "%";

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

            for (int i = 0; i < healthBarPoints.Length; i ++)
            {
                healthBarPoints[i].enabled = !DisplayHealthPoint(RelativePressure, i);
            }
        }

        else
        {
            uiButton.text = "Finished";
        }
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }
}
