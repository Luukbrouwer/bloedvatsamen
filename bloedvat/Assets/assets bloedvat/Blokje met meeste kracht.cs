//===================
//dit is het script voor berekenen wat de output (stretch)
//==================


using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;


public class ForceCalculator : MonoBehaviour
{
    public GridGenerator gridGenerator; // Sleep je GridGenerator-scriptcomponent hierin in de Unity Inspector.
    public TMP_Text resultText; // Het tekstveld om de waarde weer te geven

    private float maxDownwardDistance; 
    private GameObject blockWithMaxDisplacement; 

    
    
    void Update()
    {
        FindMaxDownwardBlock(); 
    }

    

    void FindMaxDownwardBlock()
    {
        maxDownwardDistance = float.MinValue;//
        
        blockWithMaxDisplacement = null;

        //float maxForceMagnitude = float.MinValue; // Initialize with a very small value

             

        for (int x = 0; x < gridGenerator.columns; x++)
        {
            for (int z = 0; z < gridGenerator.rows; z++)
            {
                
                GameObject cube = gridGenerator.GetGridObject(x, z);
                Vector3 Startingplace = gridGenerator.startingplace[cube.GetComponent<Rigidbody>()];
                SpringBehaviornew springBehavior = cube.GetComponent<SpringBehaviornew>();

                float distance = Vector3.Distance(cube.transform.position, Startingplace); 

                if (distance > maxDownwardDistance)
                {
                    maxDownwardDistance = distance;                    
                    blockWithMaxDisplacement = cube;                    
                }
            }
        }

        
        if (blockWithMaxDisplacement != null)
        {
            //===========show max displacement
            resultText.text = "highest strech = " + maxDownwardDistance.ToString();
            maxDownwardDistance=float.MinValue;//reset maxDownwardDistance
        }
    }


}
