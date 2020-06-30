using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class StartMenuButtonHandler : MonoBehaviour
{
    [SerializeField]
    public string StartGameScene = "GameInitialization";
    //public string LoadGameScene = "Not Used";

    public void OnNewGamePress(){

        SceneManager.LoadScene(StartGameScene, LoadSceneMode.Single);
    }

    /*public void OnLoadGamePress(){
        // TODO - What do we do here? There's no loading yet
        Debug.Log("Loading Game");

        SceneManager.LoadScene(LoadGameScene, LoadSceneMode.Single);
    }
    */ 
    public void OnQuitPress(){
        // Application.Quit() will close the application
        Application.Quit();
    }
}
