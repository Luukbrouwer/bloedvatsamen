using UnityEngine;

public class ElasticDeformation : MonoBehaviour
{
    public float springConstant = 100.0f; // Adjust this value for your specific simulation.
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Apply a force to deform the object
        Vector3 deformationForce = springConstant * (initialPosition - transform.position);
        GetComponent<Rigidbody>().AddForce(deformationForce);
    }
}
