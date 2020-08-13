using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;

// Behaviour for the game object in the Map Scene corresponding to a node
// This class handles the visual appearence of nodes, mouse events, and partialy the info panels
public class MapNode : MonoBehaviour
{


    public OverworldMap.LocationNode NodeData { get; set; }
    EncounterNode encounterData;
    
    // Panel
    TravelPanel panel;
    Vector3 offset = new Vector3(0, 60, 0);
    Camera cam;

    enum ColourModes {Default, Probability}
    ColourModes colourMode = ColourModes.Default;

    public void Init(OverworldMap.LocationNode Node){
        NodeData = Node;
        cam = Camera.main;
    
        // Appearance is determined by node type
        // Towns are represented by a mesh
        if (NodeData.Type == OverworldMap.LocationType.TOWN)
        {
            transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);

            Town t = DataTracker.Current.TownManager.GetTownById(NodeData.LocationId);

            if (t.HasTag("Farm"))
            {
                transform.Find("farm").gameObject.SetActive(true);
            }
            else if (t.Size == Town.Sizes.Small)
            {
                transform.Find("smallTown").gameObject.SetActive(true);

            }
            else if (t.Size == Town.Sizes.Medium)
            {
                transform.Find("town").gameObject.SetActive(true);

            }
            else if (t.Size == Town.Sizes.Large)
            {
                transform.Find("largeTown").gameObject.SetActive(true);
            }
        }

        // Points of interest are represetned by an '!'
        else if (NodeData.Type == OverworldMap.LocationType.POI)
        {
            transform.Find("EncounterMark").gameObject.SetActive(true);
        }

        // Empty nodes have a shape which corresponds to the chance of triggering an encounter
        else
        {
            encounterData = new EncounterNode();
            encounterData.Init(NodeData.Id);
            encounterData.SampleTexture(NodeData.PosX, NodeData.PosY);
            transform.Find("Icon").gameObject.SetActive(true);
            SetSprite();
        }

        // Listeners
        EventManager.Instance.OnProbabilityChange.AddListener((int id)=> {
            if (id == this.NodeData.Id){
                SetSprite();
                if (colourMode == ColourModes.Probability) {
                    ColourByProbability();
                }
            }
        });

        EventManager.Instance.SetViewDefault.AddListener(ColourToBlack);
        EventManager.Instance.SetViewProbability.AddListener(ColourByProbability);
    }

    private void OnDestroy() {
        
        EventManager.Instance.SetViewDefault.RemoveListener(ColourToBlack);
        EventManager.Instance.SetViewProbability.RemoveListener(ColourByProbability);
        if (encounterData != null) encounterData.Delete();
    }

    void SetSprite(){
        if (NodeData.Type == OverworldMap.LocationType.NONE) {
            SpriteRenderer sprite =  transform.Find("Icon").GetComponent<SpriteRenderer>();
            switch (encounterData.p)
            {
                case "Low":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/hexagon");
                break;
                case "Moderate":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/diamond");
                break;
                case "High":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/triangle");
                break;
                case "Very High":
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/cross");
                break;
                default:
                    sprite.sprite = Resources.Load<Sprite>("Sprites/Map/Nodes/circle");
                break;
            }
        }
    }

    void ColourByProbability(){
    if (NodeData.Type == OverworldMap.LocationType.NONE) {
        colourMode = ColourModes.Probability;
        // gradient stuff
            Gradient g = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;
            colorKey = new GradientColorKey[2];
            colorKey[0].color = GameObject.Find("Map").GetComponent<OverworldMapUI>().color1 / 255.0f;
            colorKey[0].time = 0.0f;
            colorKey[1].color = GameObject.Find("Map").GetComponent<OverworldMapUI>().color2 / 255.0f;
            colorKey[1].time = 1.0f;
        
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            g.SetKeys(colorKey, alphaKey);
        
            SpriteRenderer sprite =  transform.Find("Icon").GetComponent<SpriteRenderer>();
            switch (encounterData.p)
            {
                case "Low":
                    sprite.color = g.Evaluate(0.25f);
                break;
                case "Moderate":
                    sprite.color = g.Evaluate(0.5f);
                break;
                case "High":
                    sprite.color = g.Evaluate(0.75f);
                break;
                case "Very High":
                    sprite.color = g.Evaluate(1.0f);
                break;
                default:
                    sprite.color = g.Evaluate(0.0f);
                break;
            }
        }
    }

    void ColourToBlack(){
        if (NodeData.Type == OverworldMap.LocationType.NONE) {
            colourMode = ColourModes.Default;
            SpriteRenderer sprite =  transform.Find("Icon").GetComponent<SpriteRenderer>();
            sprite.color = Color.black; 
        }
    }

    /// <summary>
    /// Roll the probability on a node to see if an encounter is triggered
    /// </summary>
    /// <returns>True if an encounter was triggered</returns>
    public bool RollEncounter(){
        return encounterData.RollEncounter();
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
    /// Takes an info panel and fills it with this node's data
    /// </summary>
    /// <param name="obj">The info panel game object</param>
    public void setPanel(GameObject obj)
    {
        if (panel || obj == null) return;
        bool adjacent = DataTracker.Current.WorldMap.HasEdge(NodeData.Id, DataTracker.Current.currentLocationId);

        panel = obj.GetComponent<TravelPanel>();
        panel.SetNode(this);
        obj.SetActive(true);

        // Show travel costs for adjacent panels
        if (adjacent){
            panel.SetTravelInfo(MapTravel.GetFuelCost(this), MapTravel.CaravanTravelRate);
        }

        // For towns, the info panel shows the town's tags
        if (NodeData.Type == OverworldMap.LocationType.TOWN) {
            Town t = DataTracker.Current.TownManager.GetTownById(NodeData.LocationId);
            panel.SetName(t.Name);
            string details = "";
            if (t.Tags.Count > 0)
            {
                foreach (var tag in t.Tags)
                {
                    if (tag.Name == "Small" || tag.Name == "Medium" || tag.Name == "Large")
                    {
                        continue;
                    }
                    details += tag.Name + " ";
                }
            }

            switch (t.Size)
            {
                case Town.Sizes.Small:
                    details += "Hamlet";
                    break;
                case Town.Sizes.Medium:
                    details += "Town";
                    break;
                case Town.Sizes.Large:
                    details += "City";
                    break;
                default:
                    details += "Town";
                    break;
            }
            panel.SetDetails(details);
            //THIS PLUS GIVING THE DETAILS TEXT IN THE PREFAB THE TOOLTIP SPRITESHEET SHOWS THE BUY/SELL DIFFS IN TOOLTIP
            //panel.AddTownTags(t.Tags);
        }

        // Empty nodes show chance of event
        else if (NodeData.Type == OverworldMap.LocationType.NONE) {
            if (encounterData.p == "Safe"){
                panel.SetDetails("Safe to travel");
            }
            else{
                panel.SetDetails($"{encounterData.p} chance of event");
            }

        }
        else if (NodeData.Type == OverworldMap.LocationType.POI) {
            panel.SetDetails("Point of Interest");
        }

        obj.transform.position = cam.WorldToScreenPoint(gameObject.transform.position) + offset;
        panel.onNodeHover();
    }

    /// <summary>
    /// Invoke an event on mouse over
    /// This requests the map UI controller to give us an info panel
    /// </summary>
    public void OnMouseEnter()
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
        if (DataTracker.Current.WorldMap.HasEdge(NodeData.Id, DataTracker.Current.currentLocationId))
        {
            if (panel)
            {
                EventManager.Instance.OnNodeMouseDown.Invoke(this);
                panel.onNodeClick();
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
