//=========================================
//dit is het script voor de input
// in dit script wordt ook aan de guidewire een kracht gegeven relatief aan de druk die gemeten wordt. 
//=========================================



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;



public class ArduinoInput : MonoBehaviour
{
    //start arduino connection:
    SerialPort data_stream =new SerialPort("COM3" , 115200);
    public string receivedstring;
    public Rigidbody rb;
    public string[] datas;
    
    private Vector3 beginGuwi;
    
    public float areaGuideWire =1;
    public float PressureToForce;
   
    

    
    void Start()
    {
        //for arduino connection:
        data_stream.Open(); //initiate serial monitor "stream"
        beginGuwi= rb.transform.position;
    }


    void Update()
    {
        //for arduino connection:
        receivedstring = data_stream.ReadLine();
        string[] datas = receivedstring.Split(";"); //split data tussen ';'
        float CurrentPressure = float.Parse(datas[0]);

        PressureToForce= CurrentPressure * areaGuideWire;
        rb.AddForce(0, -PressureToForce * Time.deltaTime, 0);

        // voor bewegen van guidewire met magnetic angle sensor:===============================
        //float length= float.Parse(datas[0]) *0.00001f;
        //transform.position = new Vector3(beginGuwi[0], beginGuwi[1] - length, beginGuwi[2]);
        //=====================================================================================
    }


    

    void OnApplicationQuit()
    {
       data_stream.Close(); 
    }
}