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


    void OnEnable()
    {
        
        // Grab text fields from SceneGraph (THESE ARE Erroneous. too ambigious)
        NameField = GameObject.Find("Quest Name").GetComponent<UnityEngine.UI.Text>();
        DescriptionField = GameObject.Find("Description").GetComponent<UnityEngine.UI.Text>();
        TaskField = GameObject.Find("Task Text").GetComponent<UnityEngine.UI.Text>();
        QuestCollection = GameObject.Find("Quest List Content");

        // Load QuestItem resource
        QuestItem = Resources.Load<GameObject>("Prefabs/Quest/Item");

        // get active and completed quests from quest journal
        // set Selected Quest to first active quest
        DataTracker.Current.QuestJournal.SyncQuests();
        var activeQuests = DataTracker.Current.QuestJournal.ActiveQuests;
        var completedQuests = DataTracker.Current.QuestJournal.CompletedQuests;

        // Gross Code, to be thrown out because this would be better solved by creating QuestItems whenever a Quest is added to QuestManager 'via' events
        // This just iterates through active and completed quests, Creating QuestItems where the names don't match.
        if (activeQuests.Count > 0)
        {
            if (activeQuests[0] != null)
            {
                selectedQuest = activeQuests[0];
                updateFields(); 
            }

            foreach (Quest quest in activeQuests) 
            {
                if (GameObject.Find(quest.Name) == null) 
                {
                    instantiateQuestItem(quest);
                }
            }
        }

        if (completedQuests.Count > 0) 
        {
            foreach (Quest quest in completedQuests) 
            {
                if (activeQuests.Count == 0 && completedQuests.Count > 0)
                {
                    selectedQuest = completedQuests[0];
                    updateFields();
                }

                if (GameObject.Find(quest.Name) == null) 
                {
                    instantiateQuestItem(quest);
                }
            }
        }
    }

    private void updateFields()
    {
        NameField.text = selectedQuest.Name;
        DescriptionField.text = selectedQuest.Description;
        TaskField.text = string.Join("\n\n", selectedQuest.stages.ConvertAll(s => string.Format("[{0}] {1}\n\t{2}", (s.Complete) ? "✓" : " ", s.Description, string.Join("\n\t", s.conditions.ConvertAll(c => string.Format("{0} {1}", (c.IsSatisfied) ? "✓" : " ", c))))));
    }

    private void instantiateQuestItem(Quest quest)
    {
        var item = GameObject.Instantiate(QuestItem);
        item.GetComponentInChildren<UnityEngine.UI.Text>().text = quest.Name;            
        item.transform.SetParent(QuestCollection.transform, false);
        item.name = quest.Name;
    
        item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {focusQuest(quest.Name);});
    }

    public void focusQuest(string questName)
    {
        selectedQuest = DataTracker.Current.QuestManager.GetQuest(questName);
        updateFields();
    }

    public void toggleActive(bool displayed)
    {

    }

    public void toggleCompleted(bool displayed)
    {

    }

    public void disableUI()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
