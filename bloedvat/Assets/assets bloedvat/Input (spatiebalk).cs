//=========================================
//dit is het script voor de input
//op dit moment is dat spatiebalk die een kracht toevoegd aan de guidewire
//=========================================




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float ForceApplied = -20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
        if (Input.GetKey("space"))
        {
            rb.AddForce(0, -ForceApplied * Time.deltaTime, 0);
        }

       

    }
}

