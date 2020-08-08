using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCheats : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        // Load Dev Mode Button
        var DevModeButton = Resources.Load("Prefabs/General/DeveloperModeButton");
        GameObject devmode = (GameObject)GameObject.Instantiate(DevModeButton);

        // Add Dev Mode Functionality
        devmode.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(devModeEnable);
        // Add Button Functionality
        devmode.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => {GameObject.Find("Initializer").GetComponent<Initializer>().OnEnterGameClick();});
    }

    public void devModeEnable()
    {
        //Add Edge for Skipping Tutorial
        DataTracker.Current.WorldMap.AddEdge(119, 124);
    }

    void devModeDisable(){
        DataTracker.Current.WorldMap.RemoveEdge(119, 124);
    }
}
