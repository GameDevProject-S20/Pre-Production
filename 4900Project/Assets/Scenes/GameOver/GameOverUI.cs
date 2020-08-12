using Dialogue;
using Encounters;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public RawImage Background;


    private void Start()
    {
        
    }

    public void OnTryAgainClick()
    {
        SceneManager.LoadScene("InitializerScene", LoadSceneMode.Single);
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

}
