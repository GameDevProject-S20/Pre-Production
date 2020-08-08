using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCheats : MonoBehaviour
{
    private UnityEngine.Events.UnityEvent devModeEvent;

    void Start()
    {
        // Load Dev Mode Button
        var DevModeButton = Resources.Load("Prefabs/General/DeveloperModeButton");
        GameObject devmode = (GameObject)GameObject.Instantiate(DevModeButton);

        // Subscribe to Button onClick Event
        devModeEvent = devmode.GetComponentInChildren<UnityEngine.UI.Button>().onClick;

        devModeEvent.AddListener(devModeEnable);
        devModeEvent.AddListener(() => {GameObject.Find("Initializer").GetComponent<Initializer>().OnEnterGameClick();});
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
