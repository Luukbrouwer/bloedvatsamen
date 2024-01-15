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
    //List<string> datas = new List<string>();
    //public List<string> datas2;
    
    private Vector3 beginGuwi;
    
    public float areaGuideWire =1;
    public float PressureToForce;
    public float scalepressure=10;
    public Vector3 startGW;
    public float distance;
    public float CurrentPressure;
    

    
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
        //datas.Add(receivedstring.Split(";"));
        string[] datas= receivedstring.Split(";"); //split data tussen ';'
        //datas2= receivedstring.Split(";"); //split data tussen ';'
        float CurrentPressure = float.Parse(datas[0]);
        float distance = float.Parse(datas[1]);
        PressureToForce= CurrentPressure * areaGuideWire * scalepressure;
        rb.AddForce(0, -PressureToForce * Time.deltaTime, 0);

        // voor bewegen van guidewire met magnetic angle sensor:===============================
        //float length= float.Parse(datas[0]) *0.00001f;
        //transform.position = new Vector3(beginGuwi[0], beginGuwi[1] - length, beginGuwi[2]);
        //=====================================================================================
        
        if ( rb.transform.position.y >= beginGuwi.y )
        {
            transform.position = beginGuwi;
        }
        Debug.Log(CurrentPressure);
        Debug.Log(distance);
    }


    

    void OnApplicationQuit()
    {
       data_stream.Close(); 
    }
}