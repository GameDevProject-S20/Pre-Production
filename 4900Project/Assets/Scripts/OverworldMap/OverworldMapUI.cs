using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMapUI : MonoBehaviour
{
    public GameObject LocationPrefab;
    public GameObject PathPrefab;

    // Start is called before the first frame update
    void Start()
    {
        DrawGraph();
    }

    private void DrawGraph()
    {
        // Draw nodes
        foreach (var node in DataTracker.Current.WorldMap.GetNodeEnumerable())
        {
            GameObject nodeObj = Instantiate(LocationPrefab, transform.parent);
            Vector3 pos = new Vector3(node.PosX, 0, node.PosY) * 10;
            nodeObj.transform.position += pos;
            nodeObj.transform.SetParent(transform, true);
            nodeObj.name = node.Name;
            if (node.Type == OverworldMap.LocationType.TOWN){
                nodeObj.transform.Find("Icon").gameObject.SetActive(false);
                nodeObj.transform.Find("TownMesh").gameObject.SetActive(true);
                nodeObj.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);
            }
        }


        // Draw edges
        // Currently not rendering at the correct location on the map -- how do we fix this?
        foreach (var edge in DataTracker.Current.WorldMap.GetEdgeEnumerable())
        {
            GameObject line = Instantiate(PathPrefab, transform.parent);
            LineRenderer lr = line.GetComponent<LineRenderer>();
            Vector3[] lineEnds =
                {
                    new Vector3(edge.Item1.PosX, 0, edge.Item1.PosY)* 10,
                    new Vector3(edge.Item2.PosX, 0, edge.Item2.PosY)* 10
                };
                Vector3 a = lineEnds[0] - 0.2f * (lineEnds[0] - lineEnds[1]);
                Vector3 b = lineEnds[1] - 0.2f * (lineEnds[1] - lineEnds[0]);
                lineEnds[0] = a;
                lineEnds[1] = b;
            lr.SetPositions(lineEnds);
            line.transform.SetParent(transform, true);
        }
    }
}
