using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;

// Behaviour for the game object in the Map Scene corresponding to a node

public class MapNode : MonoBehaviour
{

    public int NodeId { get; set; }
    public OverworldMap.LocationType Type { get; set; }

    TravelPanel panel;
    Camera camera;
    Vector3 offset = new Vector3(0, 60, 0);


    private void Start()
    {
        camera = Camera.main;
        EventManager.Instance.OnNodeMouseDown.AddListener(OtherNodeSelected);

        // Appearance is determined by node type
        if (Type == OverworldMap.LocationType.TOWN)
        {
            transform.Find("Icon").gameObject.SetActive(false);
            transform.Find("TownMesh").gameObject.SetActive(true);
            transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);
        }
        else if (Type == OverworldMap.LocationType.EVENT)
        {
            transform.Find("Icon").gameObject.SetActive(false);
            transform.Find("EncounterMark").gameObject.SetActive(true);
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
                panel.transform.position = camera.WorldToScreenPoint(gameObject.transform.position) + offset;
            }
        }
    }

    /// <summary>
    /// Assosiate an info panel with this node
    /// </summary>
    /// <param name="obj">The info panel game object</param>
    public void setPanel(GameObject obj)
    {
        if (panel) return;
        panel = obj.GetComponent<TravelPanel>();
        panel.SetNode(this);
        obj.SetActive(true);
        panel.SetInfo(MapTravel.GetFuelCost(this), MapTravel.dayRate);
        obj.transform.position = camera.WorldToScreenPoint(gameObject.transform.position) + offset;
        panel.onNodeHover();

    }

    /// <summary>
    /// Invoke an event on mouse over
    /// This tells the map UI to assign this node an info panel
    /// </summary>
    private void OnMouseEnter()
    {
        if (panel) return;
        if (DataTracker.Current.WorldMap.HasEdge(NodeId, DataTracker.Current.currentLocationId))
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
    /// Close this node if another node is selected
    /// </summary>
    /// <param name="node">Node that was selected</param>
    private void OtherNodeSelected(MapNode node)
    {
        if (panel && node.NodeId != NodeId)
        {
            panel.onCancelButtonClick();
            Close();
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
