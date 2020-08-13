﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;
using Quests;
using SIEvents;
using System.Linq;
using Dialogue;
using Assets.Scripts.Settings;
using UnityEngine.SceneManagement;

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
    public CampfireManager CampfireManager = CampfireManager.Instance;

    [SerializeField]
    public float MapSize;
    [SerializeField]
    //ARL Any of these "currents" should probably belong to Player
    //ARL Need to be capitalized
    public int currentShopId = 0; // Needed if we want store to be their own scene. If we make the store window a prefab, we don't need this. 
    public int currentLocationId = 1;

    public TravelType travelMode { get; private set; }

    public int dayCount = 0;
    public int hourCount = 6;
    private void Awake() {

        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } else {
            _current = this;
        }

        ItemManager.Current.Init();
        WorldMap = OverworldMapLoader.LoadMap();

        Player.Inventory.WeightLimit = 750f;

        TownManager.LoadData();
      
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

    private void OnTravelTypeChanged(TravelType type)
    {
        this.travelMode = type;
    }

    // Useed for debugging
    //   See accompanying button in DataTracker scene
    public void PrintJournal()
    {
        Debug.Log(string.Format("[IN PROGRESS]\n\n{0}", string.Join("\n", QuestJournal.Instance.ActiveQuests.Select(q => q.ToString()))));
        Debug.Log(string.Format("[COMPLETE]\n\n{0}", string.Join("\n", QuestJournal.Instance.CompletedQuests.Select(q => q.ToString()))));
    }

    public void IncrementTime(int i){
        hourCount += i;
        EventManager.OnTimeAdvance.Invoke(i);
        if (hourCount == 20) {
            EventManager.OnEvening.Invoke();
            CampfireManager.Instance.LoadCampfireScene();
        }
        if (hourCount >= 24){
            EventManager.OnDayAdvance.Invoke();
            hourCount = hourCount % 24;
            dayCount += 1;
        }

    }
}
