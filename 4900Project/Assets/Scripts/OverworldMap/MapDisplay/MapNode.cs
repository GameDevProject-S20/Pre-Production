using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;

// Behaviour for the game object in the Map Scene corresponding to a node

public class MapNode : MonoBehaviour
{

    public int NodeId { get; set; }
    public OverworldMap.LocationType Type { get; set; }
    public int LocationId { get; set; }

    TravelPanel panel;
    Camera cam;
    Vector3 offset = new Vector3(0, 60, 0);


    private void Start()
    {
        cam = Camera.main;
        //EventManager.Instance.OnNodeMouseDown.AddListener(OtherNodeSelected);

        // Appearance is determined by node type
        if (Type == OverworldMap.LocationType.TOWN)
        {
            transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);

            Town t = DataTracker.Current.TownManager.GetTownById(LocationId);

            if (t.HasTag("Farm")){
                transform.Find("farm").gameObject.SetActive(true);
            }
            else if (t.Size == Town.Sizes.Small){
                    transform.Find("smallTown").gameObject.SetActive(true);

            }
            else if (t.Size == Town.Sizes.Medium){
                    transform.Find("town").gameObject.SetActive(true);

            }
            else if (t.Size == Town.Sizes.Large){
                    transform.Find("largeTown").gameObject.SetActive(true);
            }
            transform.Find("Indicator").gameObject.SetActive(true);


        }
        else if (Type == OverworldMap.LocationType.EVENT)
        {
            transform.Find("EncounterMark").gameObject.SetActive(true);
        }
        else if (Type == OverworldMap.LocationType.POI)
        {
            transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);
            transform.Find("tinyTown").gameObject.SetActive(true);
            transform.Find("Indicator").gameObject.SetActive(true);
        }
        else {
            transform.Find("Icon").gameObject.SetActive(true);
        }

        foreach(Transform child in transform) {
            if (!child.gameObject.activeInHierarchy && child.gameObject.name != "Icon"){
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Update the info panel's position so it is always on top of the node
    /// </summary>
    private void Update()
    {
        if (panel)
        {
            if (panel.gameObject.activeInHierarchy)
            {
                panel.transform.position = cam.WorldToScreenPoint(gameObject.transform.position) + offset;
            }
        }
    }

    /// <summary>
    /// Assosiate an info panel with this node
    /// </summary>
    /// <param name="obj">The info panel game object</param>
    public void setPanel(GameObject obj, bool showEnterButton)
    {
        if (panel) return;
        bool adjacent = DataTracker.Current.WorldMap.HasEdge(NodeId, DataTracker.Current.currentLocationId);
        if (Type == OverworldMap.LocationType.NONE && ! adjacent) return;

        panel = obj.GetComponent<TravelPanel>();
        panel.SetNode(this);
        obj.SetActive(true);

        if (showEnterButton){
            panel.EnableButton();
            panel.Select();
        }
        if (DataTracker.Current.WorldMap.HasEdge(NodeId, DataTracker.Current.currentLocationId)){
            panel.SetTravelInfo(MapTravel.GetFuelCost(this), MapTravel.dayRate);
        }

        if (Type == OverworldMap.LocationType.TOWN) {
            panel.SetDetails(DataTracker.Current.TownManager.GetTownById(LocationId).Name);
        }
        else if (Type == OverworldMap.LocationType.EVENT) {
            panel.SetDetails("Unknown Event");

        }
        else if (Type == OverworldMap.LocationType.POI) {
            panel.SetDetails("Point of Interest");
        }

        obj.transform.position = cam.WorldToScreenPoint(gameObject.transform.position) + offset;
        panel.onNodeHover();
        if (showEnterButton){
            panel.Select();
        }

    }

    /// <summary>
    /// Invoke an event on mouse over
    /// This tells the map UI to assign this node an info panel
    /// </summary>
    private void OnMouseEnter()
    {
        if (panel) return;
        EventManager.Instance.OnNodeMouseEnter.Invoke(this);
    }

    /// <summary>
    /// Close the panel when the mouse leaves, unless the node has been selected
    /// </summary>
    private void OnMouseExit()
    {
        if (panel)
        {
            panel.onNodeLeave();
        }
    }

    /// <summary>
    /// On click, select the node.
    /// </summary>
    private void OnMouseDown()
    {
        // Only allow clicks on adjacent nodes
        if (DataTracker.Current.WorldMap.HasEdge(NodeId, DataTracker.Current.currentLocationId))
        {
            if (panel)
            {
                EventManager.Instance.OnNodeMouseDown.Invoke(this);
                panel.onNodeClick();
            }
            else
            {
                // If the mouse is over the node as it becomes adjacent, there will not be a panel
                // So request a panel from Map UI
                EventManager.Instance.OnNodeMouseEnter.Invoke(this);
            }
        }

    }

    /// <summary>
    /// Remove the info panel
    /// </summary>
    public void Close()
    {
        panel = null;
    }



}
