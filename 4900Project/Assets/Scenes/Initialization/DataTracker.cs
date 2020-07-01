using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    [SerializeField]
    public Sprite map_closed;
    public Sprite map_open;
    public Sprite journal_closed;
    public Sprite journal_open;
    public Sprite inventory_closed;
    public Sprite inventory_open;
    public GameObject HUDCanvasGameObject; 



    //internal information 
    private UIControl dialogueControler; 

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

    IEnumerator Start()
    {
        Awake();

        SceneManager.LoadScene("Town", LoadSceneMode.Additive);


        string currentTown = "Town name"; 

        IDButton endBtn = new DButton()
        {
            Text = "Done.",
            OnButtonClick = DFunctions.CloseDialogue
        };

        List<IDPage> dialoguePages = new List<IDPage>();
        List<IDButton> dialogueButtons = new List<IDButton>();
        dialogueButtons.Add(new DButton()
        {
            Text = "Continue",
            OnButtonClick = DFunctions.GoToNextPage
        });
        dialoguePages.Add(new DPage()
        {
            Text = "Welcome to Split Ends! \n\n You're in a town called " + currentTown + "Yada yada coolio stuffs.",
            Buttons = new List<IDButton>() {
                new DButton()
                {
                    Text = "Continue",
                    OnButtonClick = DFunctions.GoToNextPage
                }
            }
        });

        dialoguePages.Add(new DPage()
        {
            Text = "There's a rumor that a sherif need something in the town! We recomend checking them out. ",
            Buttons = new List<IDButton>() {
                new DButton()
                {
                    Text = "Okay!",
                    OnButtonClick = DFunctions.CloseDialogue
                }
            }
        });

        IDialogue dialogue = DialogueManager.CreateDialogue(dialoguePages);



        /* Lets load in the town scene then the dialog for the first quest */

        //SceneManager.LoadScene("EncounterTestScene", LoadSceneMode.Additive);
        yield return new WaitForSeconds(5f);
        Debug.Log("unload town after 5 secionds (done)");

        //SceneManager.UnloadSceneAsync("Town");


    }



    public void printAll()
    {
        Debug.Log("Tracker Full Print");
        Debug.Log("Health " + player.Health.ToString());
        //Debug.Log(player.Items.ToString());
    }



    /**********************************
     * HUD Button Clicks *
     **********************************/


    public void OnMapClick()
    {
        Image btn = (Image) GameObject.Find("MapButton").GetComponent("Image"); 

        if (btn.sprite == map_closed)
        {
            Debug.Log("Open up Map");
            btn.sprite = map_open;
        } else
        {
            Debug.Log("Closed the map"); 
            btn.sprite = map_closed;
        }


    }

    public void onJounralClick()
    {
        Image btn = (Image)GameObject.Find("JournalButton").GetComponent("Image");

        if (btn.sprite == journal_closed)
        {
            Debug.Log("Open up journal");
            btn.sprite = journal_open;
        }
        else
        {
            Debug.Log("Closed the JOURNAL");
            btn.sprite = journal_closed;
        }
    }

    public void onInventoryClick()
    {
        Image btn = (Image)GameObject.Find("InventoryButton").GetComponent("Image");

        if (btn.sprite == inventory_closed) 
        {
            Debug.Log("Open up inventory");
            SceneManager.LoadScene("InventoryTestScene", LoadSceneMode.Additive);

            btn.sprite = inventory_open;
        }
        else
        {
            Debug.Log("Closed the INVentory");
            SceneManager.UnloadSceneAsync("InventoryTestScene");
            btn.sprite = inventory_closed;
        }
    }




}