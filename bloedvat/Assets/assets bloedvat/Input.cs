//=========================================
//dit is het script voor de input
//=========================================




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;





public class Movement : MonoBehaviour
{
    //start arduino connection:
    SerialPort data_stream =new SerialPort("COM3" , 115200);
    public string receivedstring;
    public Rigidbody rb;
    public string[] datas;
    
    private Vector3 beginGuwi;
    
    //======================
    //spatiebalk
    public float ForceApplied = -20f;
    //==========================

    
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
        float length= float.Parse(datas[0]) *0.00001f;
        transform.position = new Vector3(beginGuwi[0], beginGuwi[1] - length, beginGuwi[2]);
    }


    void FixedUpdate() //spatiebalk...
    {
        if (Input.GetKey("space"))
        {
            rb.AddForce(0, -ForceApplied * Time.deltaTime, 0);
        }
    }

    void OnApplicationQuit()
    {
       data_stream.Close(); 
    }
}


    

    




