using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionScript : MonoBehaviour
{
    private Transform plane;  // Reference to the position of the game object
    public TMP_Text positionText; // Reference to the Text object for displaying position

    // Start is called before the first frame update
    void Start()
    {
        plane = GetComponent<Transform>(); // Get the position component
    }

    // Update is called once per frame
    void Update()
    {
        positionText.text = "Position: " + plane.position.y.ToString("F2") + "m"; //"2" = 2 decimals
    }
}
