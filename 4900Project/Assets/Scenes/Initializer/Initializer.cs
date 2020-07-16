using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Initializer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(loader());
    }

    IEnumerator loader(){
        SceneManager.LoadScene("DataTracker", LoadSceneMode.Additive);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
    }
}
