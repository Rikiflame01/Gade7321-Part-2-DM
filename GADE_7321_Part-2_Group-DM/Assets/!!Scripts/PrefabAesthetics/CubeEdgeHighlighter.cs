using UnityEngine;

public class EdgeHighlighter : MonoBehaviour
{
    public Material lineMaterial;  // Ensure this is set to your initial material in the Unity Inspector

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
