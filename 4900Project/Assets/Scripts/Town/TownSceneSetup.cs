using FileConstants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TownSceneSetup : MonoBehaviour
{
    //Currently just sets up dummy box to see if towns/townmanager is working
    //TO DO
    //-Once dialogue. etc implemented, create contact function
    //-Once shops are completed, display shops
    //-UI design for final product
    //-implement universal buttons
    //-Once data tracker done, have it always pull current town id
    //-Once quests setup, reintergrate
    [SerializeField]
    Text textBox;

    private Town townData;

    //placeholder int
    public int currentTown = 1;
    // Start is called before the first frame update
    void Start()
    {
        townData = TownManager.current.GetTownById(currentTown);
        Setup();

    }

    void Setup()
    {
        textBox.text = "Town: " + townData.Name + "(" + townData.Id + ")\n" + "Leader: " + townData.Leader + "\nCurrent Reputation Value: " + townData.Reputation + "\nShop IDs: \n";
        for(int i = 0; i < townData.shops.Count; i++)
        {
            textBox.text = textBox.text + townData.shops[i] + "\n";
        }
    }

    public void Shift(int i)
    {
        currentTown += i;
        if (currentTown < 0)
        {
            currentTown = 0;
        }
        if(currentTown > 4)
        {
            currentTown = 4;
        }
        townData = TownManager.current.GetTownById(currentTown);
        Setup();
    }
}
