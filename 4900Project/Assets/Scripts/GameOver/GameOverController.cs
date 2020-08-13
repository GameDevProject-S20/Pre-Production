using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverController : MonoBehaviour

{
    [SerializeField]
    [Header("Trigger a -10000 health event")]
    public bool triggerGameOver;

    // Start is called before the first frame update
    void Start()
    {
        triggerGameOver = false; 
        DataTracker.Current.EventManager.OnHealthChange.AddListener(CheckGameOverState);

    }

    private void Update()
    {
        if (triggerGameOver)
        {
            DataTracker.Current.Player.AddHealth(-10000);
            triggerGameOver = false; 
        }
    }


    public void CheckGameOverState(int a, int b, int c, string d)
    {
        Debug.Log("Called GameOver Check State"); 
        if (DataTracker.Current.Player.Health <= 0)
        {
            if (DataTracker.Current.DialogueManager.GetActiveDialogue() != null)
                DataTracker.Current.DialogueManager.GetActiveDialogue().Hide();

            //GameObject.Destroy(GameObject.Find("DataTracker"));
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }


}


