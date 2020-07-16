using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestJournalWindow : MonoBehaviour
{
    
    Quests.Quest selectedQuest;

    UnityEngine.UIElements.TextElement NameField;
    UnityEngine.UIElements.TextElement DescriptionField;
    UnityEngine.UIElements.TextElement TaskField;

    // Start is called before the first frame update
    void Start()
    {
        // Set these to first active quest
        NameField = this.transform.Find("Quest Name").GetComponent<UnityEngine.UIElements.TextElement>();
        DescriptionField = this.transform.Find("Description").GetComponent<UnityEngine.UIElements.TextElement>();
        TaskField = this.transform.Find("Task Text").GetComponent<UnityEngine.UIElements.TextElement>();
        

        //get active and completed quests from quest journal
        // set Selected Quest to first active quest
        DataTracker.Current.QuestJournal.SyncQuests();
        selectedQuest = DataTracker.Current.QuestJournal.ActiveQuests[0];

        NameField.text = selectedQuest.Name;
        DescriptionField.text = selectedQuest.Description;
        TaskField.text = selectedQuest.

        // list the quests in the Quest Collection viewport

        // Have selected quest updated when a quest collection item is clicked
        // update fields
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
