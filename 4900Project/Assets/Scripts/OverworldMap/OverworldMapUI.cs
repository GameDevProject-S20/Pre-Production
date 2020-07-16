using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Encounters;

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

    [SerializeField]
    Button enterNodeButton;

    [SerializeField]
    GameObject TownMenuGameObject;
    [SerializeField]
    GameObject EnterNodeButtonCanvas;

    bool isActive = true;
    bool isTravelling = false;
    //Movement variables
    float translateSmoothTime = 1.5f;
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
            GameObject nodeObj = Instantiate(LocationPrefab, transform.parent);
            Vector3 pos = new Vector3(node.PosX, 0, node.PosY) * DataTracker.Current.mapScale * 2;
            nodeObj.transform.position += pos;
            nodeObj.transform.SetParent(transform, true);
            nodeObj.name = node.Name;
            nodeObj.GetComponent<MapNode>().nodeID = node.Id;
            if (node.Type == OverworldMap.LocationType.TOWN)
            {
                nodeObj.transform.Find("Icon").gameObject.SetActive(false);
                nodeObj.transform.Find("TownMesh").gameObject.SetActive(true);
                nodeObj.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);
            }
            else if (node.Type == OverworldMap.LocationType.EVENT)
            {
                nodeObj.transform.Find("Icon").gameObject.SetActive(false);
                nodeObj.transform.Find("EncounterMark").gameObject.SetActive(true);
            }
            if (node.Id == DataTracker.Current.currentLocationId)
            {
                playerMarker.transform.position = nodeObj.transform.position;
                targetPos = playerMarker.transform.position;
            }
        }


        // Draw edges
        // Currently not rendering at the correct location on the map -- how do we fix this?
        foreach (var edge in DataTracker.Current.WorldMap.GetEdgeEnumerable())
        {
            GameObject line = Instantiate(PathPrefab, transform.parent);
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
            line.transform.SetParent(transform, true);
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive && !isTravelling)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999, LayerMask.GetMask("MapNode")))
            {
                int selected = hit.collider.gameObject.GetComponent<MapNode>().nodeID;
                if (DataTracker.Current.WorldMap.HasEdge(selected, DataTracker.Current.currentLocationId))
                {
                    DataTracker.Current.currentLocationId = selected;
                    targetPos = hit.collider.gameObject.transform.position;
                    isTravelling = true;
                    OverworldMap.LocationNode node;
                    if (DataTracker.Current.WorldMap.GetNode(selected, out node))
                    {
                        Debug.Log(node.Name);
                        if (node.Type == OverworldMap.LocationType.TOWN || node.Type == OverworldMap.LocationType.EVENT)
                        {
                            enterNodeButton.interactable = true;
                        }
                        else
                        {
                            enterNodeButton.interactable = false;
                        }
                    }
                }
            }
        }

        playerMarker.transform.position = Vector3.SmoothDamp(playerMarker.transform.position, targetPos, ref translatSmoothVelocity, translateSmoothTime);
        if (Vector3.Distance(playerMarker.transform.position, targetPos) > 0.1f){
            Vector3 dir = ((targetPos - playerMarker.transform.position).normalized);
            float theta = Vector2.SignedAngle(new Vector2(dir.x, dir.z), Vector2.left);
            playerMarker.transform.eulerAngles = new Vector3(-90, 0, theta );
        }
        else{
            isTravelling = false;
        }
    }



    public void OnButtonClick()
    {
        OverworldMap.LocationNode node = DataTracker.Current.GetCurrentNode();
        switch (node.Type)
        {
            case OverworldMap.LocationType.TOWN:
                TownMenuGameObject.GetComponent<TownWindow>().UpdatePrefab();
                Debug.Log("town");
                TownMenuGameObject.SetActive(true);
                EnterNodeButtonCanvas.SetActive(false);
                isActive = false;
                break;
            case OverworldMap.LocationType.EVENT:
                Debug.Log("Event");
                //SceneManager.LoadScene("Encounter", LoadSceneMode.Additive);
                var mgr = EncounterManager.Instance;
                mgr.RunRandomEncounter();
                break;
            default:
                break;
        }
    }

    public void TownMapClosed()
    {
        TownMenuGameObject.SetActive(false);
        EnterNodeButtonCanvas.SetActive(true);
        isActive = true;
    }
}
