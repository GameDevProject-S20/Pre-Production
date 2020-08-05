using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;
using Quests;
using SIEvents;
using System.Linq;
using Dialogue;
using Assets.Scripts.Settings;

[System.Serializable]
public class DataTracker : MonoBehaviour
{
    public enum TravelType
    {
        TRUCK,
        WALK
    }

    private static DataTracker _current;
    public static DataTracker Current {get {return _current;}}

    public Player Player = Player.Instance;
    public OverworldMap.LocationGraph WorldMap;
    public QuestManager QuestManager = QuestManager.Instance;
    public QuestJournal QuestJournal = QuestJournal.Instance;
    [SerializeField]
    public EncounterManager EncounterManager = EncounterManager.Instance;
    public DialogueManager DialogueManager = DialogueManager.Instance;
    public EventManager EventManager = EventManager.Instance;
    public TownManager TownManager = TownManager.Instance;
    public ShopManager ShopManager = ShopManager.Instance;
    public SettingsManager SettingsManager = SettingsManager.Instance;
    [SerializeField]
    public float MapSize;
    [SerializeField]
    //ARL Any of these "currents" should probably belong to Player
    //ARL Need to be capitalized
    public int currentShopId = 0; // Needed if we want store to be their own scene. If we make the store window a prefab, we don't need this. 
    public int currentLocationId = 1;

    public TravelType CurrentTravelType { get; private set; }

    public int dayCount = 0;

    private void Awake() {

        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }

        WorldMap = OverworldMapLoader.LoadMap();
        ShopManager.LoadData();
        TownManager.LoadData();

        Player.Inventory.WeightLimit = 10000f;
        Player.Inventory.AddItem("Rations", 12);
        Player.Inventory.AddItem("Fuel", 30);
        Player.Inventory.AddItem("Fresh Fruit", 1);
        Player.Inventory.AddItem("Scrap Metal", 9);
        Player.Inventory.AddItem("Wrench", 1);

        CurrentTravelType = TravelType.TRUCK;
        EventManager.OnInventoryChange.AddListener(() => OnInventoryChangedHandler());

        DontDestroyOnLoad(gameObject);
        EventManager.onDataTrackerLoad.Invoke();
    }

    public OverworldMap.LocationNode GetCurrentNode(){
        OverworldMap.LocationNode node;
        if (WorldMap.GetNode(currentLocationId, out node)){
            return node;
        }
        return null;
    }

    private void OnInventoryChangedHandler()
    {
        int fuelAmount = Player.Instance.Inventory.Contains("Fuel");

        if (CurrentTravelType == TravelType.TRUCK && fuelAmount <= 0)
        {
            CurrentTravelType = TravelType.WALK;
            EventManager.Instance.OnTravelTypeChanged.Invoke(CurrentTravelType);
        }
        else if (CurrentTravelType == TravelType.WALK && fuelAmount > 0)
        {
            CurrentTravelType = TravelType.TRUCK;
            EventManager.Instance.OnTravelTypeChanged.Invoke(CurrentTravelType);
        }
    }

    // Useed for debugging
    //   See accompanying button in DataTracker scene
    public void PrintJournal()
    {
        Debug.Log(string.Format("[IN PROGRESS]\n\n{0}", string.Join("\n", QuestJournal.Instance.ActiveQuests.Select(q => q.ToString()))));
        Debug.Log(string.Format("[COMPLETE]\n\n{0}", string.Join("\n", QuestJournal.Instance.CompletedQuests.Select(q => q.ToString()))));
    }
}
