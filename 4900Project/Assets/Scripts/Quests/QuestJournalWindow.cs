using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;

public class QuestJournalWindow : MonoBehaviour
{
    
    Quest selectedQuest;

    UnityEngine.UI.Text NameField;
    UnityEngine.UI.Text DescriptionField;
    UnityEngine.UI.Text TaskField;

    GameObject QuestItem;
    GameObject QuestCollection;


    // Start is called before the first frame update

    void OnEnable()
    {
        // Set these to first active quest
        NameField = GameObject.Find("Quest Name").GetComponent<UnityEngine.UI.Text>();
        DescriptionField = GameObject.Find("Description").GetComponent<UnityEngine.UI.Text>();
        TaskField = GameObject.Find("Task Text").GetComponent<UnityEngine.UI.Text>();
        QuestCollection = GameObject.Find("Content");

        QuestItem = Resources.Load<GameObject>("Prefabs/Quest/Item");
        

        //get active and completed quests from quest journal
        // set Selected Quest to first active quest
        DataTracker.Current.QuestJournal.SyncQuests();
        var activeQuests = DataTracker.Current.QuestJournal.ActiveQuests;
        var completedQuests = DataTracker.Current.QuestJournal.CompletedQuests;

        if (activeQuests[0] != null){
            selectedQuest = activeQuests[0];

            NameField.text = selectedQuest.Name;
            DescriptionField.text = selectedQuest.Description;
            TaskField.text = string.Join("\n\n", selectedQuest.stages.ConvertAll(s => string.Format("[{0}] {1}\n\t{2}", (s.Complete) ? "✓" : " ", s.Description, string.Join("\n\t", s.conditions.ConvertAll(c => string.Format("{0} {1}", (c.IsSatisfied) ? "✓" : " ", c))))));
        }
           
        foreach (Quest quest in activeQuests) {

            if (GameObject.Find(quest.Name) == null) {
                var item = GameObject.Instantiate(QuestItem);
                item.GetComponentInChildren<UnityEngine.UI.Text>().text = quest.Name;            
                item.transform.SetParent(QuestCollection.transform, false);
                item.name = quest.Name;
            
                item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {focusQuest(quest.Name);});
            }
        }

        foreach (Quest quest in completedQuests) {

            if (GameObject.Find(quest.Name) == null) {
                var item = GameObject.Instantiate(QuestItem);
                item.GetComponentInChildren<UnityEngine.UI.Text>().text = quest.Name;            
                item.transform.SetParent(QuestCollection.transform, false);
                item.name = quest.Name;
            
                item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {focusQuest(quest.Name);});
            }
        }
    }

    public void focusQuest(string questName)
    {
        selectedQuest = DataTracker.Current.QuestManager.GetQuest(questName);

        NameField.text = selectedQuest.Name;
        DescriptionField.text = selectedQuest.Description;
        TaskField.text = string.Join("\n\n", selectedQuest.stages.ConvertAll(s => string.Format("[{0}] {1}\n\t{2}", (s.Complete) ? "✓" : " ", s.Description, string.Join("\n\t", s.conditions.ConvertAll(c => string.Format("{0} {1}", (c.IsSatisfied) ? "✓" : " ", c))))));
    }

    public void disableUI()
    {
        transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
