using UnityEngine;
using System.Collections.Generic;

public class SpringBehaviornew : MonoBehaviour
{
    public float springConstant = 100.0f;
    public float restLength = 1.0f; // dit is de veer lengte in rust
    //public float damping = 0.5f;

    
    //connectedObjects word in dit script gebruikt, maar in het script spawner worden de objecten in deze lijst gezet:
    public List<Rigidbody> connectedObjects = new List<Rigidbody>(); // een rigidbody lijst omdat je een kracht wil kunnen toevoegen


    
    void CalculateSpringForce()
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
            //denk dat er iets mis gaat in de demping dus heb dit er voor nu even uitgehaald
            float forceMagnitude = -springConstant * (distance - restLength) ;//- damping * Vector3.Dot(displacement.normalized, connectedObject.velocity);//pas formules toe
            
            // Apply spring force
            Rigidbody currentRB = GetComponent<Rigidbody>();
            currentRB.AddForce(displacement.normalized * forceMagnitude);
        } 
    } 

    void FixedUpdate()
    {
        CalculateSpringForce();
    }
}