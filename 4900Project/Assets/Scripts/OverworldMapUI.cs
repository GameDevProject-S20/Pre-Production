using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMapUI : MonoBehaviour
{
    public GameObject LocationPrefab;
    public GameObject PathPrefab;
    private OverworldMap.LocationGraph graph;

    private bool drawQueued; // Used to avoid OnStart sync issues  

    // Start is called before the first frame update
    void Start()
    {
        SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (drawQueued)
        {
            DrawGraph();
            drawQueued = false;
        }
    }

    private void SetVisible(bool visible)
    {
        foreach (var r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.enabled = visible;
        }
    }

    public void SetGraph(OverworldMap.LocationGraph graph)
    {
        this.graph = graph;
    }

    public void QueueDraw()
    {
        drawQueued = true;
    }

    private void DrawGraph()
    {
        SetVisible(true);

        // Draw nodes
        // Currently not rendering at the correct location on the map -- how do we fix this?
        foreach (var node in graph.GetNodeEnumerable())
        {
            GameObject spriteObj = Instantiate(LocationPrefab, transform.parent);
            Vector3 pos = new Vector3(node.PosX, node.PosY, 0);
            spriteObj.transform.position += pos;
        }

        // Draw edges
        // Currently not rendering at the correct location on the map -- how do we fix this?
        foreach (var edge in graph.GetEdgeEnumerable())
        {
            GameObject line = Instantiate(PathPrefab, transform);
            LineRenderer lr = line.GetComponent<LineRenderer>();
            Vector3[] lineEnds =
                {
                    new Vector3(edge.Item1.PosX, edge.Item1.PosY, transform.position.z),
                    new Vector3(edge.Item2.PosX, edge.Item2.PosY, transform.position.z)
                };
            lr.SetPositions(lineEnds);
        }
    }
}
