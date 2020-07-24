using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Encounters;
using SIEvents;

public class OverworldMapUI : MonoBehaviour
{

    // Display settings
    [Header("Display Settings")]
    [SerializeField]
    float MapDrawScale = 1;

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

    [Header("Button")]
    [SerializeField]
    Button enterNodeButton;

    // Canvases
    [Header("Canvases")]
    [SerializeField]
    GameObject TownMenuGameObject;
    [SerializeField]
    GameObject EnterNodeButtonCanvas;
    [SerializeField]
    public GameObject QuestJournalCanvas;
    [SerializeField]
    Canvas TravelPanelCanvas;
    [SerializeField]
    public GameObject InventoryCanvas;

    //sounds
    [SerializeField]
    AudioClip Vroom;


    //Movement variables
    bool isActive = true;
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

    }

    private void RedrawMap()
    {
        Clear();
        DrawGraph();
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
            mn.LocationId = node.LocationId;

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
            Vector3 a = lineEnds[0] - 0.2f * (lineEnds[0] - lineEnds[1]);
            Vector3 b = lineEnds[1] - 0.2f * (lineEnds[1] - lineEnds[0]);
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
            node.setPanel(GetTravelPanel(), false);
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
        AudioSource audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        audioSource.PlayOneShot(Vroom, 2.0F);
        targetPos = selectedNode.gameObject.transform.position;
        isTravelling = true;
        MapTravel.Travel(selectedNode);
    }

    private void Update()
    {
        // Move the player
        if (isTravelling)
        {
            playerMarker.transform.position = Vector3.SmoothDamp(playerMarker.transform.position, targetPos, ref translatSmoothVelocity, translateSmoothTime);
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
                isTravelling = false;
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

            if (node.Type == OverworldMap.LocationType.TOWN || node.Type == OverworldMap.LocationType.POI)
            {
                selectedNode.setPanel(GetTravelPanel(), true);
            }
            else
            {
                enterNodeButton.gameObject.SetActive(false);
            }

            DataTracker.Current.currentLocationId = node.Id;
            DataTracker.Current.EventManager.OnNodeArrive.Invoke(node);
        }
    }

    public void OnButtonClick(int i)
    {
        TownMenuGameObject.GetComponent<TownWindow>().UpdatePrefab();
        TownMenuGameObject.SetActive(true);
        EnterNodeButtonCanvas.SetActive(false);
        isActive = false;
    }


    public void TownMapClosed()
    {
        TownMenuGameObject.SetActive(false);
        EnterNodeButtonCanvas.SetActive(true);
        isActive = true;
    }
}