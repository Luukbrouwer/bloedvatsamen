using UnityEngine;

public class SimpleGridGenerator : MonoBehaviour
{
    public GameObject prefab; // Assign your object prefab in the Unity Inspector.
    public int rows = 5; // Adjust the number of rows.
    public int columns = 5; // Adjust the number of columns.
    public float spacing = 1.0f; // Adjust the spacing between objects.

    private GameObject[,] gridObjects;

    void Start()
    {
        GenerateGrid();
        ConnectObjects();
    }

    void GenerateGrid()
    {
        gridObjects = new GameObject[columns, rows];

        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                GameObject newObj = Instantiate(prefab, position, Quaternion.identity);
                gridObjects[x, z] = newObj;
            }
        }
    }

    void ConnectObjects()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                GameObject currentObject = gridObjects[x, z];
                SpringBehavior springBehavior = currentObject.GetComponent<SpringBehavior>();

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        int neighborX = x + dx;
                        int neighborZ = z + dz;

                        if (neighborX >= 0 && neighborX < columns && neighborZ >= 0 && neighborZ < rows)
                        {
                            GameObject neighborObject = gridObjects[neighborX, neighborZ];
                            springBehavior.connectedObjects.Add(neighborObject.GetComponent<Rigidbody>());
                        }
                    }
                }
            }
        }
    }
}
