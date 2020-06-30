using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class StartMenuButtonHandler : MonoBehaviour
{
    public static UnityEvent OnFinishLoading = new UnityEvent();

    private void Start() {
        OnFinishLoading.AddListener(Init);
    }


    public void OnNewGamePress(){

        SceneManager.LoadScene("DefaultScene", LoadSceneMode.Single);
    }

    public void Init(){

        /*Quest medicineQuest = Quest.MakeMedicineQuest();
        QuestManager.current.addQuest(medicineQuest);

        // Wants to load the Town Menu as the background scene
        SceneManager.LoadScene("TownMenu", LoadSceneMode.Single);

        // Add in the tutorial text on top
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Additive);

        Tutorial.TutorialCompleted.AddListener(() =>
        {
            QuestManager.current.startQuest(medicineQuest.name);

            // Load the quest dialog
            SceneManager.LoadScene("MedicineQuestScene", LoadSceneMode.Additive);
            MedicineQuestDialog.DialogCompleted.AddListener(() =>
            {
                // give money for medicine
                DataTracker.instance.player.Items.Cash += 50;
            });
        });*/
    }

    public void OnLoadGamePress(){
        // TODO - What do we do here? There's no loading yet
        Debug.Log("Loading Game");
    }

    public void OnQuitPress(){
        // Application.Quit() will close the application
        Application.Quit();
    }
}
