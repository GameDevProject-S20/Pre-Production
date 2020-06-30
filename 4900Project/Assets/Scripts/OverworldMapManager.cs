using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMapManager : MonoBehaviour
{
    public OverworldMap.LocationGraph Graph { get; } = new OverworldMap.LocationGraph();

    public GameObject uiPrefab;
    private OverworldMapUI view;

    void Start()
    {
        view = Instantiate(uiPrefab, transform).GetComponent<OverworldMapUI>();
        view.SetGraph(Graph);
    }

    public void RequestDraw()
    {
        view.QueueDraw();
    }
}
