//==================================
//dit script niet gebruiken, gebruik newSpringBeh
//==================================




using UnityEngine;
using System.Collections.Generic;

public class SpringBehavior : MonoBehaviour
{
    public float springConstant = 100.0f;
    public float restLength = 0.3f; // dit is de veer lengte in rust
    public float damping = 0.5f;

    public List<Rigidbody> connectedObjects = new List<Rigidbody>(); // een rigidbody lijst omdat je een kracht wil kunnen toevoegen


    //public Vector3[,] beginLenght = new Vector3[self, Connected];
    //public Vector3<Rigidbody> L = new Vector3<Rigidbody>();z

    
    private Dictionary<Rigidbody, Vector3> lengte = new Dictionary<Rigidbody, Vector3>();


    void Start()
    {

        foreach (Rigidbody connectedObject in connectedObjects)
        {
            Vector3 beginPos = transform.position;
            Vector3 beginPosConnected = connectedObject.transform.position;
            Vector3 distencevect =  beginPos - beginPosConnected;
            //L.GetComponent<Rigidbody>() = distencevect;
            lengte[GetComponent<Rigidbody>()] = distencevect;

        }
    }

    void FixedUpdate()
    {
        foreach (Rigidbody connectedObject in connectedObjects)
        {
            if (connectedObject == null)
            {
                Debug.LogWarning("Connected object not assigned in SpringBehavior.");
                continue;
            }

            //the following formula's are being applied to the next part of the script:
            //F = -k * (x - restLength)
            //damping = -c * V
            //Total Force = -k * Displacement - c * Velocity
       
            // Calculate displacement from rest length
            Vector3 displacement =   transform.position - connectedObject.transform.position;// buurplaats-eigenplaats= afstant tussen objecten
            Vector3 beginLength = lengte[GetComponent<Rigidbody>()];
            //float distance = displacement.magnitude; //grootte van de afstant tussen objecten
            Vector3 forceMagnitude = -springConstant * (displacement - beginLength);//- damping * Vector3.Dot(displacement.normalized, connectedObject.velocity)* displacement.normalized;//pas formules toe
            /*
            Vector3 displacement = connectedObject.transform.position - transform.position;// buurplaats-eigenplaats= afstant tussen objecten
            float distance = displacement.magnitude; //grootte van de afstant tussen objecten
            float forceMagnitude = -springConstant * (distance - restLength) - damping * Vector3.Dot(displacement.normalized, connectedObject.velocity);//pas formules toe
            */



            // Apply spring force
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(forceMagnitude);
        }
    }
}