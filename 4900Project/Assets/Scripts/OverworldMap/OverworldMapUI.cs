using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class OverworldMapUI : MonoBehaviour
{
    [SerializeField]
    GameObject LocationPrefab;
    [SerializeField]
    GameObject PathPrefab;
    [SerializeField]
    GameObject playerMarker;

    [SerializeField]
    Transform NodesContainer;
    [SerializeField]
    Transform PathsContainer;

    // This should exist elsewhere - in the hud or something
    [SerializeField]
    Button enterNodeButton;

    //Movement variables
    float translateSmoothTime = 0.1f;
	Vector3 translatSmoothVelocity;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        DrawGraph();
        Camera.main.transform.position = playerMarker.transform.position + new Vector3(0, 6, 0);
    }

    private void DrawGraph()
    {
        // Draw nodes
        foreach (var node in DataTracker.Current.WorldMap.GetNodeEnumerable())
        {
            GameObject nodeObj = Instantiate(LocationPrefab, transform.root);
            Vector3 pos = new Vector3(node.PosX, 0, node.PosY) * DataTracker.Current.mapScale * 2;
            nodeObj.transform.position += pos;
            nodeObj.transform.SetParent(NodesContainer, true);
            nodeObj.name = node.Name;
            nodeObj.GetComponent<MapNode>().nodeID = node.Id;
            if (node.Type == OverworldMap.LocationType.TOWN){
                nodeObj.transform.Find("Icon").gameObject.SetActive(false);
                nodeObj.transform.Find("TownMesh").gameObject.SetActive(true);
                nodeObj.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);
            }
            if (node.Id == DataTracker.Current.currentLocationId){
                playerMarker.transform.position = nodeObj.transform.position;
                targetPos = playerMarker.transform.position;
            }
        }


        // Draw edges
        // Currently not rendering at the correct location on the map -- how do we fix this?
        foreach (var edge in DataTracker.Current.WorldMap.GetEdgeEnumerable())
        {
            GameObject line = Instantiate(PathPrefab, transform.root);
            line.name = "Edge_" + edge.Item1.Id + "-" + edge.Item2.Id;
            LineRenderer lr = line.GetComponent<LineRenderer>();
            Vector3[] lineEnds =
                {
                    new Vector3(edge.Item1.PosX, 0, edge.Item1.PosY)* DataTracker.Current.mapScale * 2,
                    new Vector3(edge.Item2.PosX, 0, edge.Item2.PosY)* DataTracker.Current.mapScale * 2
                };
                Vector3 a = lineEnds[0] - 0.2f * (lineEnds[0] - lineEnds[1]);
                Vector3 b = lineEnds[1] - 0.2f * (lineEnds[1] - lineEnds[0]);
                lineEnds[0] = a;
                lineEnds[1] = b;
            lr.SetPositions(lineEnds);
            line.transform.SetParent(PathsContainer, true);
        }

    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Send ray from camera to mouse position
            // If the ray hits a node, the player has clicked on that node
            if(Physics.Raycast (ray, out hit,999, LayerMask.GetMask("MapNode")))
            {
                int selected =hit.collider.gameObject.GetComponent<MapNode>().nodeID;
                // Move the player to selected node if it is adjacent to current node
                if (DataTracker.Current.WorldMap.HasEdge(selected, DataTracker.Current.currentLocationId)){
                    DataTracker.Current.currentLocationId = selected;
                    targetPos = hit.collider.gameObject.transform.position;
                    OverworldMap.LocationNode node;
                    if (DataTracker.Current.WorldMap.GetNode(selected, out node)){
                        if (node.Type == OverworldMap.LocationType.TOWN){
                            enterNodeButton.interactable = true;
                        }
                        else{
                            enterNodeButton.interactable = false;
                        }
                    }
                }
            }
        }

        // Update player's position over time
        playerMarker.transform.position = Vector3.SmoothDamp(playerMarker.transform.position, targetPos, ref translatSmoothVelocity, translateSmoothTime);
    }

    public void OnButtonClick(){
        SceneManager.LoadScene("Town");
    }

}
