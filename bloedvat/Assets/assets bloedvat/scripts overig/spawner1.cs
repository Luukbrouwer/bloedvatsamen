/*using UnityEngine;

public class GridGenerator2 : MonoBehaviour
{
    public GameObject prefab; // koppeling naar prefab
    public int rows = 5; 
    public int columns = 5; 
    public Vector3 objectSize = new Vector3(1.0f, 1.0f, 1.0f); // blokjes in grid size aanpassen

    private GameObject[,] gridObjects;  // maak een array die de plaatsen van een grid bevat

    
    

    void Start()// dit script heeft alleen een void start, want het wordt maar 1 keer uitgevoerd aan het begin.
    {
        GenerateGrid();
        ConnectObjects();
    }

    
    void GenerateGrid()
    {
        gridObjects = new GameObject[columns, rows];    // de eerder gedefinede gridObjects krijgt nu een maat namelijk rowsxcolumns
        //ga elk object in grid langs, ittereer langs x enz :
        for (int x = 0; x < columns; x++)   
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = new Vector3(x * objectSize.x, 0, z * objectSize.z); // maak vextor3(x,y,z) en schaal hem naar de juiste Size
                //You might use Instantiate to create objects dynamically, and then use the Transform component to modify their properties after they've been created.
                GameObject newObj = Instantiate(prefab, position, Quaternion.identity); //instantiate  creates  new instances of objects;   Quaternion.identity zorgd ervoor dat het blokje niet geroteerd is bij plaatsen


                // Adjust the size of the newly created object.
                newObj.transform.localScale = objectSize;

                

                gridObjects[x, z] = newObj;//plaats het net gemaakte object in de array gridobjects om hem later terug te kunnen halen
            }
        }
    }

    void ConnectObjects()
    {
        

        for (int x = 0; x < columns; x++)//ga elke x af
        {
            for (int z = 0; z < rows; z++)//ga elke y af
            {
                GameObject currentObject = gridObjects[x, z]; //haalt object op dat in de array gridobjects is geplaatst en noemt hem currentobject
                SpringBehavior springBehavior = currentObject.GetComponent<SpringBehavior>(); // hier wordt een ander script (springbehavior) opgehaald om naar te kunnen verwijzen


                // gaat alle objecten langs die naast het huidige object staan, dus dat zijn acht omliggenden. 
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        if (dx == 0 && dz == 0) continue; // Skip the central object itself

                        int neighborX = x + dx;
                        int neighborZ = z + dz;

                        if (neighborX >= 0 && neighborX < columns && neighborZ >= 0 && neighborZ < rows)// maakt alleen een neighbor aan als deze ook bestaat(, soms zit je op de rand)
                        {
                            GameObject neighborObject = gridObjects[neighborX, neighborZ]; // noemt de zojuistgevonden buur neighborobject om ernaar te kunnen verwijzen in de volgende regel
                            springBehavior.connectedObjects.Add(neighborObject.GetComponent<Rigidbody>());// in het script springBehavior is een lijst aangemaakt voor buren, hier wordt dit object nu aan toegevoetgd
                        }


                   
                        
                    }
                }

                //randen vast zetten
                Rigidbody currentObjectRigidbody = currentObject.GetComponent<Rigidbody>();

                if (x == 0 || x == columns - 1)
                {
                    GameObject neighborObject = springBehavior.connectedObjects[x, z];
                    foreach (Rigidbody neighbor in neighborObject)
                    {
                        if (neighbor.position.x == currentObjectRigidbody.position.x)
                        {
                            FixedJoint fixedJoint = currentObject.AddComponent<FixedJoint>();
                            fixedJoint.connectedBody = neighbor;
                        }
                    }
                }

                if (z == 0 || z == rows - 1)
                {
                    GameObject neighborObject = springBehavior.connectedObjects[x, z];
                    foreach (Rigidbody neighbor in neighborObject)
                    {
                        if (neighbor.position.z == currentObjectRigidbody.position.z)
                        {
                            FixedJoint fixedJoint = currentObject.AddComponent<FixedJoint>();
                            fixedJoint.connectedBody = neighbor;
                        }
                    }
                }
            }
        }
    }
}
*/