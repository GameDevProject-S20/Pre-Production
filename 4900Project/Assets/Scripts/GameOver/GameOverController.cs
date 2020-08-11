using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataTracker.Current.EventManager.OnHealthChange.AddListener(CheckGameOverState);

    }


    public void CheckGameOverState(int a, int b, int c, string d)
    {
        Debug.Log("Called GameOver Check State"); 
        if (DataTracker.Current.Player.Health <= 0)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }


}


