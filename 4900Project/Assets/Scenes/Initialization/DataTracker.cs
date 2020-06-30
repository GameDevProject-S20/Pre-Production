using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//cribbed over from inventory as holdover until can link them properly
//public class ItemTester : IInventoryItem
//{
//    private string _itemName;
//    public string Name
//    {
//        get { return _itemName; }
//    }
//    private Money _value;
//    public Money Value
//    {
//        get { return _value; }
//        set { _value = value; }
//    }

//    public string SpecialField
//    {
//        get; set;
//    }

//    public ItemTester(string name, Money value)
//    {
//        this._itemName = name;
//        this._value = value;
//        this.SpecialField = "Special string";
//    }
//}

public class Trader
{
    public int Health;
    //public Inventory Items;

    public int CurrNodeId;

    public Trader(int hp, int cap, int cash)
    {
        Health = hp;
        //Items = new Inventory(cap, cash);
    }
}

public class DataTracker : MonoBehaviour
{
    public static DataTracker instance = null;


    private int startingCapacity = 10;
    private int startingHp = 100;
    private int starterCash = 100;
    public Trader player;
    int StartingNodeID = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        player = new Trader(startingHp, startingCapacity, starterCash);
        //Initialize:
        // Map, Event Manager, and so on
    
    }

    //public OverworldMap.LocationNode GetCurrentNode()
    //{
    //    OverworldMap.LocationNode node;
    //    if (WorldMap.GetNode(player.CurrNodeId, out node))
    //    {
    //        return node;
    //    }
    //    return null;
    //}

    private void Start()
    {
        Awake();


        /* Lets load in the town scene then the dialog for the first quest */
        SceneManager.LoadScene("InventoryTestScene", LoadSceneMode.Additive);

        SceneManager.LoadScene("EncounterTestScene", LoadSceneMode.Additive);



    }

    public void printAll()
    {
        Debug.Log("Tracker Full Print");
        Debug.Log("Health " + player.Health.ToString());
        //Debug.Log(player.Items.ToString());
    }
}