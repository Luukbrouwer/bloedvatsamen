using UnityEngine;
using System.Collections.Generic;

public class SpringBehaviornew : MonoBehaviour
{
    public float springConstant = 100.0f;
    public float restLength = 1.0f; // dit is de veer lengte in rust
    public float damping = 0.5f;

    public List<Rigidbody> connectedObjects = new List<Rigidbody>(); // een rigidbody lijst omdat je een kracht wil kunnen toevoegen

    [HideInInspector]
    public float forceMagnitude; // Voeg een variabele toe om de kracht op te slaan

   /* public void CalculateForce()
    {
        foreach (Rigidbody connectedObject in connectedObjects)
        {
            if (connectedObject == null)
            {
                Debug.LogWarning("Connected object not assigned in SpringBehavior.");
                continue;
            }

            // Calculate displacement from rest length
            Vector3 displacement = connectedObject.transform.position - transform.position;
            float distance = displacement.magnitude;

            // Calculate the spring force using Hooke's law
            float forceMagnitude = -springConstant * (distance - restLength) - damping * Vector3.Dot(displacement.normalized, connectedObject.velocity);

            Debug.Log("Force between " + gameObject.name + " and " + connectedObject.gameObject.name + ": " + forceMagnitude);

            // Apply spring force to the current object's rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(displacement.normalized * forceMagnitude);
        }

        // Bereken en sla de kracht op
        forceMagnitude = Mathf.Abs(forceMagnitude);

        Debug.Log("Force on cube: " + forceMagnitude);
    } */

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
            Vector3 displacement = connectedObject.transform.position - transform.position;// buurplaats-eigenplaats= afstant tussen objecten
            float distance = displacement.magnitude; //grootte van de afstant tussen objecten
            float forceMagnitude = -springConstant * (distance - restLength) - damping * Vector3.Dot(displacement.normalized, connectedObject.velocity);//pas formules toe
            
            // Apply spring force
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(displacement.normalized * forceMagnitude);
        }
    }
}