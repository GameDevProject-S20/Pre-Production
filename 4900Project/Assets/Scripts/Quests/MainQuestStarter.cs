using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;
using Quests;
public class MainQuestStarter : MonoBehaviour
{
    private void Start() {
        EventManager.Instance.OnQuestComplete.AddListener(Ready);
                EventManager.Instance.OnQuestComplete.AddListener((Quest q)=>{
                    Debug.Log("test");
                });

    }

    void Ready(Quest q){
        if (q.Name == "Field Work"){
            EventManager.Instance.OnTownLeave.AddListener(StartQuest);
            EventManager.Instance.OnQuestComplete.RemoveListener(Ready);
        }
    }

    void StartQuest(){
        EventManager.Instance.OnEncounterTrigger.Invoke(219);
        EventManager.Instance.OnTownLeave.RemoveListener(StartQuest);
    }
}
