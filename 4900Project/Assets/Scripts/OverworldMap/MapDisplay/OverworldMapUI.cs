using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Encounters;
using SIEvents;
using Assets.Scripts.EscapeMenu.Interfaces;

public class OverworldMapUI : MonoBehaviour
{

    // Display settings
    [Header("Display Settings")]
    [SerializeField]
    float MapDrawScale = 1;
    [SerializeField]
    Material material;
    // Prefabs
    [Header("Prefabs")]
    [SerializeField]
    GameObject LocationPrefab;
    [SerializeField]
    GameObject PathPrefab;
    [SerializeField]
    GameObject travelPanelPrefab;

    // Player Marker
    [Header("Player Marker")]
    [SerializeField]
    GameObject playerMarker;
    [SerializeField]
    GameObject TruckObject;

    // Empty game objects to organize hierarchy
    [Header("Containers")]
    [SerializeField]
    Transform NodesContainer;
    [SerializeField]
    Transform PathsContainer;

    // Canvases & Windows
    [Header("Canvases")]
    [SerializeField]
    GameObject TownMenuGameObject;
    [SerializeField]
    public GameObject QuestJournalCanvas;
    [SerializeField]
    Canvas TravelPanelCanvas;
    [SerializeField]
    public GameObject InventoryCanvas;
    [SerializeField]
    SidePanel SidePanel;

    //sounds
    [SerializeField]
    AudioClip Vroom;

    //Movement variables
    bool isActive = true;
    int FreezeCount = 0; 
    bool isTravelling = false;
    float translateSmoothTime = 1f;
    Vector3 translatSmoothVelocity;
    Vector3 targetPos;

    // Node Selection
    MapNode selectedNode;

    // Travel UI pooling
    // To avoid repeated instantiation, we create 4 travel panels on start and reuse them
    public List<GameObject> travelPanelPool;

    // Path Highlighting
    [Header("Path Materials")]
    [SerializeField]
    Material pathDefaultMaterial;
    [SerializeField]
    Material pathHighlightMaterial;

    // Start is called before the first frame update
    void Start()
    {
        DrawGraph();
        Camera.main.transform.position = playerMarker.transform.position + new Vector3(0, 6, 0);

        // Instantiate a few travel info panel objects
        travelPanelPool = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = GameObject.Instantiate(travelPanelPrefab, TravelPanelCanvas.transform);
            obj.SetActive(false);
            travelPanelPool.Add(obj);

        }

        /// Add event listeners
        EventManager.Instance.RequestRedraw.AddListener(RedrawMap);
        EventManager.Instance.OnNodeMouseEnter.AddListener(onNodeMouseEnter);
        EventManager.Instance.OnNodeMouseDown.AddListener(onNodeMouseDown);
        EventManager.Instance.OnTravelStart.AddListener(onTravelStart);
        EventManager.Instance.OnEnterTownButtonClick.AddListener(OnButtonClick);
    
        EventManager.Instance.FreezeMap.AddListener(() => {
            isActive = false;
            FreezeCount++;
        });

        EventManager.Instance.UnfreezeMap.AddListener(() => {
            //if (!TownMenuGameObject.activeInHierarchy) isActive = true;
            FreezeCount--;
            if (FreezeCount <= 0)
            {
                FreezeCount = 0;
                isActive = true;
            } 
        });
        EventManager.Instance.OnDialogueEnd.AddListener(ShowSidePanel);

    }

    private void RedrawMap()
    {
        Clear();
        DrawGraph();
    }

    void ShowSidePanel(){
        if (!TownMenuGameObject.activeInHierarchy){
        OverworldMap.LocationNode node;
        DataTracker.Current.WorldMap.GetNode(DataTracker.Current.currentLocationId, out node);
        if (node.Type == OverworldMap.LocationType.POI || node.Type == OverworldMap.LocationType.TOWN){
            SidePanel.Open();
        }}
    }

    private void Clear()
    {
        foreach (Transform child in NodesContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in PathsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void DrawGraph()
    {
        int currentId = DataTracker.Current.currentLocationId;

        // Draw nodes
        foreach (var node in DataTracker.Current.WorldMap.GetNodeEnumerable())
        {
            GameObject nodeObj = Instantiate(LocationPrefab, transform.parent);
            Vector3 pos = new Vector3(node.PosX, 0, node.PosY) * DataTracker.Current.MapSize * MapDrawScale;
            nodeObj.transform.position += pos;
            nodeObj.transform.SetParent(NodesContainer, true);
            nodeObj.name = node.Name;
            MapNode mn = nodeObj.GetComponent<MapNode>();
            mn.NodeId = node.Id;
            mn.Type = node.Type;

            // Move the player to the starting node
            if (node.Id == currentId)
            {
                playerMarker.transform.position = nodeObj.transform.position;
                targetPos = playerMarker.transform.position;
            }
        }


        // Draw edges
        foreach (var edge in DataTracker.Current.WorldMap.GetEdgeEnumerable())
        {
            GameObject line = Instantiate(PathPrefab, transform.parent);
            line.name = "Edge_" + edge.Item1.Id + "_" + edge.Item2.Id + "_";
            LineRenderer lr = line.GetComponent<LineRenderer>();
            Vector3[] lineEnds =
                {
                    new Vector3(edge.Item1.PosX, 0, edge.Item1.PosY)* DataTracker.Current.MapSize * MapDrawScale,
                    new Vector3(edge.Item2.PosX, 0, edge.Item2.PosY)* DataTracker.Current.MapSize * MapDrawScale
                };
            Vector3 a = lineEnds[0] - 0.15f * (lineEnds[0] - lineEnds[1]);
            Vector3 b = lineEnds[1] - 0.15f * (lineEnds[1] - lineEnds[0]);
            lineEnds[0] = a;
            lineEnds[1] = b;
            lr.SetPositions(lineEnds);
            line.transform.SetParent(PathsContainer, true);
            if (edge.Item1.Id == DataTracker.Current.currentLocationId || edge.Item2.Id == DataTracker.Current.currentLocationId)
            {
                lr.material = pathHighlightMaterial;
            }
        }

    }

    /// <summary>
    /// Return an available travel info panel
    /// </summary>
    /// <returns>The next inactive travel info panel</returns>
    public GameObject GetTravelPanel()
    {
        foreach (var item in travelPanelPool)
        {
            if (!item.activeInHierarchy)
            {
                return item;
            }
        }
        return travelPanelPool[0];
    }

    /// <summary>
    /// When hovering over a node, display a travel info panel if we are not travelling.
    /// </summary>
    /// <param name="node"></param>
    void onNodeMouseEnter(MapNode node)
    {
        if (!isTravelling && isActive)
        {
            node.setPanel(GetTravelPanel());
        }
    }

    /// <summary>
    /// When clicking a node, select it unless we are travelling
    /// </summary>
    /// <param name="node"></param>
    void onNodeMouseDown(MapNode node)
    {
        if (!isTravelling && isActive)
        {
            selectedNode = node;
        }
    }

    /// <summary>
    /// Begin travel to a new node
    /// </summary>
    void onTravelStart()
    {
        float volume = 2.0F * DataTracker.Current.SettingsManager.VolumeMultiplier;

            AudioSource audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
            audioSource.PlayOneShot(Vroom, volume);
            targetPos = selectedNode.gameObject.transform.position;
            isTravelling = true;
    }

    private void Update()
    {
        material.SetVector("PlayerPosition", playerMarker.transform.position);
        // Move the player
        if (isTravelling)
        {
            // Divides by the VehicleSpeed multiplier - eg. If we want to double the speed, then we want to divide it by 2x (so it takes 0.5 seconds)
            playerMarker.transform.position = Vector3.SmoothDamp(playerMarker.transform.position, targetPos, ref translatSmoothVelocity, translateSmoothTime / DataTracker.Current.SettingsManager.VehicleSpeed);
            if (Vector3.Distance(playerMarker.transform.position, targetPos) > 0.2f)
            {
                Vector3 dir = ((targetPos - playerMarker.transform.position).normalized);
                float theta = Vector2.SignedAngle(new Vector2(dir.x, dir.z), Vector2.left);
                if (TruckObject) TruckObject.transform.eulerAngles = new Vector3(-90, 0, theta);
            }
            // On Arrival
            else
            {
                OnNodeArrival();
            }
        }
    }

    /// <summary>
    /// Upon arriving at a node...
    /// ...enable the 'Enter Node' button
    /// ...trigger any encounters
    /// ...trigger an event
    /// </summary>
    void OnNodeArrival()
    {
        isTravelling = false;
        OverworldMap.LocationNode node;
        if (DataTracker.Current.WorldMap.GetNode(selectedNode.NodeId, out node))
        {

            foreach (Transform path in PathsContainer.transform)
            {
                if (path.gameObject.name.Contains("_" + DataTracker.Current.currentLocationId.ToString() + "_"))
                {
                    path.GetComponent<LineRenderer>().material = pathDefaultMaterial;
                }
                if (path.gameObject.name.Contains("_" + node.Id.ToString() + "_"))
                {
                    path.GetComponent<LineRenderer>().material = pathHighlightMaterial;
                }
            }

            if (node.Type == OverworldMap.LocationType.TOWN)
            {
                SidePanel.OpenTown(node.LocationId);
            }
            else if (node.Type == OverworldMap.LocationType.POI){
                SidePanel.OpenPOI(node.LocationId);
            }

            DataTracker.Current.currentLocationId = node.Id;
            DataTracker.Current.EventManager.OnNodeArrive.Invoke(node);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000)){
            MapNode mn = hit.transform.gameObject.GetComponent<MapNode>();
            if (mn) {
                Debug.Log(hit.transform.gameObject.name);
                mn.OnMouseEnter();
            }
        }
    }

    public void OnButtonClick(int i)
    {
        TownMenuGameObject.GetComponent<TownWindow>().UpdatePrefab();
        TownMenuGameObject.SetActive(true);
        EventManager.Instance.FreezeMap.Invoke();
    }


    public void TownMapClosed()
    {
        TownMenuGameObject.SetActive(false);
        SidePanel.Open();
        EventManager.Instance.UnfreezeMap.Invoke();
    }
}