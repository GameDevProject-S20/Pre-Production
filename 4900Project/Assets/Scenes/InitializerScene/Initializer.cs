using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Initializer : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("DataTracker", LoadSceneMode.Additive);
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
    }
}
