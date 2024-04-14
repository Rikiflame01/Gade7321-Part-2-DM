using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public int width = 5;
    public int height = 5;
    public int depth = 5;
    public float spacing = 1.1f;

    public Vector3 GridCenter { get; private set; }

    void Start()
    {
        GenerateGrid();
        CalculateCenter();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Vector3 position = new Vector3(x * spacing, y * spacing, z * spacing);
                    Instantiate(cubePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }

    void CalculateCenter()
    {
        GridCenter = new Vector3((width - 1) * spacing / 2, (height - 1) * spacing / 2, (depth - 1) * spacing / 2);
    }

}