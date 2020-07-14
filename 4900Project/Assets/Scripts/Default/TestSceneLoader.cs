using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoader : MonoBehaviour
{
    private void Start() {
        SceneManager.LoadScene("DataTracker", LoadSceneMode.Additive);
    }
    public void load(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
