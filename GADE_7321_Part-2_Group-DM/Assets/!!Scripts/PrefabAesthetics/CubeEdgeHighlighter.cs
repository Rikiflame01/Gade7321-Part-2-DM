using UnityEngine;

public class EdgeHighlighter : MonoBehaviour
{
    //this script is used to highlight the edges of a cube

    /*
     * The EdgeHighlighter script in Unity is designed to visually outline the edges of a cube by dynamically 
     * creating lines along each edge. It uses a LineRenderer component to draw lines between defined vertices 
     * of the cube. Upon starting, the script instantiates twelve lines corresponding to the edges of the cube,
     * each anchored to the cube’s transform. It sets the material, width, and positions for these lines based 
     * on the start and end points passed to the AddLineRenderer method. 
     * The script ensures all lines are child objects of the cube, allowing them to move and rotate with it.
     */

    public Material lineMaterial;  // Material you want to use for the lines

    void Start()
    {
        AddLineRenderer(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f));
        AddLineRenderer(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f));
        AddLineRenderer(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f));
        AddLineRenderer(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, 0.5f));
        AddLineRenderer(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f));
        AddLineRenderer(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f));
        AddLineRenderer(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f));
        AddLineRenderer(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, 0.5f));
        AddLineRenderer(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f));
        AddLineRenderer(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f));
        AddLineRenderer(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f));
        AddLineRenderer(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f));
    }

    void AddLineRenderer(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("Edge");
        lineObj.transform.parent = this.transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.positionCount = 2;

        lr.SetPosition(0, transform.TransformPoint(start));
        lr.SetPosition(1, transform.TransformPoint(end));
    }
}
