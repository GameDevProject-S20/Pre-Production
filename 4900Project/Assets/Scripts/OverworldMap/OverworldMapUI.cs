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
            GameObject nodeObj = Instantiate(LocationPrefab, transform.parent);
            Vector3 pos = new Vector3(node.PosX, 0, node.PosY) * 10;
            nodeObj.transform.position += pos;
            nodeObj.transform.SetParent(transform, true);
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

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if(Physics.Raycast (ray, out hit,999, LayerMask.GetMask("MapNode")))
            {
                int selected =hit.collider.gameObject.GetComponent<MapNode>().nodeID;
                if (DataTracker.Current.WorldMap.HasEdge(selected, DataTracker.Current.currentLocationId)){
                    float weight = DataTracker.Current.Player.Inventory.TotalWeight();
                    int fuel = DataTracker.Current.Player.Inventory.Contains("Fuel");
                    //will be done with percent of max capacity later, just testing with constants for now
                    int fuelrate;
                    int dayrate;
                    if (weight > 60) {
                        fuelrate = 3;
                        dayrate = 2;
                    } else if(weight > 30) {
                        fuelrate = 2;
                        dayrate = 1;
                    } else {
                        fuelrate = 1;
                        dayrate = 1;
                    }
                    if (Input.GetMouseButtonDown(0)) //left click for node info (player is more likely to see first this way)
                    {
                        Debug.Log("Weight="+weight);
                        Debug.Log("Traveling here will take "+dayrate+" day(s) and " + fuelrate + " fuel"); //example weight-fuel/time rates: below 30% max weight = 1 fuel 1 day, 31-100% weight = 2 fuel 1 day, >100% = 3 fuel 2 days
                        Debug.Log("Current fuel: " + fuel);
                        Debug.Log("Current day: " + DataTracker.Current.dayCount);
                        Debug.Log("You will have " + (fuel - fuelrate) + " fuel left after traveling here");
                        Debug.Log("It will be day " + (DataTracker.Current.dayCount + dayrate) + " after traveling here");


                    } else if(fuel-fuelrate>0)
                    {
                        DataTracker.Current.Player.Inventory.RemoveItem("Fuel", fuelrate);
                        DataTracker.Current.dayCount += dayrate;
                       //update date/fuel
                       //check enough fuel, gameover if not enough
                        DataTracker.Current.currentLocationId = selected;
                        targetPos = hit.collider.gameObject.transform.position;
                        OverworldMap.LocationNode node;
                        if (DataTracker.Current.WorldMap.GetNode(selected, out node)) {
                            if (node.Type == OverworldMap.LocationType.TOWN) {
                                enterNodeButton.interactable = true;
                            } else {
                                enterNodeButton.interactable = false;
                            }
                        }
                    } else
                    {
                        Debug.Log("You're out of fuel! On the bright side not only will the scavengers get a good payday from your remains, but you'll serve as a good reminder to always check the tanks before leaving port\nGAME OVER");
                    }
                }
            }
        }

        playerMarker.transform.position = Vector3.SmoothDamp(playerMarker.transform.position, targetPos, ref translatSmoothVelocity, translateSmoothTime);
    }

    public void OnButtonClick(){
        SceneManager.LoadScene("Town");
    }

}
