using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;
using Quests;

public class DataTracker : MonoBehaviour
{
    private static DataTracker _current;
    public static DataTracker Current {get {return _current;}}

    public PlayerData Player = new PlayerData();
    public OverworldMap.LocationGraph WorldMap = OverworldMapLoader.CreateTestMap();
    public QuestManager QuestManager = QuestManager.Instance;
    public EncounterManager EncounterManager = EncounterManager.Instance;
    public EventManager EventManager = EventManager.Instance;
    public TownManager TownManager = TownManager.Instance;
    public ShopManager ShopManager = ShopManager.Instance;

    [SerializeField]
    public int currentShopId = 0; // Needed if we want store to be their own scene. If we make the store window a prefab, we don't need this.
    public int currentLocationId = 0;

    private void Awake() {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }

        Player.Inventory.addItem("item1", 2);
        Player.Inventory.addItem("item2", 8);
        Player.Inventory.addItem("item4", 6);
        Player.Inventory.addItem("item8", 3);
        Player.Inventory.addItem("item5", 1);
        Player.Inventory.addItem("item7", 6);

        ShopManager.LoadData();
        TownManager.LoadData();

        DontDestroyOnLoad(gameObject);
    }

    public OverworldMap.LocationNode GetCurrentNode(){
        OverworldMap.LocationNode node;
        if (WorldMap.GetNode(currentLocationId, out node)){
            return node;
        }
        return null;
    }

    public void PrintQuest()
    {
        Debug.Log(QuestManager.GetActiveQuest());
        Debug.Log(string.Format("Active Handlers: [{0}]\n{1}", EventManager.OnTransactionHandlers.Count, string.Join("\n", EventManager.OnTransactionHandlers)));
    }
}

public class PlayerData {
    public Inventory Inventory = new Inventory();
}
