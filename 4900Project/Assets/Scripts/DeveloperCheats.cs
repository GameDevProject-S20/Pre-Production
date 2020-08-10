using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;

public class DeveloperCheats : MonoBehaviour
{
    private UnityEngine.Events.UnityEvent devModeEvent;

    private SIEvents.Events.MapEvents.MapNodeEvent hoverNodeEvent;

    private int[] tempEdge = {0, 0};

    void Start()
    {
        loadDevModeButton();
    }

    private void loadDevModeButton(){
        var DevModeButton = Resources.Load("Prefabs/General/DeveloperModeButton");
        GameObject devmode = (GameObject)GameObject.Instantiate(DevModeButton);

        devModeEvent = devmode.GetComponentInChildren<UnityEngine.UI.Button>().onClick;
        devModeEvent.AddListener(devModeEnable);
        devModeEvent.AddListener(() => {GameObject.Find("Initializer").GetComponent<Initializer>().OnEnterGameClick();});
    }

    public void devModeEnable()
    {
        hoverNodeEvent = DataTracker.Current.EventManager.OnNodeMouseEnter;
        hoverNodeEvent.AddListener(TemporaryDevEdge);
    }

    void devModeDisable(){
        hoverNodeEvent.RemoveListener(TemporaryDevEdge);
    }

    void TemporaryDevEdge(MapNode node){
        OverworldMap.LocationGraph map = DataTracker.Current.WorldMap;
        if (map.HasEdge(tempEdge[0], tempEdge[1]))
            map.RemoveEdge(tempEdge[0], tempEdge[1]);

        int current = DataTracker.Current.GetCurrentNode().Id;
        int target = node.NodeData.Id;
        tempEdge[0] = current;
        tempEdge[1] = target;

        if (!(map.HasEdge(tempEdge[0], tempEdge[1])))
            map.AddEdge(tempEdge[0], tempEdge[1]);
    }
}
