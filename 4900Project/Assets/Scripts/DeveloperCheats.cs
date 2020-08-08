using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCheats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Add Edge for Skipping Tutorial
        DataTracker.Current.WorldMap.AddEdge(119, 124);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
