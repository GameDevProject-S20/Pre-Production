using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;

public class QuestJournalWindow : MonoBehaviour
{

    [SerializeField] 
    GameObject ActiveCategoryButton;

    [SerializeField]
    GameObject CompletedCategoryButton;
    Quest selectedQuest;

    private UnityEngine.UI.Text NameField, DescriptionField, TaskField;

    private GameObject QuestItem, QuestCollection;

    private bool displayCompletedQuests = true;
    private bool displayActiveQuests = true;

    private GameObject activeToggleButton;
    private GameObject completedToggleButton;
    private List<GameObject> activeQuestItems = new List<GameObject>();
    private List<GameObject> completedQuestItems = new List<GameObject>();



    void OnEnable()
    {
        
        // Grab text fields from SceneGraph (THESE ARE Erroneous. too ambigious)
        NameField = GameObject.Find("Quest Name").GetComponent<UnityEngine.UI.Text>();
        NameField.text = "";
        DescriptionField = GameObject.Find("Description").GetComponent<UnityEngine.UI.Text>();
        DescriptionField.text = "";
        TaskField = GameObject.Find("Task Text").GetComponent<UnityEngine.UI.Text>();
        TaskField.text = "";
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
        activeToggleButton = GameObject.Instantiate(ActiveCategoryButton, QuestCollection.transform);
        activeToggleButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(toggleActive);
        if (activeQuests.Count > 0)
        {
            if (activeQuests[0] != null)
            {
                selectedQuest = activeQuests[0];
                updateFields(); 
            }

                foreach (Quest quest in activeQuests) 
                {
                        activeQuestItems.Add(
                            instantiateQuestItem(quest)
                        );
                }
        }

        completedToggleButton = GameObject.Instantiate(CompletedCategoryButton, QuestCollection.transform);
        completedToggleButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(toggleCompleted);
        if (completedQuests.Count > 0) 
        {

                foreach (Quest quest in completedQuests) 
                {
                        completedQuestItems.Add(
                            instantiateQuestItem(quest)
                        );
                }
        }
    }

    private void updateFields()
    {
        NameField.text = selectedQuest.Name;
        DescriptionField.text = selectedQuest.Description;
        TaskField.text = string.Join("\n\n", selectedQuest.stages.ConvertAll(s => string.Format("[{0}] {1}\n\t{2}", (s.Complete) ? "✓" : " ", s.Description, string.Join("\n\t", s.conditions.ConvertAll(c => string.Format("{0} {1}", (c.IsSatisfied) ? "✓" : " ", c))))));
    }

    private GameObject instantiateQuestItem(Quest quest)
    {
        var item = GameObject.Instantiate(QuestItem);
        item.GetComponentInChildren<UnityEngine.UI.Text>().text = quest.Name;            
        item.transform.SetParent(QuestCollection.transform, false);
        item.name = quest.Name;
    
        item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {focusQuest(quest.Name);});

        return item;
    }

    public void focusQuest(string questName)
    {
        selectedQuest = DataTracker.Current.QuestManager.GetQuest(questName);
        updateFields();
    }

    public void toggleActive()
    {
        displayActiveQuests = !displayActiveQuests;
        foreach (GameObject item in activeQuestItems){
            item.SetActive(displayActiveQuests);
        }
    }

    public void toggleCompleted()
    {
        displayCompletedQuests = !displayCompletedQuests;
        foreach (GameObject item in completedQuestItems){
            item.SetActive(displayCompletedQuests);
        }
    }

    public void disableUI()
    {
        transform.parent.gameObject.SetActive(false);
        GameObject.Destroy(completedToggleButton);
        GameObject.Destroy(activeToggleButton);
        foreach(GameObject Item in activeQuestItems)
        {
            Destroy(Item);
        }
        activeQuestItems = new List<GameObject>();
        foreach(GameObject Item in completedQuestItems)
        {
            Destroy(Item);
        }
        completedQuestItems = new List<GameObject>();
    }
}
