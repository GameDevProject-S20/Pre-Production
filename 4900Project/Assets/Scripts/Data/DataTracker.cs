using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;
using Quests;
using SIEvents;
using System.Linq;
using Dialogue;

public class DataTracker : MonoBehaviour
{
    private static DataTracker _current;
    public static DataTracker Current {get {return _current;}}

    public Player Player = Player.Instance;
    public OverworldMap.LocationGraph WorldMap = OverworldMapLoader.CreateTestMap();
    public QuestManager QuestManager = QuestManager.Instance;
    public QuestJournal QuestJournal = QuestJournal.Instance;
    public EncounterManager EncounterManager = EncounterManager.Instance;
    public DialogueManager DialogueManager = DialogueManager.Instance;
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

        Player.Inventory.AddItem("item1", 2);
        Player.Inventory.AddItem("item2", 8);
        Player.Inventory.AddItem("item4", 6);
        Player.Inventory.AddItem("item8", 3);
        Player.Inventory.AddItem("item5", 1);
        Player.Inventory.AddItem("item7", 6);

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

    // Useed for debugging
    //   See accompanying button in DataTracker scene
    public void PrintJournal()
    {
        Debug.Log(string.Format("[IN PROGRESS]\n\n{0}", string.Join("\n", QuestJournal.Instance.ActiveQuests.Select(q => q.ToString()))));
        Debug.Log(string.Format("[COMPLETE]\n\n{0}", string.Join("\n", QuestJournal.Instance.CompletedQuests.Select(q => q.ToString()))));
    }
}