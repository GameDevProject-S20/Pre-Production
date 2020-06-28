using System.Collections;
using UnityEngine;
using Encounters;


public class EncounterInvoker : MonoBehaviour
{
    void Start()
    {
        Debug.Log("foobar");
        var mgr = RandomEncounterManager.Instance;
    }
}
